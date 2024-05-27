using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public GameData gameData;

    private string dataPath;

    // 싱글턴
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    //싱글턴 인스턴스 초기화
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 로딩 시 인스턴스 유지
            dataPath = Path.Combine(Application.persistentDataPath, "gameData.json");
            LoadGameData();
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }

    //저장
    public void SaveGameData()
    {
        string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(dataPath, jsonData);
    }

    //불러오기
    public void LoadGameData()
    {
        if (File.Exists(dataPath))
        {
            string jsonData = File.ReadAllText(dataPath);
            GameData gameData = JsonConvert.DeserializeObject<GameData>(jsonData);
        }
        else
        {
            gameData = new GameData();
        }
    }
    
    public void expUp(int num)
    {
        gameData.exp += num;
        while (gameData.exp < gameData.LvUpEXP[gameData.level])
        {
            gameData.exp -= gameData.LvUpEXP[gameData.level];
            gameData.level++;
        }
        return;
    }
}
