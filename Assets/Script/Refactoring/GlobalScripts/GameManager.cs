using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEditor.SceneManagement;
using System;
using Unity.Collections.LowLevel.Unsafe;

public class GameManager : MonoBehaviour
{
    public GameData gameData = new GameData();
    public AtenaDate atenaDate = new AtenaDate();

    public Dictionary<string, IIndexer<object>> tables = new Dictionary<string, IIndexer<object>>();

    private string[] gameDataName = new string[] { "level", "exp", "energy", "friends", "cash", "reputation", "atenaGrowth", "favor" };

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindAnyObjectByType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
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

        LoadGameData();
    }

    // Start is called before the first frame update
    void Start()
    {
        //InitGameData();
        //LoadGameData();
        //SaveGameData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitGameData()
    {
        string filePath = Application.persistentDataPath + "/test.db";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("파일 제거 완료");
        }
        return;
    }

    void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/test.db";

        if (!File.Exists(filePath))
        {
            string streamingFilePath = Application.streamingAssetsPath + "/test.db";
            File.Copy(streamingFilePath, filePath);

            Debug.Log("connect new database at " + streamingFilePath);
        }

        using (var connection = new SqliteConnection("URI=file:" + filePath))
        {
            connection.Open();

            string[] nameList = new string[] { "GameData", "AtenaDate" };
            IIndexer<object>[] dataList = new IIndexer<object>[] {gameData, atenaDate};

            using (var command = connection.CreateCommand())
            {
                for (int i = 0; i < nameList.Length; i++)
                {
                    string name = nameList[i];
                    IIndexer<object> data = dataList[i];

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
                            
                            idx++;
                        }
                    }
                }
            }
        }
    }

    void SaveGameData()
    {
        string filePath = Application.persistentDataPath + "/test.db";

        using (var connection = new SqliteConnection("URI=file:" + filePath))
        {
            connection.Open();

            string[] nameList = new string[] { "GameData", "AtenaDate" };
            IIndexer<object>[] dataList = new IIndexer<object>[] { gameData, atenaDate };

            using (var command = connection.CreateCommand())
            {
                for (int i = 0; i < nameList.Length; i++)
                {
                    string name = nameList[i];
                    IIndexer<object> data = dataList[i];

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
    }
}



public interface IIndexer<T>
{
    T this[int index] { get; set; }
}
