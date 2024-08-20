using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

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
        //LoadGameData();
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

            using (var command = connection.CreateCommand())
            {
                foreach (string name in nameList)
                {
                    command.CommandText = $"SELECT * FROM {name}";
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Debug.Log("Name: " + reader["name"] + ", Value: " + reader["value"] + ", Type: " + reader["type"]);
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
