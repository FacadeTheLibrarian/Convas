using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

internal sealed class VoiceVoxClient : BaseVoiceSynthesisClient {

    private const int TIMEOUT = 3;
    private const int SERVER_PING_ITERATION = 5;
    private VoiceVoxURLBuilder _urlBuilder = default;
    private SpeakerIDHandler _speakerID = default;

    public VoiceVoxClient() {
        _urlBuilder = new VoiceVoxURLBuilder();
        _speakerID = new SpeakerIDHandler(_urlBuilder);
    }
    public override void Dispose() {
        _urlBuilder = null;
        _speakerID = null;
    }

    public override async UniTask<AudioClip> GetVoiceClipAsync(string text, CancellationToken token) {
        bool isServerAlive = false;
        try {
            isServerAlive = await ServerPingAsync(token);
        }
        catch (OperationCanceledException){
            throw new OperationCanceledException();
        }

        if (!isServerAlive) {
            return null;
        }

        byte[] audioQuery = default;
        AudioClip clip = default;
        try {
            audioQuery = await IssueAudioQuery(_speakerID.GetSpeakerID, text, token);
            clip = await PostSynthesis(_speakerID.GetSpeakerID, audioQuery, token);
        }
        catch (OperationCanceledException){
            throw new OperationCanceledException();
        }

        if (!clip) {
            return null;
        }
        return clip;
    }
    public override async UniTask<bool> ServerPingAsync(CancellationToken token) {
        int iteration = 0;
        string url = _urlBuilder.GetIsSpeakerInitialized(_speakerID.GetSpeakerID);
        while (iteration < SERVER_PING_ITERATION) {
            using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET)) {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.timeout = TIMEOUT;
                try {
                    token.ThrowIfCancellationRequested();
                    await request.SendWebRequest();
                }
                catch (OperationCanceledException) {
                    throw new OperationCanceledException();
                }
                catch {
                    #region DEBUG
#if UNITY_EDITOR
                    Debug.LogError("サーバーとの通信に失敗しました");
#endif
                    #endregion
                    return false;
                }
                if (request.responseCode == WebUtility.HTTP_STATUS_OKAY) {
                    #region DEBUG
#if UNITY_EDITOR
                    Debug.Log("VoiceVoxサーバーは稼働中です");
#endif
                    #endregion
                    return true;
                }
            }
        }
        #region DEBUG
#if UNITY_EDITOR
        Debug.LogError("VoiceVoxサーバーは稼働していません");
#endif
        #endregion
        return false;
    }
    private async UniTask<byte[]> IssueAudioQuery(int speakerId, string text, CancellationToken token) {
        string url = _urlBuilder.GetAudioQuery(speakerId, text);

        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)) {
            request.downloadHandler = new DownloadHandlerBuffer();
            try {
                token.ThrowIfCancellationRequested();
                await request.SendWebRequest();
            }
            catch (OperationCanceledException) {
                throw new OperationCanceledException();
            }
            catch {
                #region DEBUG
#if UNITY_EDITOR
                Debug.LogError("サーバーとの通信に失敗しました");
#endif
                #endregion
                return null;
            }
            if (request.responseCode == WebUtility.HTTP_STATUS_OKAY) {
                return request.downloadHandler.data;
            }
            #region DEBUG
#if UNITY_EDITOR
            Debug.LogError($"音声クエリの取得に失敗しました: {request.responseCode}");
#endif
            #endregion
        }
        return null;
    }

    //INFO: VoiceVoxが音声合成をPOSTで受け付けるのでGETしかないWebRequestMultiMediaが使えない
    //UPDATE: 使えました プロパティで変えてください
    private async UniTask<AudioClip> PostSynthesis(int speakerId, byte[] audioQuery, CancellationToken token) {
        string url = _urlBuilder.GetSynthesisVoice(speakerId);

        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV)) {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader(WebUtility.CONTENT_TYPE, WebUtility.APPLICATION_JSON);
            request.uploadHandler = new UploadHandlerRaw(audioQuery);
            try {
                token.ThrowIfCancellationRequested();
                await request.SendWebRequest();
            }
            catch (OperationCanceledException) {
                throw new OperationCanceledException();
            }
            catch {
                #region DEBUG
#if UNITY_EDITOR
                Debug.LogError("サーバーとの通信に失敗しました");
#endif
                #endregion
                return null;
            }
            if (request.responseCode == WebUtility.HTTP_STATUS_OKAY) {
                return DownloadHandlerAudioClip.GetContent(request);
            }
            #region DEBUG
#if UNITY_EDITOR
            Debug.LogError($"音声クエリの取得に失敗しました: {request.responseCode}");
#endif
            #endregion
        }
        return null;
    }
}
