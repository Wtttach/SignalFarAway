using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorBlock))]
public class ColorBlockEditor : Editor
{
    private ColorBlock colorBlock;
    private SerializedProperty blockMatrix;

    private void OnEnable()
    {
        colorBlock = target as ColorBlock;
        blockMatrix = serializedObject.FindProperty("isBlock");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Color Canvas");

        serializedObject.Update();

        for (int i = 0; i < colorBlock.sizeY; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < colorBlock.sizeX; j++)
            {
                blockMatrix.GetArrayElementAtIndex(i * 20 + j).boolValue =
                    EditorGUILayout.Toggle(blockMatrix.GetArrayElementAtIndex(i * 20 + j).boolValue, GUILayout.Width(20f));
            }
            EditorGUILayout.EndHorizontal();
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}
