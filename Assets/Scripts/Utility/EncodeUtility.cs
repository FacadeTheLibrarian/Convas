using System.Text;

internal sealed class EncodeUtility {
    static public string ShiftJIStoUTF8(in string textInShiftJIS) {
        Encoding UTF8Encode = Encoding.GetEncoding("UTF-8");
        byte[] byteCode = Encoding.UTF8.GetBytes(textInShiftJIS);
        string textInUTF8 = UTF8Encode.GetString(byteCode);
        return textInUTF8;
    }
}
