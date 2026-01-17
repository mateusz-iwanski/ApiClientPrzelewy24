namespace ApiClientPrzelewy24.Objects
{
    /// <summary>
    /// ¯¹danie zwrotu œrodków w Przelewy24
    /// </summary>
    public record RefundRequestDto(
        int OrderId,
        string SessionId,
        int Amount,
        string Currency
    )
    {
        /// <summary>
        /// Opcjonalny UUID zwrotu
        /// </summary>
        public string? RefundsUuid { get; init; }
    }
}