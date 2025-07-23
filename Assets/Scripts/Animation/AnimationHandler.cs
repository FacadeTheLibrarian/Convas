using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class AnimationHandler : BaseAnimationHandler
{
    private readonly int LISTENING = Animator.StringToHash("Listening");
    private readonly int PREPARATION = Animator.StringToHash("Preparation");
    private readonly int COMPLETE = Animator.StringToHash("Complete");
    private readonly int GREETING = Animator.StringToHash("Greeting");
    private readonly int READING = Animator.StringToHash("Reading");
    private readonly int IDLE = Animator.StringToHash("Idle");

    private Animator _modelAnimator = default;
    private Animator _spriteAnimator = default;

    public AnimationHandler(in Animator model, in Animator sprite) : base() {
        _modelAnimator = model;
        _spriteAnimator = sprite;
    }
    public override void OnCompleteInitialization() {
        _modelAnimator.enabled = true;
    }
    public override void OnIdle() {
        _spriteAnimator.Play(IDLE);
    }
    public override void OnListening() {
        _spriteAnimator.Play(LISTENING);
    }
    public override void OnStartPreparation() {
        _spriteAnimator.Play(PREPARATION);
    }
    public override void OnCompletePreparation() {
        _spriteAnimator.Play(COMPLETE);
    }
    public override void OnGreeting() {
    }
    public override void OnReading() {

    }
    public override void Dispose() {
        _spriteAnimator = null;
        _modelAnimator = null;
    }
}
