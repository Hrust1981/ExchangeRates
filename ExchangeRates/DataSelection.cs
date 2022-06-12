namespace ExchangeRates
{
    public class DataSelection
    {
        List<Currency> _currencyList;
        public DataSelection(List<Currency> currencyList)
        {
            _currencyList = currencyList;
        }

        public string Run(string ticker)
        {
            string list = null;

            if (ticker != null && _currencyList != null)
            {
                foreach (var item in _currencyList)
                {
                    if (item.ToString().Contains(ticker))
                    {
                        list = item.ToString();
                    }
                }
            }
            return list;
        }
    }
}
