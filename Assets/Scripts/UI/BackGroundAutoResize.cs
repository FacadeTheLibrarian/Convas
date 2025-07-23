using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal sealed class BackGroundAutoResize : MonoBehaviour {
    [SerializeField] private Image _backgroundImage = default;

    public void ExecuteResize() {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        _backgroundImage.rectTransform.sizeDelta = screenSize;
    }
}
