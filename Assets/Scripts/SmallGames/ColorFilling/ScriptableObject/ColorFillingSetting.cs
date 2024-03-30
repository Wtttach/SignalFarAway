using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorName { white = 0, red = 1, orange = 2, yellow = 3, green = 4, blue = 5, purple = 6 };

/// color filling game setting
public class ColorFillingSetting : SmallGameSetting
{
    public int sizeX, sizeY;
    public int[] colorRequest = new int[6];
    [HideInInspector]
    public bool[] isSpace = new bool[400];


    public List<ColorBlock> blocksData;
}