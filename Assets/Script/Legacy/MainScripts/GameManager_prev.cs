using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class GameManager_prev : MonoBehaviour
{
    private static GameManager_prev instance;
    public GameData_prev gameData;

    private string dataPath;

    public delegate void func();

    public List<func> fList;

    public bool Tutorial = false;

    public int earnings = 0;

    public List<string> NumToSitu = new List<string> { "Situ_Idle"};
    public Dictionary<string,int> SituToNum = new Dictionary<string, int> { { "Situ_Idle", 0 } };

    public static GameManager_prev Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager_prev>();
                if (instance == null)
                {
                    instance = new GameObject("GameManager_prev").AddComponent<GameManager_prev>();
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
            GenSitu();
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

    private void GenSitu()
    {
        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        TextAsset csvData = Resources.Load<TextAsset>("test_date");
        // 파일 경로가 유효한지 확인
        if (csvData != null)
        {
            // CSV 파일의 모든 줄을 읽기
            string[] lines = csvData.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            if (lines.Length > 0)
            {
                // 첫 번째 줄은 헤더로 사용
                string[] headers = lines[0].Split(',');

                // 각 행의 데이터를 딕셔너리에 저장
                for (int i = 1; i < lines.Length; i++)
                {
                    if (!string.IsNullOrEmpty(lines[i]))
                    {
                        string[] fields = lines[i].Split(',');

                        Dictionary<string, string> entry = new Dictionary<string, string>();

                        for (int j = 0; j < headers.Length; j++)
                        {
                            entry[headers[j]] = fields[j];
                        }

                        data.Add(entry);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다");
            return;
        }

        for (int i = 0; i < data.Count; i++)
        {
            string str = data[i]["상황ID"];

            if (!Instance.SituToNum.ContainsKey(str))
            {
                Instance.SituToNum[str] = Instance.SituToNum.Count;
                Instance.NumToSitu.Add(str);
            }

        }
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
            gameData = JsonConvert.DeserializeObject<GameData_prev>(jsonData);
        }
        else
        {
            gameData = new GameData_prev();
        }


    }

    public void InitGameData()
    {
        gameData = new GameData_prev();
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
