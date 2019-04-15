using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;

namespace WoxPlugins.CurrencyConverter {
    public partial class SettingPanel : UserControl {
        private readonly Settings _settings;
        public SettingPanel(Settings settings) {
            InitializeComponent();
            _settings = settings;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            APIKeyField.Text = _settings.APIKey;

            APIKeyField.TextChanged += (o, ev) => {
                _settings.APIKey = APIKeyField.Text;
            };
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e) {
            Process.Start("https://free.currencyconverterapi.com/free-api-key");
        }
    }
}
