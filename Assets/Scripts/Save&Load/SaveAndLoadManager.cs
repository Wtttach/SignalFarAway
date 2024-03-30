using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager Instance { get; private set; }

    [Serializable]
    public class SaveData
    {
        public string playerInfo;
        public List<string> levelResults;
        public int currentLevel;

        public SaveData()
        {
            playerInfo = System.DateTime.Now.ToString();
            levelResults = new List<string>();
            levelResults.Add("1");
            levelResults.Add("2");
            levelResults.Add("3");
            levelResults.Add("4");
            currentLevel = 0;
        }
    }

    public SaveData playerData = new SaveData();
    

    private void Awake()
    {
        if (Instance == null)
        {
            playerData = new SaveData();
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SaveGame()
    {
        playerData.playerInfo = System.DateTime.Now.ToString();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (!Directory.Exists(Application.streamingAssetsPath + "/Saves"))
        {
            Debug.Log("Create");
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Saves");
        }
        FileStream fileStream = File.Create(Application.streamingAssetsPath + "/Saves" + "/playerSave.save");
        if (fileStream == null) Debug.Log("Failed");
        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.streamingAssetsPath + "/Saves" + "/playerSave.save"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.streamingAssetsPath + "/Saves" + "/playerSave.save", FileMode.Open);
            SaveData saveData = (SaveData) binaryFormatter.Deserialize(fileStream);
            playerData = saveData;
            fileStream.Close();

            GameObject.Find("SceneChangeManager").GetComponent<SceneChange>().ChangeToSceneIndex(playerData.currentLevel);
        }
    }
}
