using System;
using System.Security.Cryptography;
using System.Text;

namespace ApiClientPrzelewy24.Clients
{
    public static class Przelewy24SignatureHelper
    {
        // Compute sha384 hex (lowercase) for Przelewy24 "register" sign using exact canonical JSON
        public static string ComputeRegisterSign(string sessionId, int merchantId, long amount, string currency, string crc)
        {
            // Canonical JSON: exact order, no spaces — matches calculator input
            var payload = $"{{\"sessionId\":\"{sessionId}\",\"merchantId\":{merchantId},\"amount\":{amount},\"currency\":\"{currency}\",\"crc\":\"{crc}\"}}";
            var bytes = Encoding.UTF8.GetBytes(payload);
            var hash = SHA384.HashData(bytes);
            var hex = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return hex;
        }
    }
}