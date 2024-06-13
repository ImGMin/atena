using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public GameData gameData;

    private string dataPath;

    public delegate void func();

    public List<func> fList;

    public bool Tutorial = false;

    public int earnings = 0;

    public List<string> NumToSitu = new List<string> { "Situ_Idle"};
    public Dictionary<string,int> SituToNum = new Dictionary<string, int> { { "Situ_Idle", 0 } };

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            dataPath = Path.Combine(Application.persistentDataPath, "gameData.json");
            LoadGameData();
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
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

    public void SaveGameData()
    {
        string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(dataPath, jsonData);
    }

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

    public void InitGameData()
    {
        gameData = new GameData();
        SaveGameData();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
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

    public void ChangeValue(int? exp = null, int? energy = null, int? friends = null, int? cash = null, int? reputation = null, int? atenaGrowth = null, int? curTime = null)
    {
        bool flag = false;

        if (exp.HasValue)
        {
            gameData.exp += exp.Value;
            LevelUp();
            gameData.ChangeLvExp();
            flag = true;
        }

        if (energy.HasValue)
        {
            gameData.energy += energy.Value;
            if (gameData.energy < 0)
            {
                gameData.energy = 0;
            }
            if (gameData.energy > 1000)
            {
                gameData.energy = 1000;
            }

            gameData.ChangeEnergy();
            flag = true;
        }

        if (friends.HasValue)
        {
            gameData.friends += friends.Value;
            if (gameData.friends < 0)
            {
                gameData.friends = 0;
            }
            if (gameData.friends > 150)
            {
                gameData.friends = 150;
            }

            gameData.ChangeFriend();
            flag = true;
        }

        if (cash.HasValue)
        {
            gameData.cash += cash.Value;
            if (gameData.cash < 0)
            {
                gameData.cash = 0;
            }

            gameData.ChangeCash();
            flag = true;
        }

        if (reputation.HasValue)
        {
            gameData.reputation = reputation.Value;
            if ( gameData.reputation < -99)
            {
                gameData.reputation = -99;
            }
            if (gameData.reputation > 99)
            {
                gameData.reputation = 99;
            }

            gameData.ChangeReputation();
            flag = true;
        }

        if (atenaGrowth.HasValue)
        {
            gameData.atenaGrowth += atenaGrowth.Value;
            if (gameData.atenaGrowth < 0)
            {
                gameData.atenaGrowth = 0;
            }

            gameData.ChangeAtenaGrowth();
            flag = true;
        }

        if (curTime.HasValue)
        {
            gameData.curTime.day += curTime.Value;
            gameData.curTime.UpdateDate();
            gameData.ChangeAtenaDate();
            flag = true;
        }

        if (flag) 
        {
            SaveGameData(); 
        }
    }

    
}
