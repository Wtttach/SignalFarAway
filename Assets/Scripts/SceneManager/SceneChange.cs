using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{

    IEnumerator SceneChangeCoroutine(int index)
    {
        float loadTime = 1f;
        AsyncOperation sceneLoad =  SceneManager.LoadSceneAsync(index);
        sceneLoad.allowSceneActivation = false;

        while (sceneLoad.progress < 0.9f)
        {
            loadTime -= Time.deltaTime;
            yield return null;
        }

        while (loadTime > 0f)
        {
            loadTime -= Time.deltaTime;
            yield return null;
        }

        sceneLoad.allowSceneActivation = true;
    }

    public void ChangeToSceneIndex(int index)
    {
        this.transform.GetChild(1).gameObject.SetActive(true);
        var SL = GameObject.Find("Save&LoadManager").GetComponent<SaveAndLoadManager>();
        SL.playerData.currentLevel = index;
        StartCoroutine(SceneChangeCoroutine(index));
    }
}
