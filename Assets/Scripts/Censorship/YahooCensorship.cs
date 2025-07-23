using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

internal sealed class YahooCensorship : BaseCensorship
{
    private const string URL = @"https://jlp.yahooapis.jp/MAService/V2/parse";

    private NecessaryProperties _properties = default;
    private YahooCensorshipJsonBuilder _jsonBuilder = default;
    private YahooCensorshipJsonDeserializer _jsonDeserializer = default;
    public YahooCensorship(in IBadWords badWords, in NecessaryProperties properties) : base(badWords) {
        _properties = properties;
        _jsonBuilder = new YahooCensorshipJsonBuilder();
        _jsonDeserializer = new YahooCensorshipJsonDeserializer();
    }

    public override async UniTask<e_censorshipResult> IsBadWordIncluded(string input, CancellationToken token) {
        using (UnityWebRequest request = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbPOST)) {
            request.SetRequestHeader(WebUtility.CONTENT_TYPE, WebUtility.APPLICATION_JSON);
            request.SetRequestHeader(WebUtility.USER_AGENT, @$"Yahoo AppID: {_properties[(int)NecessaryProperties.e_properties.YahooAppId]}");
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            int randomID = Random.Range(0, int.MaxValue);
            string userId = randomID.ToString();
            byte[] parameter = _jsonBuilder.BuildJson(userId, input);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(parameter);
            try {
                token.ThrowIfCancellationRequested();
                await request.SendWebRequest();
            }
            catch {
                Debug.Log("通信エラーが発生しました");
                return e_censorshipResult.ERROR;
            }

            if(request.responseCode == WebUtility.HTTP_STATUS_OKAY) {
                YahooCensorshipData data = _jsonDeserializer.Deserialize(request.downloadHandler.text);
                if(data.GetId != userId) {
                    return e_censorshipResult.ERROR;
                }
                List<string> badWords = _badWords.GetBadWords();
                foreach (string word in data.GetHiraganaTexts) {
                    if (badWords.Contains(word)) {
                        Debug.Log($"NGワードが含まれています: {word}");
                        return e_censorshipResult.NG;
                    }
                }
            }
        }
        return e_censorshipResult.OK;
    }
}
