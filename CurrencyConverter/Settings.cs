using System.Text.RegularExpressions;

namespace WoxPlugins.CurrencyConverter {
    public class Settings {
        public string APIKey { get; set; }
        public bool isValid() {
            if (this.APIKey == null) return false;
            return Regex.Match(this.APIKey, @"^[0-9a-zA-Z]+$").Success;
        }
    }
}
