using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoisePoint : MonoBehaviour
{
    WaveTuning waveTuning;
    // Start is called before the first frame update
    void Start()
    {
        waveTuning = GetComponentInParent<WaveTuning>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaveGame")
        {
            waveTuning.GameFail();
        }
    }
}
