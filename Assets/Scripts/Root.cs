using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using System.Threading;

internal sealed class Root : MonoBehaviour {
    [SerializeField] private NecessaryProperties _necessaryProperties = default;
    [SerializeField] private BackGroundAutoResize _backgroundAutoResize = default;

    [SerializeField] private BadWordLibrary _badWordLibrary = default;
    [SerializeField] private AudioSource _audioSource = default;
    [SerializeField] private Animator _modelAnimator = default;
    [SerializeField] private Animator _spriteAnimator = default;

    [SerializeField] private DictationPresenter _dictationPresenter = default;
    [SerializeField] private AnswerPresenter _answerPresenter = default;

    private BaseVoiceSynthesisServer _server = default;

    private GameMain _main = default;

    private CancellationToken _token = default;

    private bool _isBooted = false;

    private void Awake() {
        _token = this.GetCancellationTokenOnDestroy();
        _ = Constructor(_token);
    }

    private async UniTask Constructor(CancellationToken token) {
        _backgroundAutoResize.ExecuteResize();
        BaseAnimationHandler animationHandler = new AnimationHandler(_modelAnimator, _spriteAnimator);
        animationHandler.OnStartPreparation();

        _server = new VoiceVoxServer();
        try {
            await _server.AwakeServerAsync(token);
        }
        catch (OperationCanceledException) {
            return;
        }
        BaseVoiceSynthesisClient client = new VoiceVoxClient();
        bool isServerAlive = false;
        try {
            isServerAlive = await client.ServerPingAsync(token);
        }
        catch (OperationCanceledException) {
            return;
        }

        BaseInput input = new UnityInput();
        BaseDictation dictation = new WindowsDictation();
        BaseChatSystem chatSystem = new ChatSystem(_badWordLibrary, _necessaryProperties);

        _answerPresenter.SetUp(chatSystem);
        _dictationPresenter.SetUp(dictation);

        _main = new GameMain(input, dictation, client, chatSystem, animationHandler, _audioSource, _token);
        animationHandler.OnCompleteInitialization();
        animationHandler.OnCompletePreparation();
        _isBooted = true;
    }

    private void Update() {
        if (!_isBooted) {
            return;
        }
        _main.Update();
    }
    private void OnDestroy() {
        _dictationPresenter.Dispose();
        _answerPresenter.Dispose();

        _main.Dispose();
        _main = null;

        _server.Dispose();
        _server = null;
    }

    private void EndGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

