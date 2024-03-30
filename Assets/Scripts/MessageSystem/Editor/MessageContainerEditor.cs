using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MessageContainer))]
public class MessageContainerEditor : Editor
{
    private MessageContainer messageContainer;

    private void OnEnable()
    {
        messageContainer = target as MessageContainer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Create New Message"))
        {
            messageContainer.CreateNewMessage();
        }
    }
}
