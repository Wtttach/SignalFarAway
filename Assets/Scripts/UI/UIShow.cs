using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShow : MonoBehaviour
{
    public Graphic _graphic;
    public bool visible;

    public void ChangeUIState(bool value)
    {
        if (value == visible) return;
        visible = value;

        if (_graphic == null) return;
        if (visible) _graphic.canvasRenderer.SetAlpha(1f);
        else _graphic.canvasRenderer.SetAlpha(0f);
    }

    private void OnEnable()
    {
        _graphic.canvasRenderer.SetAlpha(visible ? 1f : 0f);
    }
}
