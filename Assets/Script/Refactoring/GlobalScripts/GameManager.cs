using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEditor.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public GameData gameData = new GameData();
    public AtenaDate atenaDate = new AtenaDate();

    public Dictionary<string, IIndexer<object>> tables = new Dictionary<string, IIndexer<object>>();


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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitGameData();
        LoadGameData();
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
                            Debug.Log("Name: " + reader["name"] + ", Value: " + reader["value"] + ", Type: " + reader["type"]);
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
}



public interface IIndexer<T>
{
    T this[int index] { get; set; }
}
