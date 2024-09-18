using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public GameData gameData = new GameData();
    public AtenaDate atenaDate = new AtenaDate();

    public SituData situData = new SituData();
    public WorkData workData = new WorkData();
    public PayData payData = new PayData();

    public Dictionary<string, List<(string,IIndexer<object>)>> tables = new Dictionary<string, List<(string, IIndexer<object>)>>();

    private string[] dbNameList = new string[] { "GameData", "WeekData" };

    private string[] gameDataName = new string[] { "level", "exp", "energy", "friends", "cash", "reputation", "atenaGrowth", "favor" };

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(GameManager)) as GameManager;

                if (_instance == null) {
                    if (applicationIsQuitting)
                    {
                        return null;
                    }

                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString();
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private static bool applicationIsQuitting = false;
    public void OnDestroy()
    {
        applicationIsQuitting = true;
        gameData.EventDiscard();
    }

    private void Awake()
    {
        GenTableList();

        applicationIsQuitting = false;
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        LoadAllData();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenTableList()
    {
        tables.Add("GameData", new List<(string, IIndexer<object>)> 
        { 
            ("GameData", gameData), 
            ("AtenaDate", atenaDate) }
        );

        tables.Add("WeekData", new List<(string, IIndexer<object>)> 
        { 
            ("SituData", situData), 
            ("WorkData", workData), 
            ("PayData", payData) 
        });
    }

    public void InitAllData()
    {
        foreach (string dbName in dbNameList)
        {
            InitGameData(dbName);
        }
    }
    void LoadAllData()
    {
        foreach (string dbName in dbNameList)
        {
            LoadGameData(dbName);
        }
    }
    public void SaveAllData()
    {
        foreach (string dbName in dbNameList)
        {
            SaveGameData(dbName);
        }
    }

    void InitGameData(string dbName)
    {
        string filePath = Application.persistentDataPath + $"/{dbName}.db";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("파일 제거 완료");
        }

        LoadGameData(dbName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        return;
    }

    void LoadGameData(string dbName)
    {
        string filePath = Application.persistentDataPath + $"/{dbName}.db";

        if (!File.Exists(filePath))
        {
            string streamingFilePath = Application.streamingAssetsPath + $"/{dbName}.db";
            File.Copy(streamingFilePath, filePath);

            Debug.Log("connect new database at " + streamingFilePath);
        }

        using (var connection = new SqliteConnection("URI=file:" + filePath))
        {
            connection.Open();

            List<(string, IIndexer<object>)> tableList = tables[dbName];

            using (var command = connection.CreateCommand())
            {
                for (int i = 0; i < tableList.Count; i++)
                {
                    string name = tableList[i].Item1;
                    IIndexer<object> data = tableList[i].Item2;

                    command.CommandText = $"SELECT * FROM {name}";
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        int idx = 0;
                        while (reader.Read())
                        {
                            //Debug.Log("Name: " + reader["name"] + ", Value: " + reader["value"] + ", Type: " + reader["type"]);
                            switch (reader["type"])
                            {
                                case "int":
                                    data[idx] = Convert.ToInt32(reader["value"]);
                                    break;

                                case "float":
                                    data[idx] = Convert.ToSingle(reader["value"]);
                                    break;

                                case "string":
                                    data[idx] = Convert.ToString(reader["value"]);
                                    break;

                                default:
                                    Debug.Log("typeError");
                                    break;
                            }
                            //Debug.Log($"{data[idx]}");
                            idx++;
                        }
                    }
                }
            }
        }
    }

    public void SaveGameData(string dbName)
    {
        string filePath = Application.persistentDataPath + $"/{dbName}.db";

        if (!File.Exists(filePath))
        {
            LoadGameData(dbName);
        }

        using (var connection = new SqliteConnection("URI=file:" + filePath))
        {
            connection.Open();

            List<(string, IIndexer<object>)> tableList = tables[dbName];

            using (var command = connection.CreateCommand())
            {
                for (int i = 0; i < tableList.Count; i++)
                {
                    string name = tableList[i].Item1;
                    IIndexer<object> data = tableList[i].Item2;

                    command.CommandText = $"SELECT * FROM {name}";
                    using (var reader = command.ExecuteReader())
                    {
                        int idx = 0;
                        while (reader.Read())
                        {
                            // 고유 식별자 (예: ID) 및 현재 value 값을 읽어옴
                            string rowName = reader.GetString(reader.GetOrdinal("name")); // 'name' 칼럼의 값

                            // 새로운 value 값을 계산 또는 설정 
                            string newValue = data[idx].ToString();

                            // 행 업데이트를 위한 SQL 명령어
                            using (var updateCommand = connection.CreateCommand())
                            {
                                updateCommand.CommandText = $"UPDATE {name} SET value = @newValue WHERE name = @rowName";
                                updateCommand.Parameters.Add(new SqliteParameter("@newValue", newValue));
                                updateCommand.Parameters.Add(new SqliteParameter("@rowName", rowName));
                                updateCommand.ExecuteNonQuery();
                            }

                            idx++;
                        }
                    }
                }
            }
        }

        Debug.Log("done");
    }

    public void ChangeValue(string name, int value)
    {
        int idx = -1;
        for (int i = 0; i < gameDataName.Length; i++)
        {
            if (gameDataName[i] == name)
            {
                idx = i;
                break;
            }
        }

        if (idx == -1)
        {
            Debug.Assert(false);
            return;
        }

        gameData[idx] = (int)gameData[idx] + value;
        gameData.ChangeValue(idx);

        if (name == "exp") gameData.LvUp();

        SaveGameData("GameData");
    }

    public void ChangeValue(int idx, int value)
    {
        if (idx == -1)
        {
            Debug.Assert(false);
            return;
        }

        gameData[idx] = (int)gameData[idx] + value;
        gameData.ChangeValue(idx);

        if (idx == 1) gameData.LvUp();

        SaveGameData("GameData");
    }
}
