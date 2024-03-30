using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.Serialization;

public class Message : MonoBehaviour
{
    // ----------------------------Repair Event---------------------------
    [Serializable]
    public class RepairEvent : UnityEvent { }
    [FormerlySerializedAs("MessageRepairEvent")]
    [SerializeField]
    private RepairEvent m_RepairEvent = new RepairEvent();
    public RepairEvent MessageRepairEvent
    {
        set { m_RepairEvent = value; }
        get { return m_RepairEvent; }
    }

    // ----------------------------Game Data------------------------------
    private MessageContainer messageContainer;
    public bool isLost = false;
    public bool isUseless;
    public int order;

    public char messageName;
    public char MessageName
    {
        get { return messageName; }
        set { messageName = value; }
    }
    public string messageContent;

    Image backImage;
    TextMeshProUGUI textMesh;
    Image buttonImage;
    AudioSource messageRepairAudio;

    #region MonoBehaviourCalls
    /// Mono Runtime Functions
    private void OnEnable()
    {   
        messageContainer = GetComponentInParent<MessageContainer>();
        backImage = transform.GetComponentsInChildren<Image>()[0];
        textMesh = transform.GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = transform.GetComponentsInChildren<Image>()[1];
        messageRepairAudio = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (isLost == true)
        {
            backImage.color = messageContainer.errorColor;
            textMesh.color = messageContainer.errorColor;
            textMesh.text = "Error";
            buttonImage.color = messageContainer.errorColor;

            foreach (var button in GetComponentsInChildren<Button>())
            {
                button.enabled = false;
            }
        }

        else
        {
            backImage.color = messageContainer.normalColor;
            textMesh.color = messageContainer.normalColor;
            textMesh.text = messageContent;
            buttonImage.color = messageContainer.normalColor;
        }
    }
    #endregion

    #region APIs
    /// <summary>
    /// Function to repair message
    /// </summary>
    public void RepairLostMessage()
    {
        isLost = false;
        backImage.color = messageContainer.normalColor;
        textMesh.color = messageContainer.normalColor;
        buttonImage.color = messageContainer.normalColor;
        textMesh.text = messageContent;
        messageRepairAudio.Play();

        foreach (var button in GetComponentsInChildren<Button>())
        {
            button.enabled = true;
        }

        m_RepairEvent.Invoke();
    }

    /// <summary>
    /// Set message not used to repair
    /// </summary>
    public void SetMessageUseless()
    {
        if (isUseless == false)
        {
            isUseless = true;
            backImage.color = messageContainer.disabledColor;
            textMesh.color = messageContainer.disabledColor;
            buttonImage.color = messageContainer.disabledColor;
        }
        else
        {
            isUseless = false;
            if (isLost == false)
            {
                backImage.color = messageContainer.normalColor;
                textMesh.color = messageContainer.normalColor;
                buttonImage.color = messageContainer.normalColor;
            }
            else
            {
                backImage.color = messageContainer.errorColor;
                textMesh.color = messageContainer.errorColor;
                buttonImage.color = messageContainer.errorColor;
            }
        }
    }
    #endregion
}