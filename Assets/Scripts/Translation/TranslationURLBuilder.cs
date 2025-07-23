using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

internal sealed class TranslationURLBuilder : IDisposable {
    public enum e_languages {
        japanese = 0,
        english = 1,
        max,
    }

    string _deployID = default;
    private readonly string URL = @"https://script.google.com/macros/s/";
    private readonly string EXECUTE = @"/exec";

    public readonly Dictionary<e_languages, string> LANGUAGES = new Dictionary<e_languages, string> {
        { e_languages.japanese, @"ja" },
        { e_languages.english, @"en" },
    };

    public TranslationURLBuilder(in string deplayID) {
        _deployID = deplayID;
    }

    public string GetTranslationQuery(in string text, in string source, in string target) {
        StringBuilder url = new StringBuilder(URL + _deployID + EXECUTE + @"?text=" + text + @"&source=" + source + @"&target=" + target);
        return url.ToString();
    }

    public void Dispose() {
        _deployID = null;
    }
}
