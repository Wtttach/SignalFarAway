using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetsCallback
{
    [MenuItem("Assets/Create/Small Games/Color Filling", false, 10)]
    public static void CreateColorFilling()
    {
        var _setting = ScriptableObject.CreateInstance<ColorFillingSetting>();
        ProjectWindowUtil.CreateAsset(_setting, "Color Filling Setting.asset");
    }

    [MenuItem("Assets/Create/Small Games/Color Blocks")]
    public static void CreateColorBlock()
    {
        var _setting = ScriptableObject.CreateInstance<ColorBlock>();
        ProjectWindowUtil.CreateAsset(_setting, "Color Block.asset");
    }
    
    [MenuItem("Assets/Create/Small Games/Wave Tuning", false, 10)]
    public static void CreateWaveTuning()
    {
        var _setting = ScriptableObject.CreateInstance<WaveTuningSetting>();
        ProjectWindowUtil.CreateAsset(_setting, "Wave Tuning Setting.asset");
    }

    [MenuItem("Assets/Create/Dialogue Set", false, 1)]
    public static void CreateDialogueSet()
    {
        var _setting = ScriptableObject.CreateInstance<Dialogue>();
        ProjectWindowUtil.CreateAsset(_setting, "DialogueSet.asset");
    }
}
