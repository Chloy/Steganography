using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Stenography.Views {
    /// <summary>
    /// Interaction logic for TextView.xaml
    /// </summary>
    public partial class TextView : UserControl {
        private string sourseStream;
        private int numberOfBytes;//free space in file for hide another file
        private byte[] informationForHide;
        public TextView() {
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
            openFile.Title = Properties.Lang.FDContainer;
            openFile.Filter = "Text file (*.txt)|*.txt";
            if (openFile.ShowDialog() == true) {
                PathContainer.Text = openFile.FileName;
                int index = openFile.FileName.LastIndexOf('\\');
                if (string.IsNullOrEmpty(PathOutput.Text))
                    PathOutput.Text = openFile.FileName.Substring(0, index);
                if (string.IsNullOrEmpty(NameOutput.Text))
                    NameOutput.Text = Properties.Lang.Hiden + openFile.FileName.Substring(++index, openFile.FileName.LastIndexOf('.') - openFile.FileName.LastIndexOf('\\') - 1);
                using (StreamReader reader = new StreamReader(PathContainer.Text)) {
                    sourseStream = reader.ReadToEnd();
                }
                Logs.Text += Functions.CountAbleSizeToHide(sourseStream, TextFile.equalSymbols_RU_EN, out numberOfBytes) + '\n';
                if (informationForHide != null && informationForHide.Length > numberOfBytes) {
                    Functions.WriteLog(Logs, Properties.Lang.SecretFileTooBig);
                }
            }
        }

        private void ChooseFileForHide_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = Properties.Lang.FDSecret;
            openFile.Filter = "Any file (*.*)|*.*";
            if (openFile.ShowDialog() == true) {
                PathFileForHide.Text = openFile.FileName;
                using (FileStream file = new FileStream(PathFileForHide.Text, FileMode.Open)) {
                    informationForHide = new byte[file.Length];
                    file.Read(informationForHide, 0, (int)file.Length);
                }
                if (sourseStream != null && informationForHide.Length > numberOfBytes) {
                    Functions.WriteLog(Logs, Properties.Lang.SecretFileTooBig);
                }
            }
        }

        private void ChooseOutputPath_Click(object sender, RoutedEventArgs e) {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = Properties.Lang.FDOutputDir;
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = PathOutput.Text;

            dlg.AddToMostRecentlyUsedList = true;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = PathOutput.Text;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok) {
                var folder = dlg.FileName;
                PathOutput.Text = folder;
            }
        }

        private bool CheckArguments() {
            bool result = true;
            if (PathContainer.Text == "") {
                Functions.WriteLog(Logs, Functions.errorsKind[1]);
                result = false;
            }
            if (PathFileForHide.Text == "") {
                Functions.WriteLog(Logs, Functions.errorsKind[2]);
                result = false;
            }
            if (informationForHide != null && informationForHide.Length > numberOfBytes) result = false;
            ScrollLog.ScrollToEnd();
            return result;
        }

        private void Hide_Click(object sender, RoutedEventArgs e) {
            if (!CheckArguments()) return;
            Functions.WriteLog(Logs, Properties.Lang.Start);
            informationForHide = Functions.AddExtension(informationForHide, System.IO.Path.GetExtension(PathFileForHide.Text));
            string res = TextFile.Hide(sourseStream, informationForHide);
            using (StreamWriter writer = new StreamWriter(PathOutput.Text + '\\' + NameOutput.Text + ".txt", false)) {
                for (int i = 0; i < res.Length; i++) {
                    writer.Write(res[i]);
                }
            }
            Functions.WriteLog(Logs, Properties.Lang.Finish);
        }

        private void OpenOutDir_Click(object sender, RoutedEventArgs e) {
            string absolutePath = $"{PathOutput.Text}\\{NameOutput.Text}.txt";
            if (!File.Exists(absolutePath)) return;
            string args = string.Format("/Select, \"{0}\"", absolutePath);
            Process.Start(new ProcessStartInfo("explorer.exe", args));
        }
    }
}
