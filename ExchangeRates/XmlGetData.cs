using System.Xml;

namespace ExchangeRates
{
    public class XmlGetData
    {
        static XmlDocument _xDoc = new XmlDocument();
        XmlElement _xRoot;
        List<Currency> currencyList = new List<Currency>();

        string _response, ticker, value, date;

        public XmlGetData(string response)
        {
            _response = response;
        }

        public string GetDate()
        {
            if (_xRoot != null)
            {
                foreach (XmlAttribute attr in _xRoot.Attributes)
                {
                    if (attr.Name == "Date") date = attr.InnerText;
                }
            }
            return date;
        }

        public List<Currency> GetExchangeRate()
        {
            if (_response != null)
            {
                _xDoc.LoadXml(_response);
                _xRoot = _xDoc.DocumentElement;

                if (_xRoot != null)
                {
                    foreach (XmlElement xnode in _xRoot)
                    {
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            if (childnode.Name == "CharCode") ticker = childnode.InnerText;
                            if (childnode.Name == "Value")
                            {
                                value = childnode.InnerText;
                                currencyList.Add(new Currency() { Ticker = ticker, Value = value });
                            }
                        }
                    }
                }
            }
            return currencyList;
        }
    }
}