namespace Accounting
{
    public static class CurrencyFormatter
    {
        private static readonly Dictionary<string, string> CurrencySymbols = new()
        {
            { "USD", "$" },
            { "EUR", "€" },
            { "GBP", "£" },
            { "JPY", "¥" },
            { "CNY", "¥" },
            { "CAD", "C$" },
            { "AUD", "A$" }
        };

        public static string Format(double amount, string currency)
        {
            currency = currency.ToUpperInvariant();

            if (CurrencySymbols.TryGetValue(currency, out string? symbol))
            {
                return $"{symbol!}{amount:N2}";
            }

            return $"{amount:N2} {currency}";
        }
    }
}
