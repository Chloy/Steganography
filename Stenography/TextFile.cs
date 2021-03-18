using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography {
    class TextFile {
        public TextFile() {

        }
        public static Dictionary<char, char> equalSymbols_RU_EN = new Dictionary<char, char> {//RU_EN
            {'\u0430', '\u0061'},//a
            {'\u0410', '\u0041'},//A
            {'\u0412', '\u0042'},//B
            {'\u0415', '\u0045'},//E
            {'\u0435', '\u0065'},//e
            {'\u041c', '\u004d'},//M
            {'\u041d', '\u0048'},//H
            {'\u041e', '\u004f'},//O
            {'\u043e', '\u006f'},//o
            {'\u0420', '\u0050'},//P
            {'\u0440', '\u0070'},//p
            {'\u0421', '\u0043'},//C
            {'\u0441', '\u0063'},//c
            {'\u0422', '\u0054'},//T
            {'\u0443', '\u0079'},//y
            {'\u0425', '\u0058'},//X
            {'\u0445', '\u0078'}//x
        };

        public static Dictionary<char, char> equalSymbols_EN_RU = new Dictionary<char, char> {//EN_RU
            {'\u0061', '\u0430'},//a
            {'\u0041', '\u0410'},//A
            {'\u0042', '\u0412'},//B
            {'\u0045', '\u0415'},//E
            {'\u0065', '\u0435'},//e
            {'\u004d', '\u041c'},//M
            {'\u0048', '\u041d'},//H
            {'\u004f', '\u041e'},//O
            {'\u006f', '\u043e'},//o
            {'\u0050', '\u0420'},//P
            {'\u0070', '\u0440'},//p
            {'\u0043', '\u0421'},//C
            {'\u0063', '\u0441'},//c
            {'\u0054', '\u0422'},//T
            {'\u0079', '\u0443'},//y
            {'\u0058', '\u0425'},//X
            {'\u0078', '\u0445'}//x
        };

        public static string Hide(string sourseStream, byte[] informationForHide) {
            int index = Functions.IndexOfAnySymbol(sourseStream, equalSymbols_RU_EN);
            char[] text = sourseStream.ToCharArray();
            for (int i = 0; i < informationForHide.Length; i++) {
                for (int j = 0; j < 8; j++) {
                    byte _byte = informationForHide[i];
                    _byte <<= j;
                    _byte >>= 7;
                    if (_byte == 1) {
                        char a = text[index];
                        if (!equalSymbols_RU_EN.ContainsValue(a)) {
                            text[index] = equalSymbols_RU_EN[a];
                        }
                    }
                    else if (_byte == 0 && equalSymbols_RU_EN.ContainsValue(sourseStream[index])) {
                        text[index] = equalSymbols_EN_RU[sourseStream[index]];
                    }
                    index = Functions.IndexOfAnySymbol(sourseStream, equalSymbols_RU_EN, ++index);
                    Console.WriteLine(i + " in " + informationForHide.Length);
                }
            }
            while (index != -1) {
                if (equalSymbols_EN_RU.ContainsKey(text[index])) {
                    text[index] = equalSymbols_EN_RU[text[index]];
                }
                index = Functions.IndexOfAnySymbol(sourseStream, equalSymbols_RU_EN, ++index);
            }
            sourseStream = new string(text);
            return sourseStream;
        }

        public static byte[] Unhide(string sourseStream, int characterCount) {
            byte[] hideninformation = new byte[characterCount / 8];
            int index = Functions.IndexOfAnySymbol(sourseStream, equalSymbols_RU_EN);
            for (int i = 0; i < hideninformation.Length; i++) {
                for (int j = 0; j < 8; j++) {
                    hideninformation[i] <<= 1;
                    if (equalSymbols_RU_EN.ContainsValue(sourseStream[index]))
                        hideninformation[i] += 1;
                    index++;
                    index = Functions.IndexOfAnySymbol(sourseStream, equalSymbols_RU_EN, index);
                }
            }
            Array.Resize(ref hideninformation, Functions.LastZeroIndex(hideninformation));
            return hideninformation;
        }
    }
}
