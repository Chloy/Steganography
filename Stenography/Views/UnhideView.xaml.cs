using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Stenography.Views {
    /// <summary>
    /// Interaction logic for UnhideView.xaml
    /// </summary>
    public partial class UnhideView : UserControl {
        private string extention;
        public UnhideView() {
            InitializeComponent();
        }
        private void InputButton_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = Properties.Lang.FDUnhideContainer;
            openFile.Filter = "Text or picture (*.txt; *.png)|*.txt; *.png";
            if (openFile.ShowDialog() == true) {
                Input.Text = openFile.FileName;
                int index = openFile.FileName.LastIndexOf('\\');
                if(string.IsNullOrEmpty(OutputPath.Text))
                    OutputPath.Text = openFile.FileName.Substring(0, index);
                if(string.IsNullOrEmpty(OutputName.Text))
                    OutputName.Text = Properties.Lang.Unhiden + openFile.FileName.Substring(++index, openFile.FileName.LastIndexOf('.') - openFile.FileName.LastIndexOf('\\') - 1);
            }
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e) {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = Properties.Lang.FDOutputDir;
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = OutputPath.Text;

            dlg.AddToMostRecentlyUsedList = true;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = OutputPath.Text;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok) {
                var folder = dlg.FileName;
                OutputPath.Text = folder;
            }
        }

        private void Unhide_Click(object sender, RoutedEventArgs e) {
            string ext;
            switch (System.IO.Path.GetExtension(Input.Text)) {
                case ".txt":
                    string sourseStream;
                    using (StreamReader reader = new StreamReader(Input.Text)) {
                        sourseStream = reader.ReadToEnd();
                    }
                    byte[] information = TextFile.Unhide(sourseStream, Functions.CharacterCount(sourseStream, TextFile.equalSymbols_RU_EN));
                    ext = Functions.ExtractExtension(ref information);
                    extention = string.Copy(ext);
                    using (FileStream file = new FileStream(OutputPath.Text + '\\' + OutputName.Text + ext, FileMode.Create)) {
                        file.Write(information, 0, information.Length);
                    }
                    break;
                case ".png":
                    Bitmap image = new Bitmap(Input.Text);
                    byte[] result = PictureFile.Unhide(image);
                    ext = Functions.ExtractExtension(ref result);
                    extention = string.Copy(ext);
                    using (FileStream file = new FileStream(OutputPath.Text + '\\' + OutputName.Text + ext, FileMode.Create)) {
                        file.Write(result, 0, result.Length);
                    }
                    break;
            }
        }

        private void OpenOutDir_Click(object sender, RoutedEventArgs e) {
            string absolutePath = $"{OutputPath.Text}\\{OutputName.Text}{extention}";
            if (!File.Exists(absolutePath)) return;
            string args = string.Format("/Select, \"{0}\"", absolutePath);
            Process.Start(new ProcessStartInfo("explorer.exe", args));
        }
    }
}