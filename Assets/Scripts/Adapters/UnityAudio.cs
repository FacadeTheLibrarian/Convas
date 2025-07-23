using System;
using UnityEngine;

internal sealed class UnityAudio : Audio<AudioClip> {
    public override bool IsValid() => _audio;
    public UnityAudio(AudioClip audioClip) : base(audioClip) { }
    protected override void OnDispose() {
        if(_audio != null) {
            GameObject.Destroy(_audio);
        }
        _audio = null;
    }
}