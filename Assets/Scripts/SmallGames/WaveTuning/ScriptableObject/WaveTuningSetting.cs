using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveState
{ superLow, low, averange, high, superHigh}

public class WaveTuningSetting : SmallGameSetting
{
    public WaveState a, w, fi;

    public WaveState init_a, init_w, init_fi;

    public Vector2 noisePointPosition;
    public float timeLimit = 60f;
}
