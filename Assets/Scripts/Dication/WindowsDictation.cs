using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows.Speech;

internal sealed class WindowsDictation : BaseDictation {
    private const float TIME_OUT = 3.0f;
    private const string ERROR_MESSAGE = "もう一度話してください";
    private DictationRecognizer _dictationRecognizer = default;

    public WindowsDictation() : base() {
        _dictationRecognizer = new DictationRecognizer();
        _dictationRecognizer.InitialSilenceTimeoutSeconds = TIME_OUT;

        _dictationRecognizer.DictationHypothesis += HypothesisUpdate;
        _dictationRecognizer.DictationResult += Complete;
        _dictationRecognizer.DictationError += Error;
    }
    public override void StartListening() {
        if (_dictationRecognizer.Status != SpeechSystemStatus.Running) {
            _dictationRecognizer.Start();
        }
    }
    public override void ForceStopListening() {
        if (_dictationRecognizer.Status == SpeechSystemStatus.Running) {
            _dictationRecognizer.Stop();
        }
    }

    private void HypothesisUpdate(string text) {
        PublishOnHypothesisUpdated(text);
    }
    private void Complete(string text, ConfidenceLevel confidence) {
        if (_dictationRecognizer.Status == SpeechSystemStatus.Running) {
            _dictationRecognizer.Stop();
        }
        PublishOnComplete(text);
    }
    private void Error(string error, int hresult) {
        PublishOnError(ERROR_MESSAGE);
    }
    public override void Dispose() {
        if (_dictationRecognizer.Status == SpeechSystemStatus.Running) {
            _dictationRecognizer.Stop();
        }
        _dictationRecognizer.Dispose();
    }
}
