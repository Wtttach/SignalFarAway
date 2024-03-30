using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : ScriptableObject
{
    public string speakerName;
    public Sprite speakerIcon;
    [Multiline(3)]
    public string[] dialogueContents;
    public AudioClip[] dialogueAudios;
}
