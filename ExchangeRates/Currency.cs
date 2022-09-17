namespace ExchangeRates
{
    public class Currency
    {
        public string? Ticker { get; set; }
        public string? Value { get; set; }

        public override string ToString() => $"{Ticker}  -  {Value} руб.";
    }
}
