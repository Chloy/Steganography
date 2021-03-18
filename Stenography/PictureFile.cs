using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Stenography {
    class PictureFile {
        public static Bitmap Hide(Bitmap image, byte[] infoToHide) {
            byte[] rgb = new byte[3];
            int byteIndex = 0;
            short bitIndex = 0;
            for (int i = 0; i < image.Width; i++) {
                for(int j = 0; j < image.Height; j++) {
                    if(byteIndex >= infoToHide.Length) {
                        rgb[0] = image.GetPixel(i, j).R;
                        rgb[1] = image.GetPixel(i, j).G;
                        rgb[2] = image.GetPixel(i, j).B;
                        rgb[0] &= 0b11111110;
                        rgb[1] &= 0b11111110;
                        rgb[2] &= 0b11111110;
                        image.SetPixel(i, j, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    }
                    else {
                        rgb[0] = image.GetPixel(i, j).R;
                        rgb[1] = image.GetPixel(i, j).G;
                        rgb[2] = image.GetPixel(i, j).B;
                        for (int k = 0; k < 3; k++) {
                            if (bitIndex == 8) {
                                bitIndex = 0;
                                byteIndex++;
                            }
                            if (byteIndex == infoToHide.Length) break;
                            byte _byte = infoToHide[byteIndex];
                            _byte <<= bitIndex;
                            _byte >>= 7;
                            bitIndex++;
                            if (_byte == 0) {
                                rgb[k] &= 0b11111110;
                            }
                            else {
                                rgb[k] |= 00000001;
                            }
                        }
                        image.SetPixel(i, j, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    }
                }
            }
            return image;
        }

        public static byte[] Unhide(Bitmap image) {
            byte[] result = new byte[image.Width * image.Height * 3 / 8];
            int byteIndex = 0;
            short bitIndex = 0;
            for(int i = 0; i < image.Width; i++) {
                for(int j = 0; j < image.Height; j++) {
                    byte[] rgb = new byte[3];
                    rgb[0] = image.GetPixel(i, j).R;
                    rgb[1] = image.GetPixel(i, j).G;
                    rgb[2] = image.GetPixel(i, j).B;
                    for(int k = 0; k < 3; k++) {
                        if(bitIndex == 8) {
                            bitIndex = 0;
                            byteIndex++;
                        }
                        result[byteIndex] <<= 1;
                        byte _byte = rgb[k];
                        _byte &= 0b00000001;
                        if (_byte == 0) 
                            result[byteIndex] &= 0b11111110;
                        else result[byteIndex] |= 0b00000001;
                        bitIndex++;
                    }

                }
            }
            Array.Resize(ref result, Functions.LastZeroIndex(result));
            return result;
        }
    }
}
