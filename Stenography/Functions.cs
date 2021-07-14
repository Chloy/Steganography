using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Controls;

namespace Stenography {
    class Functions {
        static string[] sizeB = { "B", "KB", "MB", "GB" };
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<int, string> errorsKind = new Dictionary<int, string> {
            {1, Properties.Lang.ContainerPathWrong},
            {2, Properties.Lang.SecretFilePathWrong},
            {3, Properties.Lang.OutputPathWrong },
            {4, Properties.Lang.SecretFileTooBig}
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string WriteLog(string str) {
            return $"{DateTime.Now.Hour}:{DateTime.Now.Minute:D2}:{DateTime.Now.Second:D2} {str}\n";
        }

        public static void WriteLog(TextBox logField, string str) {
             logField.Text += $"{DateTime.Now.Hour}:{DateTime.Now.Minute:D2}:{DateTime.Now.Second:D2} {str}\n";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="equalSymbols"></param>
        /// <param name="numberOfBytes"></param>
        /// <returns></returns>
        public static string CountAbleSizeToHide(string str, Dictionary<char, char> equalSymbols, out int numberOfBytes) {
            float howManyBytes = CharacterCount(str, equalSymbols) / 8;
            numberOfBytes = (int)howManyBytes;
            int size = 0;
            while (howManyBytes > 1000) {
                howManyBytes /= 1000;
                size++;
            }
            return $"{Properties.Lang.AbleSizeForHide} {howManyBytes} {sizeB[size]}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="numberOfBytes"></param>
        /// <returns></returns>
        public static string CountAbleSizeToHide(Bitmap image, out int numberOfBytes) {
            float howManyBytes = image.Width * image.Height * 3 / 8;
            numberOfBytes = (int)howManyBytes;
            int size = 0;
            while (howManyBytes > 1000) {
                howManyBytes /= 1000;
                size++;
            }
            return $"{Properties.Lang.AbleSizeForHide} {howManyBytes} {sizeB[size]}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="equalSymbols"></param>
        /// <returns></returns>
        public static int CharacterCount(string str, Dictionary<char, char> equalSymbols) {
            int count = 0;
            for (int i = 0; i < str.Length; i++) {
                if (equalSymbols.ContainsKey(str[i]) || equalSymbols.ContainsValue(str[i])) count++;
            }
            return count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="equalSymbols"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static int IndexOfAnySymbol(string str, Dictionary<char, char> equalSymbols, int startIndex = 0) {
            for (int i = startIndex; i < str.Length; i++) {
                if (equalSymbols.ContainsKey(str[i]) || equalSymbols.ContainsValue(str[i])) {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int LastZeroIndex(byte[] array) {
            int index = array.Length - 1;
            while (index != -1 && array[index] == 0) {
                index--;
            }
            return ++index;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapImage BitmapToImageSource(Bitmap bitmap) {
            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                System.Windows.Media.Imaging.BitmapImage bitmapimage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static byte[] AddExtension(byte[] byteStream, string extension) {
            byte[] ext = Encoding.ASCII.GetBytes(extension);
            byte[] res = new byte[byteStream.Length + ext.Length];
            Array.Copy(byteStream, res, byteStream.Length);
            Array.Copy(ext, 0, res, byteStream.Length, ext.Length);
            return res;
        }

        public static string ExtractExtension(ref byte[] byteStream) {
            string ext = Encoding.ASCII.GetString(byteStream);
            int ind = ext.LastIndexOf('.');
            Array.Resize(ref byteStream, ind);
            ext = ext.Substring(ind, ext.Length - ind);
            return ext;
        }
    }
}
