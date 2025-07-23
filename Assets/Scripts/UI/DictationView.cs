using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

internal sealed class DictationView : MonoBehaviour
{
    [SerializeField] private Text _dictationText = default;
#if UNITY_EDITOR
    private void Reset() {
        if(!TryGetComponent(out _dictationText)) {
            _dictationText = this.gameObject.AddComponent<Text>();
        }
    }
#endif
    public void OnHypothesisUpdated(string text) {
        _dictationText.text = text;
    }
    public void OnResult(string text) {
        _dictationText.text = text;
    }
}
