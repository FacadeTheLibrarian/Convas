using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

internal sealed class SpeakerIDHandler {
    private VoiceVoxURLBuilder _urlBuilder = default;
    private int _speakerID = 8;

    //8
    //68

    public int GetSpeakerID {
        get { return _speakerID; }
    }

    public SpeakerIDHandler(in VoiceVoxURLBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }

    public async UniTask FetchSpeakerIDDictionary() {
        string url = _urlBuilder.GetSpeakers();

        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET)) {
            request.SetRequestHeader(WebUtility.CONTENT_TYPE, WebUtility.APPLICATION_JSON);
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest();

            if (request.responseCode == WebUtility.HTTP_STATUS_OKAY) {
            }
        }
    }
}
