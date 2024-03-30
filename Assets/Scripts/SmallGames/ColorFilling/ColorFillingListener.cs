using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ColorFillingListener : MessageGivingManager
{
    private ColorFilling colorFilling;
    public GameObject[] ColorFillButtons;
    

    /// ----------------------monobehavior call-------------------
    private void OnEnable()
    {
        colorFilling = GetComponent<ColorFilling>();
    }

    /// ---------------------small game management----------------
    public override void WakeUpSmallGames()
    {
        base.WakeUpSmallGames();

        mainDisplay.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", MainDisplayTexture);
        mainDisplay.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", MainDisplayTexture);
        secondDisplay.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", SecondaryDisplayTexture);
        secondDisplay.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", SecondaryDisplayTexture);

        foreach (var button in ColorFillButtons)
        {
            button.GetComponent<BoxCollider>().enabled = true;
        }

        colorFilling.enabled = true;
    }

    public override bool GiveMessage()
    { 
        base.GiveMessage();
        //Debug.Log(gameIndex + "/" + messagesToGive.Count);
        
        colorFilling.enabled = false;
        return true;
    }

    public override void QuitSmallGame()
    {
        base.QuitSmallGame();

        mainDisplay.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", turnOffTexture);
        mainDisplay.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", turnOffTexture);
        secondDisplay.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", blackTexture);
        secondDisplay.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", blackTexture);

        foreach (var button in ColorFillButtons)
        {
            button.GetComponent<BoxCollider>().enabled = false;
        }
        if (gameIndex >= messagesToGive.Count)
        {
            //Debug.Log("Disable");
            box2.GetComponent<BoxCollider>().enabled = false;
            box2.GetComponent<Item3DInteractive>().enabled = false;
            foreach (var button in ColorFillButtons)
            {
                button.GetComponent<BoxCollider>().enabled = false;
            }
        }

        colorFilling.DisposeGame();
        colorFilling.enabled = false;
    }
}
