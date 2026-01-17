using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using ApiClientPrzelewy24.Clients;
using ApiClientPrzelewy24.Services;
using ApiClientPrzelewy24.Objects;

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

        #region Test & Basic Operations

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

        #endregion

        #region Transaction Operations

        /// <summary>
        /// Rejestruje transakcję w Przelewy24.
        /// GET /ApiClientPrzelewy24/Register - dla testów
        /// POST /ApiClientPrzelewy24/Register - dla produkcji
        /// </summary>
        [HttpGet("Register")]
        public async Task<IActionResult> Register()
        {
            var cfgMerchant = _configuration["Przelewy24:MerchantId"];
            var cfgPos = _configuration["Przelewy24:PosId"];
            var cfgCrc = _configuration["Przelewy24:CrcKey"];

            if (!int.TryParse(cfgMerchant, out var merchantId))
            {
                if (!int.TryParse(cfgPos, out merchantId))
                {
                    return Problem(detail: "Przelewy24:MerchantId or Przelewy24:PosId must be configured as an integer.", statusCode: 500);
                }
            }

            if (!int.TryParse(cfgPos, out var posId))
            {
                posId = merchantId;
            }

            if (string.IsNullOrWhiteSpace(cfgCrc))
            {
                return Problem(detail: "Przelewy24:Crc must be configured. Provide CRC key from your Przelewy24 sandbox panel.", statusCode: 500);
            }

            var request = new RegisterRequestDto(
                MerchantId: merchantId,
                PosId: posId,
                Amount: 1000,
                Currency: "PLN",
                Description: "integration-test",
                Email: "integration@example.com",
                Country: "PL",
                Language: "pl",
                UrlReturn: "https://example.com/return",
                UrlStatus: "https://example.com/status",
                SessionId: string.Empty,
                Sign: string.Empty);

            if (request.Amount <= 0)
                return BadRequest("amount must be greater than 0.");

            if (string.IsNullOrWhiteSpace(request.UrlReturn))
                return BadRequest("urlReturn is required.");

            try
            {
                var response = await _przelewy24Service.CreateTransactionAsync(request).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Rejestruje transakcję przez POST (produkcyjne użycie)
        /// POST /ApiClientPrzelewy24/Register
        /// Body: RegisterRequestDto (JSON)
        /// </summary>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPost([FromBody] RegisterRequestDto request)
        {
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
                var response = await _przelewy24Service.CreateTransactionAsync(request).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Weryfikuje transakcję po otrzymaniu notyfikacji z Przelewy24.
        /// PUT /ApiClientPrzelewy24/Verify
        /// Body: VerifyRequestDto (JSON)
        /// </summary>
        [HttpPut("Verify")]
        public async Task<IActionResult> VerifyTransaction([FromBody] VerifyRequestDto request)
        {
            if (request is null)
                return BadRequest("Request body is required.");

            try
            {
                var response = await _przelewy24Service.VerifyTransactionAsync(request).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Pobiera informacje o transakcji na podstawie sessionId.
        /// GET /ApiClientPrzelewy24/Transaction/{sessionId}
        /// </summary>
        [HttpGet("Transaction/{sessionId}")]
        public async Task<IActionResult> GetTransactionBySessionId(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return BadRequest("SessionId is required.");

            try
            {
                var response = await _przelewy24Service.GetTransactionBySessionIdAsync(sessionId).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        #endregion

        #region Refund Operations

        /// <summary>
        /// Wykonuje zwrot środków (pełny lub częściowy).
        /// POST /ApiClientPrzelewy24/Refund
        /// Body: RefundRequestDto (JSON)
        /// </summary>
        [HttpPost("Refund")]
        public async Task<IActionResult> RefundTransaction([FromBody] RefundRequestDto request)
        {
            if (request is null)
                return BadRequest("Request body is required.");

            try
            {
                var response = await _przelewy24Service.RefundTransactionAsync(request).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Pobiera dane o zwrocie na podstawie orderId.
        /// GET /ApiClientPrzelewy24/Refund/{orderId}
        /// </summary>
        [HttpGet("Refund/{orderId}")]
        public async Task<IActionResult> GetRefundByOrderId(long orderId)
        {
            try
            {
                var response = await _przelewy24Service.GetRefundByOrderIdAsync(orderId).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        #endregion

        #region Payment Methods

        /// <summary>
        /// Pobiera listę dostępnych metod płatności.
        /// GET /ApiClientPrzelewy24/PaymentMethods?lang=pl&amount=1000&currency=PLN
        /// </summary>
        [HttpGet("PaymentMethods")]
        public async Task<IActionResult> GetPaymentMethods([FromQuery] string lang = "pl", [FromQuery] int? amount = null, [FromQuery] string currency = "PLN")
        {
            try
            {
                var response = await _przelewy24Service.GetPaymentMethodsAsync(lang, amount, currency).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        #endregion

        #region Card Operations

        /// <summary>
        /// Pobiera informacje o karcie (refId, BIN, mask).
        /// GET /ApiClientPrzelewy24/Card/{orderId}
        /// </summary>
        [HttpGet("Card/{orderId}")]
        public async Task<IActionResult> GetCardInfo(long orderId)
        {
            try
            {
                var response = await _przelewy24Service.GetCardInfoAsync(orderId).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Obciąża kartę z 3DS (1-click payment).
        /// POST /ApiClientPrzelewy24/Card/ChargeWith3ds
        /// Body: { "token": "xxx" }
        /// </summary>
        [HttpPost("Card/ChargeWith3ds")]
        public async Task<IActionResult> ChargeCardWith3ds([FromBody] TokenRequest request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Token))
                return BadRequest("Token is required.");

            try
            {
                var response = await _przelewy24Service.ChargeCardWith3dsAsync(request.Token).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Obciąża kartę rekurencyjnie (bez 3DS).
        /// POST /ApiClientPrzelewy24/Card/Charge
        /// Body: { "token": "xxx" }
        /// </summary>
        [HttpPost("Card/Charge")]
        public async Task<IActionResult> ChargeCard([FromBody] TokenRequest request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Token))
                return BadRequest("Token is required.");

            try
            {
                var response = await _przelewy24Service.ChargeCardAsync(request.Token).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Płatność kartą z bezpośrednimi danymi (wymaga PCI DSS).
        /// POST /ApiClientPrzelewy24/Card/Pay
        /// Body: CardPayRequestDto (JSON)
        /// </summary>
        [HttpPost("Card/Pay")]
        public async Task<IActionResult> PayWithCard([FromBody] CardPayRequestDto request)
        {
            if (request is null)
                return BadRequest("Request body is required.");

            try
            {
                var response = await _przelewy24Service.PayWithCardAsync(request).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        #endregion

        #region BLIK Operations

        /// <summary>
        /// Płatność kodem BLIK (6-cyfrowy kod).
        /// POST /ApiClientPrzelewy24/Blik/ChargeByCode
        /// Body: BlikChargeByCodeRequestDto (JSON)
        /// </summary>
        [HttpPost("Blik/ChargeByCode")]
        public async Task<IActionResult> ChargeByBlikCode([FromBody] BlikChargeByCodeRequestDto request)
        {
            if (request is null)
                return BadRequest("Request body is required.");

            try
            {
                var response = await _przelewy24Service.ChargeByBlikCodeAsync(request).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Płatność aliasem BLIK (1-click).
        /// POST /ApiClientPrzelewy24/Blik/ChargeByAlias
        /// Body: BlikChargeByAliasRequestDto (JSON)
        /// </summary>
        [HttpPost("Blik/ChargeByAlias")]
        public async Task<IActionResult> ChargeByBlikAlias([FromBody] BlikChargeByAliasRequestDto request)
        {
            if (request is null)
                return BadRequest("Request body is required.");

            try
            {
                var response = await _przelewy24Service.ChargeByBlikAliasAsync(request).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Pobiera listę aliasów BLIK dla adresu email.
        /// GET /ApiClientPrzelewy24/Blik/Aliases/{email}
        /// </summary>
        [HttpGet("Blik/Aliases/{email}")]
        public async Task<IActionResult> GetBlikAliasesByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            try
            {
                var response = await _przelewy24Service.GetBlikAliasesByEmailAsync(email).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Pobiera listę custom aliasów BLIK dla adresu email.
        /// GET /ApiClientPrzelewy24/Blik/CustomAliases/{email}
        /// </summary>
        [HttpGet("Blik/CustomAliases/{email}")]
        public async Task<IActionResult> GetBlikCustomAliasesByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            try
            {
                var response = await _przelewy24Service.GetBlikCustomAliasesByEmailAsync(email).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        #endregion

        #region Report Operations

        /// <summary>
        /// Pobiera historię transakcji w zadanym okresie.
        /// GET /ApiClientPrzelewy24/Report/History?dateFrom=20250101&dateTo=20250131&type=transaction
        /// </summary>
        [HttpGet("Report/History")]
        public async Task<IActionResult> GetReportHistory([FromQuery] string dateFrom, [FromQuery] string dateTo, [FromQuery] string? type = null)
        {
            if (string.IsNullOrWhiteSpace(dateFrom))
                return BadRequest("DateFrom is required (format: YYYYMMDD).");

            if (string.IsNullOrWhiteSpace(dateTo))
                return BadRequest("DateTo is required (format: YYYYMMDD).");

            try
            {
                var response = await _przelewy24Service.GetReportHistoryAsync(dateFrom, dateTo, type).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        /// <summary>
        /// Pobiera szczegóły paczki (batch).
        /// GET /ApiClientPrzelewy24/Report/Batch/{batchId}
        /// </summary>
        [HttpGet("Report/Batch/{batchId}")]
        public async Task<IActionResult> GetBatchDetails(int batchId)
        {
            try
            {
                var response = await _przelewy24Service.GetBatchDetailsAsync(batchId).ConfigureAwait(false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        #endregion
    }

    /// <summary>
    /// Helper record dla endpointów wymagających tylko tokenu
    /// </summary>
    public record TokenRequest(string Token);
}
