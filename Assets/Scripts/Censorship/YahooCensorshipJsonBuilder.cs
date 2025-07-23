using System;
using System.Text;
using UnityEngine;

internal sealed class YahooCensorshipJsonBuilder {
    [Serializable]
    private class Params {
        public string q = default;
        public Params(in string prompt) {
            q = prompt;
        }
    }

    [Serializable]
    private class YahooCensorshipJson {
        public string id;
        public string jsonrpc;
        public string method;
        public Params @params;
        public YahooCensorshipJson(in string inputId, in string inputPrompt) {
            id = inputId;
            jsonrpc = "2.0";
            method = "jlp.maservice.parse";
            @params = new Params(inputPrompt);
        }
    }

    public YahooCensorshipJsonBuilder() { }

    public byte[] BuildJson(in string inputId, in string inputPrompt){
        string json = JsonUtility.ToJson(new YahooCensorshipJson(inputId, inputPrompt), false);
        Debug.Log(json);
        return Encoding.UTF8.GetBytes(json);
    }
}
