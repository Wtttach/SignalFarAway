using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorFillingSetting))]
public class ColorFillingEditor : Editor
{
    const int canvasSize = 20;
    private ColorFillingSetting colorFillingSetting;
    private SerializedProperty canvasMatrix;

    private void OnEnable()
    {
        colorFillingSetting = target as ColorFillingSetting;
        canvasMatrix = serializedObject.FindProperty("isSpace");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Color Canvas");

        serializedObject.Update();

        for (int i = 0; i < colorFillingSetting.sizeY; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < colorFillingSetting.sizeX; j++)
            {
                canvasMatrix.GetArrayElementAtIndex(i * canvasSize + j).boolValue =
                    EditorGUILayout.Toggle(canvasMatrix.GetArrayElementAtIndex(i * canvasSize + j).boolValue, GUILayout.Width(20f));
            }
            EditorGUILayout.EndHorizontal();
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}
