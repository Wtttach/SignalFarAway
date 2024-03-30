using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public SaveAndLoadManager.SaveData saveData;
    public float moveSpeed;

    public GameObject s1_1, s1_2;
    public GameObject s2_1, s2_2;
    public GameObject s3_1, s3_2;
    public GameObject s4_1, s4_2;
    public GameObject s5_1, s5_2;
    public GameObject s6_1, s6_2;
    public GameObject s7_1, s7_2;

    public float timer;
    public SceneChange sceneChange;

    void Start()
    {
        saveData = GameObject.Find("Save&LoadManager").GetComponent<SaveAndLoadManager>().playerData;

        if (saveData != null)
        {
            bool isC = false, isL = false;
            bool isE = false, isM = false;
            for (int i = 0; i < saveData.levelResults[0].Length; i++)
            {
                if (saveData.levelResults[0][i] == 'C') { isC = true; continue; }
                if (saveData.levelResults[0][i] == 'L') { isL = true; continue; }
                if (saveData.levelResults[0][i] == 'E') { isE = true; continue; }
                if (saveData.levelResults[0][i] == 'M') { isM = true; continue; }
            }

            if (isC && !isL) { s1_1.SetActive(true); s1_2.SetActive(false); }
            else { s1_1.SetActive(false); s1_2.SetActive(true); }

            if (isE && !isM) { s2_1.SetActive(true); s2_2.SetActive(false); }
            else { s2_1.SetActive(false); s2_2.SetActive(true); }

            isC = false; isL = false;
            bool isJ = false;
            for (int i = 0; i < saveData.levelResults[2].Length; i++)
            {
                if (saveData.levelResults[2][i] == 'C') { isC = true; continue; }
                if (saveData.levelResults[2][i] == 'L') { isL = true; continue; }
                if (saveData.levelResults[2][i] == 'J') { isJ = true; continue; }
            }

            if (isC && !isL) { s4_1.SetActive(true); s4_2.SetActive(false); }
            else { s4_1.SetActive(false); s4_2.SetActive(true); }

            bool isD = false; isL = false;

            for (int i = 0; i < saveData.levelResults[2].Length; i++)
            {
                if (saveData.levelResults[1][i] == 'D') { isD = true; continue; }
                if (saveData.levelResults[1][i] == 'L') { isL = true; continue; }
            }

            if (isD && !isL) { s6_1.SetActive(true); s6_2.SetActive(false); }
            else { s6_1.SetActive(false); s6_2.SetActive(true); }

            bool orderCorrect_1 = false, orderCorrect_2 = false, orderCorrect_3 = false;
            if (saveData.levelResults[0] == "ABCDEFGHIJK" || saveData.levelResults[0] == "ABLDEFGHIJK" || saveData.levelResults[0] == "ABCDMFGHIJK" || saveData.levelResults[0] == "ABLDMFGHIJK")
            {
                orderCorrect_1 = true;
            }
            if (saveData.levelResults[2] == "ABCDEFGHIJK" || saveData.levelResults[2] == "ABIDEFGHJK" || saveData.levelResults[2] == "ABCDEFGHIMK" || saveData.levelResults[2] == "ABIDEFGHMK")
            {
                orderCorrect_3 = true;
            }
            if (saveData.levelResults[1] == "ABCDEFGHIJK" || saveData.levelResults[1] == "ABCLEFGHIJK")
            {
                orderCorrect_2 = true;
            }

            if (orderCorrect_1 && isJ) { s3_1.SetActive(true); s3_2.SetActive(false); }
            else { s3_1.SetActive(false); s3_2.SetActive(true); }
            if (orderCorrect_2) { s7_1.SetActive(true); s7_2.SetActive(false); }
            else { s7_1.SetActive(false); s7_2.SetActive(true); }
            if (orderCorrect_3) { s5_1.SetActive(true); s5_2.SetActive(false); }
            else { s5_1.SetActive(false); s5_2.SetActive(true); }
        }

        StartCoroutine(EndingTime());
    }

    IEnumerator EndingTime()
    {
        yield return new WaitForSeconds(timer);

        sceneChange.ChangeToSceneIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}
