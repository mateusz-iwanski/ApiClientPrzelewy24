using ApiClientPrzelewy24.Objects;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiClientPrzelewy24.Clients
{
    /// <summary>
    /// Default implementation of <see cref="IPrzelewy24SignatureProvider"/>.
    /// </summary>
    /// <remarks>
    /// Purpose
    /// - Generates the "sign" value required by Przelewy24 for the
    ///   /transaction/register request. The sign is a SHA‑384 hash of a JSON
    ///   object with fields in a strict order:
    ///   {"sessionId":"str","merchantId":int,"amount":int,"currency":"str","crc":"str"}.
    /// </remarks>
    public sealed class Przelewy24SignatureProvider_Default : IPrzelewy24SignatureProvider
    {
        /// <summary>
        /// Creates the Przelewy24 "sign" value for transaction registration.
        /// </summary>
        public Task<string> CreateRegisterSignatureAsync(RegisterRequestDto request, string crcKey)
        {
            // Delegate to the shared helper to ensure a single canonical implementation is used.
            var sign = Przelewy24SignatureHelper.ComputeRegisterSign(
                request.SessionId,
                request.MerchantId,
                request.Amount,
                request.Currency,
                crcKey);

            return Task.FromResult(sign);
        }

        // Legacy/local method kept for reference (not used anymore).
        private static byte[] BuildOrderedJson(string sessionId, int merchantId, long amount, string currency, string crc)
        {
            using var ms = new MemoryStream();
            var options = new JsonWriterOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Indented = false
            };

            using (var writer = new Utf8JsonWriter(ms, options))
            {
                writer.WriteStartObject();
                writer.WriteString("sessionId", sessionId ?? string.Empty);
                writer.WriteNumber("merchantId", merchantId);
                writer.WriteNumber("amount", amount);
                writer.WriteString("currency", currency ?? string.Empty);
                writer.WriteString("crc", crc ?? string.Empty);
                writer.WriteEndObject();
                writer.Flush();
            }

            return ms.ToArray();
        }
    }
}
