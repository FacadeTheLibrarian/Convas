using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal sealed class AnswerView : MonoBehaviour
{
    [SerializeField] private Text _answerText = default;
    #region DEBUG
#if UNITY_EDITOR
    private void Reset() {
        if(!TryGetComponent(out _answerText)) {
            _answerText = this.gameObject.AddComponent<Text>();
        }
    }
#endif
    #endregion
    public void OnAnswerReady(string text) {
        _answerText.text = text;
    }
}
