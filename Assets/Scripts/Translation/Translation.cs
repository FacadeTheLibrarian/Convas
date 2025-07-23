using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

internal sealed class Translation : IDisposable {

    private TranslationURLBuilder _urlBuilder = default;
    public event Func<string, UniTask> OnCompleteTranslating;

    public Translation(in string deployID) {
        _urlBuilder = new TranslationURLBuilder(deployID);
    }

    public async UniTask Translate(string text, TranslationURLBuilder.e_languages source, TranslationURLBuilder.e_languages target) {
        string url = _urlBuilder.GetTranslationQuery(text, _urlBuilder.LANGUAGES[source], _urlBuilder.LANGUAGES[target]);

        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET)) {
            request.SetRequestHeader(WebUtility.CONTENT_TYPE, WebUtility.APPLICATION_JSON);
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest();

            if (request.responseCode == WebUtility.HTTP_STATUS_OKAY) {
                Debug.Log(request.downloadHandler.text);
                return;
            }
        }
    }

    public void Dispose() {
        _urlBuilder.Dispose();
        _urlBuilder = null;
        OnCompleteTranslating = null;
    }
}
