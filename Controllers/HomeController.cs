using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using ApiClientPrzelewy24.Clients;
using ApiClientPrzelewy24.Services;
using ApiClientPrzelewy24.Models;

namespace ApiClientPrzelewy24.Controllers
{
    [Route("ApiClientPrzelewy24")]
    public class HomeController : Controller
    {
        private readonly Przelewy24Service _przelewy24Service;
        private readonly IConfiguration _configuration;

        public HomeController(Przelewy24Service przelewy24Service, IConfiguration configuration)
        {
            _przelewy24Service = przelewy24Service;
            _configuration = configuration;
        }

        [HttpGet("TestAccess")]
        public async Task<IActionResult> TestAccess()
        {
            try
            {
                var content = await _przelewy24Service.TestAccessAsync();
                return Content(content, "application/json; charset=utf-8");
            }
            catch (Exception ex)
            {
                return Content($"TestAccess failed: {ex.Message}", "text/plain; charset=utf-8");
            }
        }

        /// <summary>
        /// Rejestruje transakcję w Przelewy24.
        /// POST /ApiClient/Register
        /// Body: RegisterRequestDto (JSON)
        /// </summary>
        //[HttpPost("Register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        [HttpGet("Register")]
        public async Task<IActionResult> Register()
        {
            // Read values from configuration (preferred) for sandbox/manual testing.
            // Expecting numeric MerchantId/PosId and CRC in configuration under "Przelewy24:*".
            var cfgMerchant = _configuration["Przelewy24:MerchantId"];
            var cfgPos = _configuration["Przelewy24:PosId"];
            var cfgCrc = _configuration["Przelewy24:CrcKey"];

            if (!int.TryParse(cfgMerchant, out var merchantId))
            {
                // fallback to PosId if MerchantId not set
                if (!int.TryParse(cfgPos, out merchantId))
                {
                    return Problem(detail: "Przelewy24:MerchantId or Przelewy24:PosId must be configured as an integer.", statusCode: 500);
                }
            }

            if (!int.TryParse(cfgPos, out var posId))
            {
                // if PosId not configured explicitly, reuse merchantId
                posId = merchantId;
            }

            if (string.IsNullOrWhiteSpace(cfgCrc))
            {
                return Problem(detail: "Przelewy24:Crc must be configured. Provide CRC key from your Przelewy24 sandbox panel.", statusCode: 500);
            }

            // Build test request using configured numeric IDs and CRC (CRC will be used by service/signature provider)
            var request = new RegisterRequestDto(
                MerchantId: merchantId,
                PosId: posId,
                Amount: 1000, // 10.00 PLN in grosze
                Currency: "PLN",
                Description: "integration-test",
                Email: "integration@example.com",
                Country: "PL",
                Language: "pl",
                UrlReturn: "https://example.com/return",
                UrlStatus: "https://example.com/status",
                SessionId: string.Empty,
                Sign: string.Empty);

            if (request is null)
                return BadRequest("Request body is required.");

            if (request.MerchantId == 0)
                return BadRequest("merchantId is required.");

            if (request.PosId == 0)
                return BadRequest("posId is required.");

            if (request.Amount <= 0)
                return BadRequest("amount must be greater than 0.");

            if (string.IsNullOrWhiteSpace(request.UrlReturn))
                return BadRequest("urlReturn is required.");

            try
            {
                // Ensure the service reads CRC from configuration, but pass-through here if needed.
                var response = await _przelewy24Service.CreateTransactionAsync(request).ConfigureAwait(false);

                // Basic handling: return full DTO from Przelewy24
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Don't leak secrets; return message for debugging
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }
}
