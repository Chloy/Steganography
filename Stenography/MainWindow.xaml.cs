using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stenography {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void textView_Click(object sender, RoutedEventArgs e) {
            DataContext = new ViewModels.TextViewModel();
        }

        private void pictureView_Click(object sender, RoutedEventArgs e) {
            DataContext = new ViewModels.PictureViewModel();
        }

        private void unhideView_Click(object sender, RoutedEventArgs e) {
            DataContext = new ViewModels.UnhideViewModel();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RollUpWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void videoView_Click(object sender, RoutedEventArgs e) {
            DataContext = new ViewModels.SettingsView();
        }

        private void Settings_Click(object sender, RoutedEventArgs e) {
            DataContext = new ViewModels.SettingsView();
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e) {
            DataContext = new ViewModels.SettingsView();
        }
    }
}
