using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class StartMenuButtons : MonoBehaviour
{
    public GameObject ContinueButton;
    public Color disableColor;
    void Start()
    {
        if (!File.Exists(Application.streamingAssetsPath + "/Saves" + "/playerSave.save"))
        {
            ContinueButton.GetComponent<Button>().interactable = false;
            ContinueButton.GetComponentInChildren<TextMeshProUGUI>().color = disableColor;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BeginNewGame()
    {
        var slManager = GameObject.Find("Save&LoadManager").GetComponent<SaveAndLoadManager>();
        slManager.playerData = new SaveAndLoadManager.SaveData();
    }
}
