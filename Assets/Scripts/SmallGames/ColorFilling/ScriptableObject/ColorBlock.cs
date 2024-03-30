using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// color block utility
public class ColorBlock : ScriptableObject
{
    public int sizeX, sizeY;
    public ColorName blockColor;
    
    [HideInInspector]
    public bool[] isBlock = new bool[400];

}