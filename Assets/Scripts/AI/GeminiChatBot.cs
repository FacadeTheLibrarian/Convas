using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.Networking;
using System.Threading;

internal sealed class GeminiChatBot : BaseAIChatBot {
    private NecessaryProperties _properties = default;
    private GeminiJsonBuilder _builder = default;
    private GeminiJsonDeserializer _serializer = default;

    public GeminiChatBot(in NecessaryProperties properties) : base() {
        _properties = properties;
        _builder = new GeminiJsonBuilder();
        _serializer = new GeminiJsonDeserializer();
    }
    public override void Dispose() {
        _properties = null;
        _builder = null;
        _serializer = null;
    }

    public override async UniTask<string> Ask(string prompt, CancellationToken token) {
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_properties[(int)NecessaryProperties.e_properties.GeminiApiKey]}";
        byte[] promptInByte = _builder.GetJson(prompt);

        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)) {
            request.SetRequestHeader(WebUtility.CONTENT_TYPE, WebUtility.APPLICATION_JSON);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.uploadHandler = new UploadHandlerRaw(promptInByte);

            await request.SendWebRequest();

            if(request.responseCode == WebUtility.HTTP_STATUS_OKAY) {
                return _serializer.Deerialize(request.downloadHandler.text);
            }
        }

        return "エラーが発生しました";
    }
}
