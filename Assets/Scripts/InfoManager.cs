using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public static InfoManager Instance;

    public string PlayerName {
        get;
        private set;
    }

    public int HighScore
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadInfo();
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highScore;
    }

    public void SaveInfo()
    {
        SaveData data = new();
        data.playerName = PlayerName;
        data.highScore = HighScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            PlayerName = data.playerName;
            HighScore = data.highScore;
        }
    }

    public string ShowInfo()
    {
        if (HighScore > 0)
        {
            return (string.IsNullOrEmpty(PlayerName) ? "[Guest]" : PlayerName) + " has the high score: " + HighScore;
        } else
        {
            return "No High Score";
        }
    }

    public void SetPlayerName(string newPlayerName)
    {
        PlayerName = newPlayerName;
        SaveInfo();
    }

    public void SetHighScore(int newHighScore, string newPlayerName = null)
    {
        if (newPlayerName != null)
        {
            PlayerName = newPlayerName;
        }

        HighScore = newHighScore;
        SaveInfo();
    }

}
