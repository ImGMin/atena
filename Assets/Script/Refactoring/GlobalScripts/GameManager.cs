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
    //GameData
    public GameData gameData = new GameData();
    public AtenaDate atenaDate = new AtenaDate();

    //WeekData
    public SituData situData = new SituData();
    public WorkData workData = new WorkData();
    public PayData payData = new PayData();

    //JsonData
    public ArrayData<bool> isEventEntryDay = new ArrayData<bool>(5);
    public ArrayData<int> isEventDay = new ArrayData<int>(5);

    public Dictionary<string, List<(string,IIndexer<object>)>> tables = new Dictionary<string, List<(string, IIndexer<object>)>>();

    private string[] dbNameList = new string[] { "GameData", "WeekData" };

    private string[] gameDataName = new string[] { "level", "exp", "energy", "friends", "cash", "reputation", "atenaGrowth", "favor" };

    private string[] filaPathList = new string[] { "EventEntry", "EventDay" };

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

        isEventEntryDay = InitArrayData<bool>(filaPathList[0]);
        isEventDay = InitArrayData<int>(filaPathList[1]);
    }
    void LoadAllData()
    {
        foreach (string dbName in dbNameList)
        {
            LoadGameData(dbName);
        }

        isEventEntryDay = LoadArrayData<bool>(filaPathList[0]);
        isEventDay = LoadArrayData<int>(filaPathList[1]);
    }
    public void SaveAllData()
    {
        foreach (string dbName in dbNameList)
        {
            SaveGameData(dbName);
        }

        SaveArrayData(isEventEntryDay, filaPathList[0]);
        SaveArrayData(isEventDay, filaPathList[1]);
    }

    void InitGameData(string dbName)
    {
        string filePath = Application.persistentDataPath + $"/{dbName}.db";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
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

    public void SaveArrayData<T>(ArrayData<T> data, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public ArrayData<T> LoadArrayData<T>(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ArrayData<T> data = JsonUtility.FromJson<ArrayData<T>>(json);
            return data;
        }
        else
        {
            ArrayData<T> data = new ArrayData<T>(5);
            for (int i = 0; i < 5; i++)
            {
                Debug.Log(data.array[i]);
            }
            return data;
        }
    }

    public ArrayData<T> InitArrayData<T>(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("제거");
        }

        return LoadArrayData<T>(fileName);
    }
}
