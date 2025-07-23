using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

internal sealed class UnityInput : BaseInput {
    public UnityInput() : base() { }
    public override void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            PublishStart();
        }
    }

    public override void Dispose() {}
}
