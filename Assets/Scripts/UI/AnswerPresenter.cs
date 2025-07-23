using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class AnswerPresenter : MonoBehaviour, IDisposable {
    [SerializeField] private AnswerView _answerView = default;
    [SerializeField] private BaseChatSystem _model = default;

    public void SetUp(in BaseChatSystem model) {
        _model = model;
        _model.OnAnswerReady += _answerView.OnAnswerReady;
    }

    public void Dispose() {
        _model.OnAnswerReady -= _answerView.OnAnswerReady;
    }
}
