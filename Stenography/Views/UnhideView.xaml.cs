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
using System.Drawing;

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
            switch (System.IO.Path.GetExtension(Input.Text)) {
                case ".txt":
                    string sourseStream;
                    using (StreamReader reader = new StreamReader(Input.Text)) {
                        sourseStream = reader.ReadToEnd();
                    }

                    byte[] information = TextFile.Unhide(sourseStream, Functions.CharacterCount(sourseStream, TextFile.equalSymbols_RU_EN));
                    using (FileStream file = new FileStream(OutputPath.Text + '\\' + OutputName.Text, FileMode.Create)) {
                        file.Write(information, 0, information.Length);
                    }
                    break;
                case ".png":
                    Bitmap image = new Bitmap(Input.Text);
                    byte[] result = PictureFile.Unhide(image);
                    using (FileStream file = new FileStream(OutputPath.Text + '\\' + OutputName.Text, FileMode.Create)) {
                        file.Write(result, 0, result.Length);
                    }
                    break;
            }
        }
    }
}
