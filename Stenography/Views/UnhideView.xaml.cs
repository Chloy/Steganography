using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Diagnostics;

namespace Stenography.Views {
    /// <summary>
    /// Interaction logic for UnhideView.xaml
    /// </summary>
    public partial class UnhideView : UserControl {
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
                OutputPath.Text = openFile.FileName.Substring(0, index);
                OutputName.Text = "Unhide" + openFile.FileName.Substring(++index, openFile.FileName.Length - index);
            }
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e) {

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
                    using (FileStream file = new FileStream(OutputPath.Text + '\\' + "ExtreactedFile" + ext, FileMode.Create)) {
                        file.Write(information, 0, information.Length);
                    }
                    break;
                case ".png":
                    Bitmap image = new Bitmap(Input.Text);
                    byte[] result = PictureFile.Unhide(image);
                    ext = Functions.ExtractExtension(ref result);
                    using (FileStream file = new FileStream(OutputPath.Text + '\\' + "ExtractedFile" + ext, FileMode.Create)) {
                        file.Write(result, 0, result.Length);
                    }
                    break;
            }
        }

        private void OpenOutDir_Click(object sender, RoutedEventArgs e) {
            string absolutePath = $"{OutputPath.Text}\\{OutputName.Text}";
            if (!File.Exists(absolutePath)) return;
            string args = string.Format("/Select, \"{0}\"", absolutePath);
            Process.Start(new ProcessStartInfo("explorer.exe", args));
        }
    }
}