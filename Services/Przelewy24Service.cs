using ApiClientPrzelewy24.Clients;
using ApiClientPrzelewy24.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Refit;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace ApiClientPrzelewy24.Services
{
    public class Przelewy24Service
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IPrzelewy24SignatureProvider _signatureProvider;
        private readonly ILogger<Przelewy24Service> _logger;

        // Fallbacks for local testing only
        private const string SandboxCrcKey = "yourSandboxCrcKey";
        private const string SandboxReportKey = "yourSandboxReportKey";

        public Przelewy24Service(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IPrzelewy24SignatureProvider signatureProvider,
            ILogger<Przelewy24Service> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _signatureProvider = signatureProvider;
            _logger = logger;
        }

        public async Task<RegisterResponseDto> CreateTransactionAsync(RegisterRequestDto request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Use configured CRC key name that matches appsettings.Development.json
            var crc = _configuration["Przelewy24:CrcKey"] ?? SandboxCrcKey;
            if (string.IsNullOrWhiteSpace(crc))
                throw new InvalidOperationException("Przelewy24 CRC key not configured.");

            // Ensure sessionId
            if (string.IsNullOrWhiteSpace(request.SessionId))
            {
                request = request with { SessionId = Guid.NewGuid().ToString("N") };
            }

            // Compute sign
            var sign = await _signatureProvider.CreateRegisterSignatureAsync(request, crc).ConfigureAwait(false);
            var signedRequest = request with { Sign = sign };

            // Create named client
            var client = _httpClientFactory.CreateClient("Przelewy24");

            // Set Basic Auth required by Przelewy24 (user = posId, password = report key)
            var reportKey = _configuration["Przelewy24:ReportKey"] ?? SandboxReportKey;
            var posForAuth = request.PosId != 0
                ? request.PosId.ToString()
                : (_configuration["Przelewy24:PosId"] ?? request.MerchantId.ToString());

            var authBytes = Encoding.UTF8.GetBytes($"{posForAuth}:{reportKey}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

            // Przelewy24 expects application/x-www-form-urlencoded for /transaction/register.
            var form = new Dictionary<string, string?>
            {
                ["merchantId"] = signedRequest.MerchantId.ToString(),
                ["posId"] = signedRequest.PosId.ToString(),
                ["amount"] = signedRequest.Amount.ToString(),
                ["currency"] = signedRequest.Currency,
                ["description"] = signedRequest.Description,
                ["email"] = signedRequest.Email,
                ["country"] = signedRequest.Country,
                ["language"] = signedRequest.Language,
                ["urlReturn"] = signedRequest.UrlReturn,
                ["urlStatus"] = signedRequest.UrlStatus,
                ["sessionId"] = signedRequest.SessionId,
                ["sign"] = signedRequest.Sign
            };

            using var content = new FormUrlEncodedContent(form);
            var responseMessage = await client.PostAsync("transaction/register", content).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Przelewy24 returned {(int)responseMessage.StatusCode}. Response content: {responseContent}");
            }

            var dto = JsonSerializer.Deserialize<RegisterResponseDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dto ?? throw new InvalidOperationException("Failed to deserialize Przelewy24 response.");
        }

        public async Task<string> TestAccessAsync(string? posId = null, string? secretId = null)
        {
            // jeśli parametry nie zostały przekazane, pobierz z konfiguracji
            var cfgPosId = posId ?? _configuration["Przelewy24:PosId"] ?? _configuration["Przelewy24:MerchantId"];
            var cfgSecret = secretId ?? _configuration["Przelewy24:ReportKey"];

            if (string.IsNullOrWhiteSpace(cfgPosId))
                throw new InvalidOperationException("Przelewy24:PosId not configured and not provided.");

            if (string.IsNullOrWhiteSpace(cfgSecret))
                throw new InvalidOperationException("Przelewy24:SecretId/ReportKey not configured and not provided.");

            var client = _httpClientFactory.CreateClient("Przelewy24");

            // Basic Auth: posId jako user, secretId jako password
            var authBytes = System.Text.Encoding.UTF8.GetBytes($"{cfgPosId}:{cfgSecret}");
            var authHeader = Convert.ToBase64String(authBytes);

            var request = new HttpRequestMessage(HttpMethod.Get, "testAccess");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

            using var resp = await client.SendAsync(request).ConfigureAwait(false);
            var content = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Przelewy24 returned {(int)resp.StatusCode}. Response content: {content}");
            }

            return content;
        }
    }

    public sealed class Przelewy24LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<Przelewy24LoggingHandler> _logger;

        public Przelewy24LoggingHandler(ILogger<Przelewy24LoggingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending request to Przelewy24: {Method} {Url}", request.Method, request.RequestUri);

            // Log Authorization presence (masked) at Information so it's visible in current logs
            if (request.Headers.Authorization != null)
            {
                var scheme = request.Headers.Authorization.Scheme;
                var param = request.Headers.Authorization.Parameter ?? string.Empty;
                var masked = param.Length > 8 ? param.Substring(0, 8) + "..." : "<redacted>";
                _logger.LogInformation("Request Authorization: {Scheme} {Masked}", scheme, masked);
            }
            else
            {
                _logger.LogInformation("Request Authorization: <missing>");
            }

            // Log other headers at Debug (keeps noise low)
            foreach (var header in request.Headers)
            {
                if (header.Key.Equals("Authorization", System.StringComparison.OrdinalIgnoreCase))
                    continue;
                _logger.LogDebug("Request header: {Key}: {Value}", header.Key, string.Join(",", header.Value));
            }

            // Log request body if present (debugging only)
            if (request.Content != null)
            {
                var body = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(body))
                {
                    _logger.LogDebug("Request body: {Body}", body);

                    // Recreate content so it can be sent downstream after reading
                    var mediaType = request.Content.Headers.ContentType?.MediaType ?? "application/json";
                    request.Content = new StringContent(body, Encoding.UTF8, mediaType);
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogInformation("Received response from Przelewy24: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogWarning("Przelewy24 error response: {Content}", content);
            }

            return response;
        }
    }
}