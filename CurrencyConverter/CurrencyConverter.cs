using System.IO;
using Wox.Plugin;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Globalization;
using System.Collections.Generic;

namespace WoxPlugins.CurrencyConverter {
    public class CurrencyConverter : IPlugin {
        private string _apiKey;
        private PluginInitContext _context;
        private string[] _CURRENCIES = {"ALL", "XCD", "EUR", "BBD", "BTN", "BND", "XAF", "CUP", "USD", "FKP", "GIP", "HUF",
                                        "IRR", "JMD", "AUD", "LAK", "LYD", "MKD", "XOF", "NZD", "OMR", "PGK", "RWF", "WST",
                                        "RSD", "SEK", "TZS", "AMD", "BSD", "BAM", "CVE", "CNY", "CRC", "CZK", "ERN", "GEL",
                                        "HTG", "INR", "JOD", "KRW", "LBP", "MWK", "MRO", "MZN", "ANG", "PEN", "QAR", "STD",
                                        "SLL", "SOS", "SDG", "SYP", "AOA", "AWG", "BHD", "BZD", "BWP", "BIF", "KYD", "COP",
                                        "DKK", "GTQ", "HNL", "IDR", "ILS", "KZT", "KWD", "LSL", "MYR", "MUR", "MNT", "MMK",
                                        "NGN", "PAB", "PHP", "RON", "SAR", "SGD", "ZAR", "SRD", "TWD", "TOP", "VEF", "DZD",
                                        "ARS", "AZN", "BYR", "BOB", "BGN", "CAD", "CLP", "CDF", "DOP", "FJD", "GMD", "GYD",
                                        "ISK", "IQD", "JPY", "KPW", "LVL", "CHF", "MGA", "MDL", "MAD", "NPR", "NIO", "PKR",
                                        "PYG", "SHP", "SCR", "SBD", "LKR", "THB", "TRY", "AED", "VUV", "YER", "AFN", "BDT",
                                        "BRL", "KHR", "KMF", "HRK", "DJF", "EGP", "ETB", "XPF", "GHS", "GNF", "HKD", "XDR",
                                        "KES", "KGS", "LRD", "MOP", "MVR", "MXN", "NAD", "NOK", "PLN", "RUB", "SZL", "TJS",
                                        "TTD", "UGX", "UYU", "VND", "TND", "UAH", "UZS", "TMT", "GBP", "ZMW", "BTC", "BYN"};
        private static readonly HttpClient _client = new HttpClient();
        public void Init(PluginInitContext context) {
            string apikey_path = Path.Combine(context.CurrentPluginMetadata.PluginDirectory, "APIKEY");
            _apiKey = File.ReadAllText(apikey_path).Trim();
            _context = context;
        }
        private bool isValid(Query query) {
            if (query.Terms.Length != 2) return false;
            if (!_CURRENCIES.Contains(query.FirstSearch.ToUpper())) return false;
            return float.TryParse(query.SecondSearch, out float val);
        }
        public List<Result> Query(Query query) {
            var results = new List<Result>();
            if (!isValid(query)) return results;
            string currency = query.FirstSearch.ToUpper();
            float value = float.Parse(query.SecondSearch, CultureInfo.InvariantCulture.NumberFormat);
            string url = string.Format("https://free.currencyconverterapi.com/api/v6/convert?q={0}_CNY&compact=ultra&apiKey={1}", currency, _apiKey);
            try {
                string responseBody = _client.GetStringAsync(url).Result;
                var resp = JsonConvert.DeserializeObject<Dictionary<string, float>>(responseBody);
                float cc_value = resp.Values.First();
                float result_value = cc_value * value;
                string result = string.Format("{0:f2}", result_value);
                results.Add(new Result() {
                    Title = result,
                    SubTitle = string.Format("{0:f2} {1} = {2} CNY", value, currency, result),
                    IcoPath = "img\\cny.ico"
                });
                return results;
            }
            catch {
                return results;
            }
        }
    }
}
