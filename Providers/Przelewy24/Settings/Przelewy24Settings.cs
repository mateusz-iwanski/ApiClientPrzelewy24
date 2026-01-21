using System.Collections.Generic;

namespace OrchardCore.PaymentGateway.Providers.Przelewy24.Settings;

public class Przelewy24AccountSettings
{
    public string? Key { get; set; }          // np. "default", "shopA", "shopB"
    public int? MerchantId { get; set; }
    public int? PosId { get; set; }
    public string? CrcKey { get; set; }
    public string? ReportKey { get; set; }
    public string? SecretId { get; set; }
    public string? BaseUrl { get; set; }
    public bool UseSandboxFallbacks { get; set; } = true;
}

public class Przelewy24Settings
{
    public string? DefaultAccountKey { get; set; } = "default";
    public List<Przelewy24AccountSettings> Accounts { get; set; } = new();
}
