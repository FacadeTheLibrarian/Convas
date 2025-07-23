using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

internal sealed class GameMain : IDisposable {
    private BaseDictation _dictation = default;
    private BaseInput _input = default;
    private BaseVoiceSynthesisClient _voiceClient = default;
    private BaseChatSystem _chatSystem = default;
    private BaseAnimationHandler _animationHandler = default;

    private AudioSource _audioSource = default;
    private CancellationToken _token = default;

    public GameMain(in BaseInput inputAction, in BaseDictation dictation, in BaseVoiceSynthesisClient voiceClient, in BaseChatSystem chatSystem, in BaseAnimationHandler animationHandler,in AudioSource audioSource, CancellationToken token) {
        _token = token;
        _dictation = dictation;
        _input = inputAction;
        _voiceClient = voiceClient;
        _chatSystem = chatSystem;
        _animationHandler = animationHandler;
        _audioSource = audioSource;

        _input.OnStart += StartDictating;
        _chatSystem.OnAnswerReady += AnswerReady;
        _dictation.OnComplete += DictationComplete;
    }
    public void Update() {
        _input.Update();
    }

    public void Dispose() {
        _input.OnStart -= StartDictating;
        _dictation.OnComplete -= DictationComplete;
        _dictation.Dispose();
        _input.Dispose();
        _voiceClient.Dispose();
        _chatSystem.Dispose();
        _audioSource = null;
    }
    private void StartDictating() {
        _animationHandler.OnListening();
        _dictation.StartListening();
    }
    private void DictationComplete(string text) {
        _animationHandler.OnIdle();
        _ = InnerDictationComplete(text);
    }
    private void AnswerReady(string text) {
        _animationHandler.OnIdle();
    }
    private async UniTask InnerDictationComplete(string text) {
        string answer = "";
        AudioClip answerClip = default;
        try {
            answer = await _chatSystem.BuildAnswerAsync(text, _token);
            answerClip = await _voiceClient.GetVoiceClipAsync(answer, _token);
        }
        catch (OperationCanceledException) {
            throw new OperationCanceledException();
        }
        if (answerClip) {
            _audioSource.PlayOneShot(answerClip);
        }
    }
}
