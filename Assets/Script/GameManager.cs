using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Xml.Linq;
using Unity.VisualScripting;

public class GameData
{
    public int level {  get; set; }
    public int exp {  get; set; }
    public int energy { get; set; }
    public int friends { get; set; }
    public int cash { get; set; }
    public int reputation { get; set; }
    public int atenaGrowth { get; set; }
    public AtenaDate curTime { get; set; }

    public GameData() {
        level = 0;
        exp = 0;
        energy = 100;
        friends = 0;
        cash = 0;
        reputation = 0;
        atenaGrowth = 0;
        curTime = new AtenaDate(2025, 1, 1);
    }

    public override string ToString()
    {
        return $"level: {level}\n exp: {exp}\n level: {level}\n level: {level}\n level: {level}\n ";
    }
}

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

    //아르바이트
    public void PlayGame()
    {
        Debug.Log("Game is starting...");
    }
}
