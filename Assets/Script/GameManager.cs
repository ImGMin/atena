using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public GameData gameData;

    private string dataPath;

    // �̱���
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

    //�̱��� �ν��Ͻ� �ʱ�ȭ
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� �ε� �� �ν��Ͻ� ����
            dataPath = Path.Combine(Application.persistentDataPath, "gameData.json");
            LoadGameData();
        }
        else if (instance != this)
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� ����
        }
    }

    //����
    public void SaveGameData()
    {
        string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(dataPath, jsonData);
    }

    //�ҷ�����
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

    //�ʱ�ȭ
    public void InitGameData()
    {
        gameData.exp = 0;
        gameData.level = 1;
        gameData.ChangeLvExp();
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
    int? reputation = null,
    int? atenaGrowth = null
    )
    {
        if (exp.HasValue)
        {
            gameData.exp += exp.Value;
            LevelUp();
            gameData.ChangeLvExp();
        }

        /*if (energy.HasValue)
        {
            gameData.energy += energy.Value;
        }

        if (friends.HasValue)
        {
            gameData.friends += friends.Value;
        }

        if (reputation.HasValue)
        {
            gameData.exp = reputation.Value; 
        }

        if (atenaGrowth.HasValue)
        {
            gameData.atenaGrowth += atenaGrowth.Value;
        }*/
    }
}
