using System.ComponentModel.DataAnnotations;

namespace OrchardCore.PaymentGateway.Providers.Przelewy24.ViewModels;

public class Przelewy24SettingsViewModel
{
    [Display(Name = "Client / Merchant ID")]
    public string? ClientId { get; set; }

    [Display(Name = "Merchant ID (int)")]
    public int? MerchantId { get; set; }

    [Display(Name = "POS ID (int)")]
    public int? PosId { get; set; }

    [Display(Name = "CRC key"), DataType(DataType.Password)]
    public string? CrcKey { get; set; }

    [Display(Name = "Report key"), DataType(DataType.Password)]
    public string? ReportKey { get; set; }

    [Display(Name = "Secret ID (optional)"), DataType(DataType.Password)]
    public string? SecretId { get; set; }

    [Display(Name = "API base URL")]
    public string? BaseUrl { get; set; }

    [Display(Name = "Allow sandbox fallbacks when empty")]
    public bool UseSandboxFallbacks { get; set; } = true;
}
