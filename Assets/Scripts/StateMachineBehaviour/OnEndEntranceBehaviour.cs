using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class OnEndEntranceBehaviour : StateMachineBehaviour {
    public event Action OnEndEntranceAnimation = default;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        OnEndEntranceAnimation?.Invoke();
    }
}
