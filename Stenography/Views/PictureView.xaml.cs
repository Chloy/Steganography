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
using System.IO;
using Microsoft.Win32;

namespace Stenography.Views {
    /// <summary>
    /// Interaction logic for PictueView.xaml
    /// </summary>
    public partial class PictueView : UserControl {
        private System.Drawing.Bitmap image;
        private int numberOfBytes;//free space in file for hide another file
        private byte[] informationForHide;
        public PictueView() {
            InitializeComponent();
        }

        private void NeedEncrypt_Click(object sender, RoutedEventArgs e) {
            if (NeedEncrypt.BorderBrush == Brushes.Green) {
                NeedEncrypt.BorderBrush = Brushes.Gray;
                DoubleBottom.BorderBrush = Brushes.Gray;
                Password.IsEnabled = false;
                PasswordVerify.IsEnabled = false;
                DoubleBottom.IsEnabled = false;
                PassDoubleBottom.IsEnabled = false;
                PassDoubleBottomVerify.IsEnabled = false;
                DoubleBottomPath.IsEnabled = false;
                DoubleBottomChoose.IsEnabled = false;
            }
            else {
                NeedEncrypt.BorderBrush = Brushes.Green;
                Password.IsEnabled = true;
                PasswordVerify.IsEnabled = true;
                DoubleBottom.IsEnabled = true;
            }
        }

        private void DoubleBottomChoose_Click(object sender, RoutedEventArgs e) {

        }

        private void DoubleBottom_Click(object sender, RoutedEventArgs e) {
            if (DoubleBottom.BorderBrush == Brushes.Green) {
                DoubleBottom.BorderBrush = Brushes.Gray;
                PassDoubleBottom.IsEnabled = false;
                PassDoubleBottomVerify.IsEnabled = false;
                DoubleBottomPath.IsEnabled = false;
                DoubleBottomChoose.IsEnabled = false;
            }
            else {
                DoubleBottom.BorderBrush = Brushes.Green;
                PassDoubleBottom.IsEnabled = true;
                PassDoubleBottomVerify.IsEnabled = true;
                DoubleBottomPath.IsEnabled = true;
                DoubleBottomChoose.IsEnabled = true;
            }
        }

        private void ChooseFileContainer_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true) {
                PathContainer.Text = openFile.FileName;
                int index = openFile.FileName.LastIndexOf('\\');
                PathOutput.Text = openFile.FileName.Substring(0, index);
                NameOutput.Text = "Hide" + openFile.FileName.Substring(++index, openFile.FileName.Length - index);
                image = new System.Drawing.Bitmap(PathContainer.Text);
                Logs.Text += Functions.CountAbleSizeToHide(image, out numberOfBytes) + '\n';
            }
        }

        private void ChooseFileForHide_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true) {
                PathFileForHide.Text = openFile.FileName;
                using (FileStream file = new FileStream(PathFileForHide.Text, FileMode.Open)) {
                    informationForHide = new byte[file.Length];
                    file.Read(informationForHide, 0, (int)file.Length);
                }
            }
        }

        private void ChooseOutputPath_Click(object sender, RoutedEventArgs e) {

        }

        private bool CheckArguments() {
            bool result = true;
            if (PathContainer.Text == "") {
                Logs.Text += Functions.WriteLog(Functions.errorsKind[1]);
                result = false;
            }
            if (PathOutput.Text == "") {
                Logs.Text += Functions.WriteLog(Functions.errorsKind[2]);
                result = false;
            }
            if (informationForHide != null && informationForHide.Length > numberOfBytes) result = false;
            ScrollLog.ScrollToEnd();
            return result;
        }

        private void Hide_Click(object sender, RoutedEventArgs e) {
            if (!CheckArguments()) return;
            image = PictureFile.Hide(image, informationForHide);
            image.Save(PathOutput.Text + '\\' + NameOutput.Text, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
