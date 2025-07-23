using System;
using System.Text;
using UnityEngine;

public class GeminiJsonBuilder {
    [Serializable]
    private class GeminiText {
        public string text;
        public GeminiText(string inputText) {
            text = inputText;
        }
    }
    [Serializable]
    private class GeminiParts<T> {
        public T[] parts;

        public GeminiParts(params T[] input) {
            parts = input;
        }
    }
    [Serializable]
    private class GeminiContents<T> {
        public T[] contents;
        public GeminiContents(params T[] input) {
            contents = input;
        }
    }

    public GeminiJsonBuilder() { }

    public byte[] GetJson(in string rawPrompt) {

        StringBuilder promptShiftJIS = new StringBuilder(rawPrompt + @"最大5行で簡潔に答えてください。また、文末はですます調とします。");
        string promptUTF8 = EncodeUtility.ShiftJIStoUTF8(promptShiftJIS.ToString());

        GeminiText[] texts = new GeminiText[]
        {
            new GeminiText(promptUTF8),
        };

        GeminiParts<GeminiText> parts = new GeminiParts<GeminiText> {
            parts = texts,
        };

        GeminiParts<GeminiText>[] formedParts = new GeminiParts<GeminiText>[]
        {
            parts,
        };
        GeminiContents<GeminiParts<GeminiText>> contents = new GeminiContents<GeminiParts<GeminiText>> {
            contents = formedParts,
        };

        return Encoding.UTF8.GetBytes(JsonUtility.ToJson(contents));
    }
}
