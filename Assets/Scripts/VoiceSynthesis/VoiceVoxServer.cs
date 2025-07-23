using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;

internal sealed class VoiceVoxServer : BaseVoiceSynthesisServer
{
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    private readonly string FOLDER_PATH = Application.streamingAssetsPath;
    private readonly string PATH = @"/VOICEVOX/VOICEVOX.exe";
    private readonly string PROCESS_NAME = "VOICEVOX";

    private const int ITERATION_DELAY = 1000;

    private Process _voiceVoxProcess = default;
    private bool _isUsingExistedProcess = false;

    public VoiceVoxServer() : base() { }

    public override async UniTask AwakeServerAsync(CancellationToken token) {

        if(IsVoiceVoxProcessExisted()) {
            _isUsingExistedProcess = true;
            return;
        }

        _voiceVoxProcess = new Process();
        _voiceVoxProcess.StartInfo = new ProcessStartInfo {
            FileName = FOLDER_PATH + PATH,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        _voiceVoxProcess.Start();
        while (_voiceVoxProcess.MainWindowHandle == IntPtr.Zero) {
            try {
                await UniTask.Delay(ITERATION_DELAY, cancellationToken: token, cancelImmediately: true);
            }
            catch (OperationCanceledException){
                throw new OperationCanceledException();
            }
        }
        IntPtr hWnd = _voiceVoxProcess.MainWindowHandle;
        ShowWindow(hWnd, 0);
    }
    private bool IsVoiceVoxProcessExisted() {
        Process[] possibleProcess = Process.GetProcessesByName(PROCESS_NAME);
        if (possibleProcess.Length <= 0) {
            return false;
        }
        return true;
    }
    public override void Dispose() {
        if (_isUsingExistedProcess) {
            return;
        }
        DisposeProcess();
    }
    private void DisposeProcess() {
        if (_voiceVoxProcess == null) {
            return;
        }
        if (_voiceVoxProcess.HasExited) {
            return;
        }
        _voiceVoxProcess.CloseMainWindow();
        _voiceVoxProcess.Dispose();
        _voiceVoxProcess = null;
    }
}
