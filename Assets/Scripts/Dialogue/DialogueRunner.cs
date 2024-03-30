using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueRunner : MonoBehaviour
{
    private int contentTotal;
    private int audioTotal;
    private int contentPointer = 0;
    private int audioPointer = 0;

    public float contextTimeParam;

    private bool isOpen = false;

    Image speakerIcon;
    TextMeshProUGUI dialogueContent;
    TextMeshProUGUI speakerName;
    AudioSource speakerAudioSource;

    private int dialoguePointer = 0;
    public List<Dialogue> dialogueAsset = new List<Dialogue>();

    RectTransform rectTransform;

    Vector3 targetPos = new Vector3(0f, 0f, 0.00f),
        originalPos = new Vector3(0, 400f, 0.00f);

    IEnumerator audioPlay, textPlay;
    

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        speakerIcon = GetComponentInChildren<Image>();
        speakerAudioSource = GetComponent<AudioSource>();
        dialogueContent = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        speakerName = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    #region IEnumerator
    IEnumerator AudioPlayEnd(AudioClip audioClip, Action callback)
    {
        yield return new WaitForSeconds(audioClip.length);

        callback();
    }

    IEnumerator TextPlayEnd(string context, Action callback)
    {
        yield return new WaitForSeconds(context.Length * contextTimeParam);

        callback();
    }

    IEnumerator ShowDialogueWindw()
    {
        while ((rectTransform.anchoredPosition3D - targetPos).magnitude > 0.0001f)
        {
            rectTransform.anchoredPosition3D = Vector3.MoveTowards(rectTransform.anchoredPosition3D, targetPos, 15f);
            yield return null;
        }
    }

    IEnumerator HideDialogueWindw()
    {
        while ((rectTransform.anchoredPosition3D - originalPos).magnitude > 0.0001f)
        {
            rectTransform.anchoredPosition3D = Vector3.MoveTowards(rectTransform.anchoredPosition3D, originalPos, 15f);
            yield return null;
        }
    }

    #endregion

    #region APIs
    public void BeginDialogue(int index)
    {
        var databox = GameObject.Find("DataBox");
        var interactCom = databox.GetComponent<Item3DInteractive>();
        if (interactCom != null) interactCom.enabled = false;

        if (speakerAudioSource.isPlaying) speakerAudioSource.Stop();

        if (audioPlay != null)
            StopCoroutine(audioPlay);
        if (textPlay != null)
            StopCoroutine(textPlay);

        isOpen = true;
        dialoguePointer = index;

        contentTotal = dialogueAsset[dialoguePointer].dialogueContents.Length;
        audioTotal = dialogueAsset[dialoguePointer].dialogueAudios.Length;
        contentPointer = 0;
        audioPointer = 0;

        speakerName.text = dialogueAsset[dialoguePointer].speakerName;
        speakerIcon.sprite = dialogueAsset[dialoguePointer].speakerIcon;
        dialogueContent.text = dialogueAsset[dialoguePointer].dialogueContents[contentPointer];
        speakerAudioSource.clip = dialogueAsset[dialoguePointer].dialogueAudios[audioPointer];

        speakerAudioSource.Play();
        audioPlay = AudioPlayEnd(speakerAudioSource.clip, () => { MoveToNextDialogue(); });
        StartCoroutine(audioPlay);
        textPlay = TextPlayEnd(dialogueContent.text, () => { MoveToNextText(); });
        StartCoroutine(textPlay);
        StartCoroutine(ShowDialogueWindw());
    }

    private void MoveToNextDialogue()
    {
        audioPointer++;
        if (audioPointer >= audioTotal) { EndDialogue(); return; }
        
        speakerAudioSource.clip = dialogueAsset[dialoguePointer].dialogueAudios[audioPointer];
        speakerAudioSource.Play();
        audioPlay = AudioPlayEnd(speakerAudioSource.clip, () => { MoveToNextDialogue(); });
        StartCoroutine(audioPlay);
    }

    private void MoveToNextText()
    {
        contentPointer++;
        Debug.Log("text: " + contentPointer + "/" + contentTotal);
        if (contentPointer >= contentTotal) { EndDialogue(); return; }
        
        dialogueContent.text = dialogueAsset[dialoguePointer].dialogueContents[contentPointer];
        textPlay = TextPlayEnd(dialogueContent.text, () => { MoveToNextText(); });
        StartCoroutine(textPlay);
    }

    public void EndDialogue()
    {
        if (isOpen == false) return;

        var databox = GameObject.Find("DataBox");
        var interactCom = databox.GetComponent<Item3DInteractive>();
        if (interactCom != null) interactCom.enabled = true;

        isOpen = false;
        StartCoroutine(HideDialogueWindw());
    }
    #endregion
}
