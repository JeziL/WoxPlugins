using System.Text.RegularExpressions;

namespace WoxPlugins.SteamGuard {
    public class Settings {
        public string SharedSecret { get; set; }
        public bool isValid() {
            if (this.SharedSecret == null) return false;
            return Regex.Match(this.SharedSecret, @"^[0-9a-zA-Z\+\/=]+$").Success;
        }
    }
}
