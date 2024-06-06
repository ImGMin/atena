using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public GameData gameData;

    private string dataPath;

    public delegate void func();

    public List<func> fList;

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

    private void Start()
    {
        fList = new List<func>()
        {
            gameData.ChangeLvExp,
            gameData.ChangeEnergy,
            gameData.ChangeFriend,
            gameData.ChangeCash,
            gameData.ChangeReputation,
            gameData.ChangeAtenaGrowth,
            gameData.ChangeAtenaDate
        };
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
            gameData = JsonConvert.DeserializeObject<GameData>(jsonData);
        }
        else
        {
            gameData = new GameData();
        }
    }

    //초기화
    public void InitGameData()
    {
        gameData.exp = 0;
        gameData.level = 1;
        gameData.energy = 100;
        gameData.friends = 0;
        gameData.cash = 0;
        gameData.reputation = 0;
        gameData.atenaGrowth = 0;
        gameData.curTime = new AtenaDate(2025,1,1);
        
        foreach (var f in fList)
        {
            f();
        }
    }

    public void LevelUp()
    {
        while (gameData.exp >= gameData.LvUpEXP[gameData.level])
        {
            gameData.exp -= gameData.LvUpEXP[gameData.level];
            gameData.level++;
        }
        return;
    }
    public void ChangeValue(
    int? exp = null,
    int? energy = null,
    int? friends = null,
    int? cash = null,
    int? reputation = null,
    int? atenaGrowth = null,
    int? curTime = null
    )
    {
        if (exp.HasValue)
        {
            gameData.exp += exp.Value;
            LevelUp();
            gameData.ChangeLvExp();
        }

        if (energy.HasValue)
        {
            gameData.energy += energy.Value;
            gameData.ChangeEnergy();
        }

        if (friends.HasValue)
        {
            gameData.friends += friends.Value;
            gameData.ChangeFriend();
        }

        if (cash.HasValue)
        {
            gameData.cash += cash.Value;
            gameData.ChangeCash();
        }

        if (reputation.HasValue)
        {
            gameData.exp = reputation.Value;
            gameData.ChangeReputation();
        }

        if (atenaGrowth.HasValue)
        {
            gameData.atenaGrowth += atenaGrowth.Value;
            gameData.ChangeAtenaGrowth();
        }

        if (curTime.HasValue)
        {
            gameData.curTime.day += curTime.Value;
            gameData.curTime.UpdateDate();
            gameData.ChangeAtenaDate();
        }
    }
}
