using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;

namespace WoxPlugins.SteamGuard {
    public partial class SettingPanel : UserControl {
        private readonly Settings _settings;

        public SettingPanel(Settings settings) {
            InitializeComponent();
            _settings = settings;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            SharedSecretField.Text = _settings.SharedSecret;

            SharedSecretField.TextChanged += (o, ev) => {
                _settings.SharedSecret = SharedSecretField.Text;
            };
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e) {
            Process.Start("https://github.com/SteamTimeIdler/stidler/wiki/Getting-your-%27shared_secret%27-code-for-use-with-Auto-Restarter-on-Mobile-Authentication");
        }
    }
}
