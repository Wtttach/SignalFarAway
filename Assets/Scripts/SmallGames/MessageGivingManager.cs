using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageGivingManager : MonoBehaviour
{
    private bool isGameOn = false;

    public int gameIndex = 0;
    public List<Message> messagesToGive;
    public List<SmallGameSetting> gameSettings;

    public GameObject exitButton;

    public GameObject mainDisplay, secondDisplay;
    public RenderTexture MainDisplayTexture;
    public RenderTexture SecondaryDisplayTexture;
    public Texture2D turnOffTexture;
    public Texture2D blackTexture;

    public GameObject box1, box2;

    public GameObject resultSuccess, resultFailure;

    public Vector3 originalPos, targetPos;
    public Vector3 originalRot, targetRot;
    public float moveSpeed;

    public AudioSource audioSource;
    public AudioClip failClip;
    public AudioClip repairClip;

    public GameObject messageCanvas;
    public GameObject notice;

    public virtual void WakeUpSmallGames()
    {
        isGameOn = true;
        messageCanvas.SetActive(false);

        exitButton.GetComponent<BoxCollider>().enabled = true;
        box1.GetComponent<BoxCollider>().enabled = false;
        box2.GetComponent<BoxCollider>().enabled = false;
        originalPos = Camera.main.transform.position;
        originalRot = Camera.main.transform.eulerAngles;
        Camera.main.GetComponent<CameraMove>().enabled = false;

        StartCoroutine(CameraMoveBack(MoveState.ZoomIn));
    }

    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);

        notice.SetActive(false);
    }

    /// <summary>
    /// Give message and switch to the next minigame.
    /// Can be overrided to set minigame quit
    /// </summary>
    public virtual bool GiveMessage()
    {
        if (gameIndex >= messagesToGive.Count)
        {
            return false;
        }
        audioSource.clip = repairClip;
        audioSource.Play();
        notice.SetActive(true);
        StartCoroutine(WaitTime(3f));
        messagesToGive[gameIndex].RepairLostMessage();
        gameIndex++;
        return true;
    }

    public SmallGameSetting GetGameSetting()
    {
        return gameSettings[gameIndex];
    }

    public IEnumerator GameSuccessShow()
    {
        resultSuccess.SetActive(true);

        yield return new WaitForSeconds(2f);

        
        QuitSmallGame();

        resultSuccess.SetActive(false);
    }

    public IEnumerator GameFailShow()
    {
        resultFailure.SetActive(true);
        yield return new WaitForSeconds(2f);
        
        QuitSmallGame();
        resultFailure.SetActive(false);
        
    }

    public void GameResult(bool success = false)
    {
        if (success)
        {
            StartCoroutine(GameSuccessShow());
        }
        else 
        {
            audioSource.clip = failClip;
            audioSource.Play();
            StartCoroutine(GameFailShow()); 
        }
    }

    public virtual void QuitSmallGame()
    {
        if (!isGameOn) return;

        isGameOn = false;

        StartCoroutine(CameraMoveBack(MoveState.ZoomOut));
    }

    public enum MoveState { ZoomIn, ZoomOut }

    IEnumerator CameraMoveBack(MoveState moveState)
    {
        Vector3 tPos, tRot;
        if (moveState == MoveState.ZoomIn)
        {
            tPos = targetPos;
            tRot = targetRot;
        }
        else
        {
            tPos = originalPos;
            tRot = originalRot;
        }


        while ((Camera.main.transform.position - tPos).magnitude > 0.001f)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, tPos, moveSpeed * 0.3f);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.LookRotation(mainDisplay.transform.position - Camera.main.transform.position), 0.3f);
            yield return null;
        }

        if (moveState == MoveState.ZoomOut)
        {
            exitButton.GetComponent<BoxCollider>().enabled = false;
            box1.GetComponent<BoxCollider>().enabled = true;
            box2.GetComponent<BoxCollider>().enabled = true;
            Camera.main.GetComponent<CameraMove>().enabled = true;
            messageCanvas.SetActive(true);
        }
    }
}
