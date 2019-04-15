using Wox.Plugin;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Globalization;
using System.Windows.Controls;
using System.Collections.Generic;
using Wox.Infrastructure.Storage;

namespace WoxPlugins.CurrencyConverter {
    public class CurrencyConverter : IPlugin, ISettingProvider, ISavable {
        private PluginInitContext _context;
        private readonly Settings _settings;
        private readonly PluginJsonStorage<Settings> _storage;
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

        public CurrencyConverter() {
            _storage = new PluginJsonStorage<Settings>();
            _settings = _storage.Load();
        }

        public void Save() {
            _storage.Save();
        }

        public void Init(PluginInitContext context) {
            _context = context;
        }

        private bool IsValid(Query query) {
            if (query.Terms.Length != 2) return false;
            if (!_CURRENCIES.Contains(query.FirstSearch.ToUpper())) return false;
            return float.TryParse(query.SecondSearch, out float val);
        }

        public List<Result> Query(Query query) {
            var results = new List<Result>();
            if (!IsValid(query)) return results;
            try {
                if (!_settings.IsValid()) {
                    results.Add(new Result() {
                        Title = "API Key 非法",
                        SubTitle = "打开设置",
                        IcoPath = "img\\cny.ico",
                        Action = _ => {
                            _context.API.OpenSettingDialog();
                            return true;
                        }
                    });
                    return results;
                }
            }
            catch {
                results.Add(new Result() {
                    Title = "API Key 未设置",
                    SubTitle = "打开设置",
                    IcoPath = "img\\cny.ico",
                    Action = _ => {
                        _context.API.OpenSettingDialog();
                        return true;
                    }
                });
                return results;
            }
            string currency = query.FirstSearch.ToUpper();
            float value = float.Parse(query.SecondSearch, CultureInfo.InvariantCulture.NumberFormat);
            string url = string.Format("https://free.currencyconverterapi.com/api/v6/convert?q={0}_CNY&compact=ultra&apiKey={1}", currency, _settings.APIKey);
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

        public Control CreateSettingPanel() {
            return new SettingPanel(_settings);
        }
    }
}
