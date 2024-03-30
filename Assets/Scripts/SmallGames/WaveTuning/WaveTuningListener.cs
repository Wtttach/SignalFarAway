using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[DisallowMultipleComponent]
public class WaveTuningListener : MessageGivingManager
{
    private WaveTuning waveTuning;
    public GameObject[] waveTuneKnob;

    #region MonoBehaviourCalls
    private void OnEnable()
    {
        waveTuning = GetComponent<WaveTuning>();
    }
    #endregion

    #region APIs
    public override void WakeUpSmallGames()
    {
        base.WakeUpSmallGames();

        mainDisplay.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", MainDisplayTexture);
        mainDisplay.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", MainDisplayTexture);
        secondDisplay.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", SecondaryDisplayTexture);
        secondDisplay.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", SecondaryDisplayTexture);

        foreach (var knob in waveTuneKnob)
        {
            knob.GetComponent<BoxCollider>().enabled = true;
        }

        waveTuning.enabled = true;
    }

    public override bool GiveMessage()
    {
        base.GiveMessage();
        waveTuning.enabled = false;

        return true;
    }

    public override void QuitSmallGame()
    {
        waveTuning.enabled = false;
        waveTuning.DisposeGame();

        mainDisplay.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", turnOffTexture);
        mainDisplay.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", turnOffTexture);
        secondDisplay.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", blackTexture);
        secondDisplay.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", blackTexture);

        foreach (var knob in waveTuneKnob)
        {
            knob.GetComponent<Item3DKnob>().ResetKnob();
            knob.GetComponent<BoxCollider>().enabled = false;
        }
        
        if (gameIndex >= messagesToGive.Count)
        {
            box1.GetComponent<BoxCollider>().enabled = false;
            box1.GetComponent<Item3DInteractive>().enabled = false;
            foreach (var button in waveTuneKnob)
            {
                button.GetComponent<BoxCollider>().enabled = false;
            }
        }
        base.QuitSmallGame();
    }
    #endregion
}
