using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveTuning : MonoBehaviour
{
    WaveTuningListener listener;

    /// ---------------------Visual Params----------------------
    public float boundry;
    public float moveSpeed;

    /// ---------------------Game Data--------------------------
    const int waveLevels = 5;
    private float[] levelParams = { 1, 1.4f, 1.8f, 2.2f, 2.6f };
    private float[] wParams = { 1f, 1.2f, 1.4f, 1.6f, 1.8f };
    private int current_a, current_w, current_fi;
    private WaveTuningSetting gameSetting;
    private float timeLimit;

    /// ---------------------Game Instance----------------------
    public GameObject currentWave, aimWave;
    private GameObject currentWavePic_1, currentWavePic_2;
    public GameObject noisePoint;
    public TextMeshProUGUI timerText;

    public GameObject aKnob, wKnob, fiKnob;

    #region MonoBehaviourCalls
    private void Awake()
    {
        listener = GetComponent<WaveTuningListener>();
        if (listener == null) 
            Debug.LogError("Wave Tuning (Small Game) listener couldn't be found!");

        currentWavePic_1 = currentWave.transform.GetChild(0).gameObject;
        currentWavePic_2 = currentWave.transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        InitializeGame();
    }

    private void Update()
    {
        /// Game Wave Moving
        currentWavePic_1.transform.localPosition += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        currentWavePic_2.transform.localPosition += new Vector3(moveSpeed * Time.deltaTime, 0, 0);


        if (currentWavePic_1.transform.localPosition.x > 3.32f)
        {
            currentWavePic_1.transform.localPosition = new Vector3((currentWavePic_2.transform.localPosition.x - boundry), currentWavePic_1.transform.localPosition.y, currentWavePic_1.transform.localPosition.z); 
        }

        if (currentWavePic_2.transform.localPosition.x > 3.32f)
        {
            currentWavePic_2.transform.localPosition = new Vector3((currentWavePic_1.transform.localPosition.x - boundry), currentWavePic_2.transform.localPosition.y, currentWavePic_2.transform.localPosition.z);
        }

        timeLimit -= Time.deltaTime;
        timerText.text = "Time Left:" + string.Format("{0:f2}", timeLimit);

        if (timeLimit <= 0) GameFail();
    }
    #endregion

    #region Initialize
    /// <summary>
    /// Initialize the game
    /// </summary>
    private void InitializeGame()
    {
        gameSetting = listener.GetGameSetting() as WaveTuningSetting;

        current_a = (int)gameSetting.init_a;
        current_w = (int)gameSetting.init_w;
        current_fi = (int)gameSetting.init_fi;

        currentWave.SetActive(true);
        aimWave.SetActive(true);

        currentWave.transform.localScale = new Vector3(levelParams[current_w],levelParams[current_a],1);
        aimWave.transform.localScale = new Vector3(levelParams[(int)gameSetting.w],levelParams[(int)gameSetting.a],1);

        noisePoint.transform.localPosition = new Vector3(-0.4f + 6.8f * gameSetting.noisePointPosition.x, -4.5f + 5f * gameSetting.noisePointPosition.y, 0);

        timeLimit = gameSetting.timeLimit;
    }

    #endregion

    #region Dispose
    /// <summary>
    /// Dispose the gameobjects when game is finished
    /// </summary>
    public void DisposeGame()
    {
        currentWave.transform.localScale = Vector3.zero;
        aimWave.transform.localScale = Vector3.zero;

        currentWave.SetActive(false);
        aimWave.SetActive(false);
    }
    #endregion

    #region APIs
    /// <summary>
    /// Use a, w, fi params to set wave animation
    /// </summary>
    private void SetWaveAnim(int a, int w, int fi)
    {
        currentWave.transform.localScale = new Vector3(wParams[w], levelParams[a], 1);
    }

    /// <summary>
    /// Check if the param if out of the levels
    /// </summary>
    private int CheckParam(int param)
    {
        if (param < 0) return (param + waveLevels);
        if (param >= waveLevels) return (param - waveLevels);

        return param;
    }

    private IEnumerator RotateKnob(GameObject knob, int level)
    {
        Vector3 originalRotation = knob.transform.eulerAngles;
        Vector3 targetRotation = new Vector3(0, 0, 360 - level * 20);
        if (targetRotation.z == 360) targetRotation.z = 0;

        while ((knob.transform.eulerAngles - targetRotation).magnitude > 0.01f)
        {
            knob.transform.eulerAngles = Vector3.Lerp(knob.transform.eulerAngles, targetRotation, 20);
            yield return null;
        }
    }

    /// <summary>
    /// Change a param of the wave for button a change.
    /// </summary>
    public void ChangeWaveA()
    {
        current_a++;
        current_a = CheckParam(current_a);

        StartCoroutine(RotateKnob(aKnob, current_a));

        
        SetWaveAnim(current_a, current_w, current_fi);
        CheckGameSuccess();
    }

    /// <summary>
    /// Change w param of the wave for button w change.
    /// </summary>
    public void ChangeWaveW()
    {
        
        current_w ++;
        current_w = CheckParam(current_w);

        StartCoroutine(RotateKnob(wKnob, current_w));

        
        SetWaveAnim(current_a, current_w, current_fi);
        CheckGameSuccess();
    }

    /// <summary>
    /// Change fi param of the wave for button fi change.
    /// </summary>
    public void ChangeWaveFi()
    {
        current_fi ++;
        current_fi = CheckParam(current_fi);

        StartCoroutine(RotateKnob(fiKnob, current_fi));

        
        SetWaveAnim(current_a, current_w, current_fi);
        CheckGameSuccess();
    }
    #endregion

    #region Callbacks
    /// <summary>
    /// Check if the game is finished
    /// </summary>
    private bool CheckGameSuccess()
    {
        if (current_a == (int)gameSetting.a && current_w == (int)gameSetting.w) 
        {
            Debug.Log("Game Finished");
            listener.GiveMessage();
            listener.GameResult(true);
            return true;
        }
        else return false;
    }
    /// <summary>
    /// if it touches the noise point, game fails
    /// </summary>
    public void GameFail()
    {
        Debug.Log("fail");
        this.enabled = false;
        listener.GameResult(false);
    }
    #endregion
}