using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneIntroClose : MonoBehaviour
{
    public float closeTime;
    IEnumerator CloseCountDown(float close)
    {
        while (close > 0.5f)
        {
            close -= Time.deltaTime;
            yield return null;
        }
        while (close > 0f)
        {
            close -= Time.deltaTime;
            Color imageColor = GetComponentInChildren<Image>().color;
            imageColor.a = close * 2;
            GetComponentInChildren<Image>().color = imageColor;

            imageColor = GetComponentInChildren<TextMeshProUGUI>().color;
            imageColor.a = close * 2;
            GetComponentInChildren<TextMeshProUGUI>().color = imageColor;

            yield return null;
        }

        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(CloseCountDown(closeTime));
    }
}
