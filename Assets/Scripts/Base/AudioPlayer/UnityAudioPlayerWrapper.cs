using System.Collections.Generic;
using System.Threading;
using Cysharp;
using Cysharp.Threading.Tasks;
using UnityEngine;

internal sealed class UnityAudioPlayerWrapper {
	private AudioSource _audioSource = default;
	private Queue<AudioClip> _audioQueue = default;

    public UnityAudioPlayerWrapper(in AudioSource source) {
		_audioSource = source;
		_audioSource.playOnAwake = false;
		_audioSource.loop = false;
		_audioQueue = new Queue<AudioClip>();
    }
	public async UniTask PlayAudio(AudioClip clip, CancellationToken token)
	{
		_audioSource.clip = clip;
		while (_audioSource.isPlaying)
		{
			await UniTask.Yield(token);
        }
    }
}