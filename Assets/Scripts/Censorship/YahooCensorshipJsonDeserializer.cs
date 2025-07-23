using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

internal sealed class YahooCensorshipJsonDeserializer
{
    private const int HIRAGANA = 1;
    public YahooCensorshipJsonDeserializer() { }
    public YahooCensorshipData Deserialize(in string json) {
        JObject parsedJson = JObject.Parse(json);
        string id = parsedJson["id"].Value<string>();
        List<string> hiraganaTexts = new List<string>();
        for (int i = 0; i < 100; i++){
            string hiragana = "";
            try {
                hiragana = parsedJson["result"]["tokens"][i][HIRAGANA].Value<string>();
            }
            catch {
                break;
            }
            hiraganaTexts.Add(hiragana);
        }
        YahooCensorshipData data = new YahooCensorshipData(id, hiraganaTexts);
        return data;
    }
}
