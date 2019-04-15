using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
