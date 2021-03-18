using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace Stenography {
    class Functions {
        delegate void CountPossibleSizeToHide();
        event CountPossibleSizeToHide CallCount;
        static string[] sizeB = { "B", "KB", "MB", "GB" };
        public static Dictionary<int, string> errorsKind = new Dictionary<int, string> {
            {1, "File-container's path is wrong." },
            {2, "Secret-file's path is wrong." },
            {3, "Output-file's path is wrong." },
        };

        public static string WriteLog(string str) {
            return $"{DateTime.Now.Hour}:{DateTime.Now.Minute:D2}:{DateTime.Now.Second:D2} {str}\n";
        }

        public static string CountAbleSizeToHide(string str, Dictionary<char, char> equalSymbols, out int numberOfBytes) {
            float howManyBytes = CharacterCount(str, equalSymbols) / 8;
            numberOfBytes = (int)howManyBytes;
            int size = 0;
            while (howManyBytes > 1000) {
                howManyBytes /= 1000;
                size++;
            }
            return $"Able size for hide {howManyBytes} {sizeB[size]}";
        }

        public static string CountAbleSizeToHide(Bitmap image, out int numberOfBytes) {
            float howManyBytes = image.Width * image.Height * 3 / 8;
            numberOfBytes = (int)howManyBytes;
            int size = 0;
            while (howManyBytes > 1000) {
                howManyBytes /= 1000;
                size++;
            }
            return $"Able size for hide {howManyBytes} {sizeB[size]}";
        }

        public static int CharacterCount(string str, Dictionary<char, char> equalSymbols) {
            int count = 0;
            for (int i = 0; i < str.Length; i++) {
                if (equalSymbols.ContainsKey(str[i]) || equalSymbols.ContainsValue(str[i])) count++;
            }
            return count;
        }

        public static int IndexOfAnySymbol(string str, Dictionary<char, char> equalSymbols, int startIndex = 0) {
            for (int i = startIndex; i < str.Length; i++) {
                if (equalSymbols.ContainsKey(str[i]) || equalSymbols.ContainsValue(str[i])) {
                    return i;
                }
            }
            return -1;
        }

        public static int LastZeroIndex(byte[] array) {
            int index = array.Length - 1;
            while (index != -1 && array[index] == 0) {
                index--;
            }
            return index;
        }

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
    }
}
