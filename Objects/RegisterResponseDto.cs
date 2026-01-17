using System.Text.Json.Serialization;

namespace ApiClientPrzelewy24.Objects
{
    public sealed record RegisterResponseDto(
        [property: JsonPropertyName("data")] RegisterResponseData? Data,
        [property: JsonPropertyName("responseCode")] int ResponseCode
    )
    {
        // Convenience accessor so existing callers that expect a top-level Token keep working.
        [JsonIgnore]
        public string? Token => Data?.Token;
    }

    public sealed record RegisterResponseData(
        [property: JsonPropertyName("token")] string? Token
    );
}
