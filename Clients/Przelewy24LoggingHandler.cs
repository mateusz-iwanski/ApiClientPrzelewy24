using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Linq;

namespace ApiClientPrzelewy24.Clients
{
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

            // Log headers (excluding Authorization) for debugging
            foreach (var header in request.Headers)
            {
                if (header.Key.Equals("Authorization", System.StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogDebug("Request header: {Key}: <redacted>", header.Key);
                }
                else
                {
                    _logger.LogDebug("Request header: {Key}: {Value}", header.Key, string.Join(",", header.Value));
                }
            }

            // Log request body if present (debugging only)
            if (request.Content != null)
            {
                var body = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(body))
                {
                    // WARNING: avoid logging secrets in production. This is for debugging only.
                    _logger.LogDebug("Request body: {Body}", body);

                    // Recreate content so it can be sent downstream after reading
                    var mediaType = request.Content.Headers.ContentType?.MediaType ?? "application/json";
                    request.Content = new StringContent(body, Encoding.UTF8, mediaType);

                    // preserve other content headers if any (except content-type which we set)
                    foreach (var h in request.Content.Headers.ToList())
                    {
                        // already present
                    }
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogInformation("Received response from Przelewy24: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Przelewy24 error response: {Content}", content);
            }

            return response;
        }
    }
}
