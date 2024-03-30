using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    RectTransform rectTransform;
    public Vector3 originalScale;
    public bool isOpen = false;

    public float animTime;
    

    IEnumerator ScaleIn()
    {
        float timer = 0;
        while (animTime - timer > 0.01f)
        {
            timer += Time.deltaTime;
            rectTransform.localScale = originalScale * (timer / animTime);
            yield return null;
        }

        rectTransform.localScale = originalScale;
    }
    IEnumerator ScaleOut()
    {
        float timer = animTime;
        while (timer > 0.01f)
        {
            timer -= Time.deltaTime;
            rectTransform.localScale = originalScale * (timer / animTime);
            yield return null;
        }

        rectTransform.localScale = Vector3.zero;
    }

    public void BeginScaleIn(bool value)
    {
        if (value == false) return;
        if (isOpen) return;

        isOpen = true;
        StartCoroutine(ScaleIn());
    }

    public void BeginScaleOut()
    {
        if (!isOpen) return;

        isOpen = false;
        StartCoroutine(ScaleOut());
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
