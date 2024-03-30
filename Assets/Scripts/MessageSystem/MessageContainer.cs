using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageContainer : MonoBehaviour
{
    
    public Color errorColor;
    public Color normalColor;
    public Color disabledColor;
    public List<string> endings;

    public GameObject messageInstance;

    static private char currentMessageName = 'A';

    #region MonoBehaviour


    #endregion

    #region APIs
    /// <summary>
    /// Create New Messages
    /// </summary>
    public void CreateNewMessage()
    {
        var newMessage = GameObject.Instantiate(messageInstance);
        newMessage.transform.SetParent(transform.GetChild(1).GetChild(0), true);
        var messageRect = newMessage.GetComponent<RectTransform>();
        messageRect.anchoredPosition3D = messageRect.anchoredPosition3D - new Vector3(0, 0, messageRect.anchoredPosition3D.z);
        messageRect.localScale = Vector3.one;
        messageRect.localEulerAngles = Vector3.zero;

        var currentMessage = newMessage.GetComponent<Message>();
        currentMessage.MessageName = currentMessageName;
        newMessage.name = "Message" + currentMessageName;

        currentMessageName++;
    }


    #endregion

    #region Callbacks
    public void CompleteMessage()
    {
        string result = new string("");
        for (int i = 0; i < transform.GetChild(1).GetChild(0).childCount; i++)
        {
            Message message = transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Message>();

            if (message.isUseless == true || message.isLost == true) continue;

            result += message.messageName;
        }

        var SL = GameObject.Find("Save&LoadManager").GetComponent<SaveAndLoadManager>();

        SL.playerData.currentLevel = SceneManager.GetActiveScene().buildIndex;
        SL.playerData.levelResults[SL.playerData.currentLevel - 1] = result;

        SL.SaveGame();
    }

    #endregion
}