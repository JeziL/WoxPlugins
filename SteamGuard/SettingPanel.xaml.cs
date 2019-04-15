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
