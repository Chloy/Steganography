using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;

namespace Stenography.Views {
    /// <summary>
    /// Interaction logic for VideoView.xaml
    /// </summary>
    public partial class VideoView : UserControl {
        byte[] informationForHide;
        public VideoView() {
            InitializeComponent();
        }

        private void engButt_Click(object sender, RoutedEventArgs e) {
            SelectLang("EN-US");
        }

        private void rusButt_Click(object sender, RoutedEventArgs e) {
            SelectLang("RU-RU");
        }

        private void SelectLang(string lng) {
            if(lng == "EN-US") {
                Properties.Settings.Default.language = "EN-US";
            }
            else if(lng == "RU-RU") {
                Properties.Settings.Default.language = "RU-RU";
            }
            Properties.Settings.Default.Save();
        }
    }
}
