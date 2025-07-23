using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System.Linq;

internal sealed class GeminiJsonDeserializer {

    public GeminiJsonDeserializer() { }

    public string Deerialize(in string json) {
        JObject parsedJson = JObject.Parse(json);
        string text = parsedJson["candidates"][0]["content"]["parts"][0]["text"].Value<string>();
        return text;
    }
}
