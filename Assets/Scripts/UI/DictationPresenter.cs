using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class DictationPresenter : MonoBehaviour, IDisposable
{
    [SerializeField] private DictationView _view = default;
    [SerializeField] private BaseDictation _model = default;

    public void SetUp(in BaseDictation model) {
        _model = model;
        _model.OnHypothesisUpdated += _view.OnHypothesisUpdated;
        _model.OnComplete += _view.OnResult;
    }

    public void Dispose() {
        _model.OnHypothesisUpdated -= _view.OnHypothesisUpdated;
        _model.OnComplete -= _view.OnResult;
    }
}
