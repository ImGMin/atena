using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatterManager : MonoBehaviour
{
    //상황ID에 대해, (게시글세트ID, 선택지세트ID)가 든 리스트
    Dictionary<string, List<(string, string)>> SituIDData;

    //게시글세트ID에 대해, (작성자, 게시글)이 든 리스트
    Dictionary<string, List<(string, string)>> PostIDData;

    //선택지세트ID에 대해, {내용, 경험치, 평판, 친구수, 성장도, 에너지, 현금}에 대한 딕셔너리가 들어있는 리스트
    Dictionary<string, List<Dictionary<string, string>>> ChoiceIDData;

    int situNum;
    string situID;

    string postSetID;
    List<int> postIdx;
    public int numberOfPost = 4;

    public GameObject content;
    public GameObject postPrefab;

    string choiceSetID;

    private void Awake()
    {
        SituIDData = ReadSituIDCSV();
        PostIDData = ReadPostIDCSV();
        ChoiceIDData = ReadChoiceIDCSV();

        //오늘의 상황ID의 번호, ID
        situNum = GameManager.Instance.gameData.Schedule[GameManager.Instance.gameData.curTime.weekday].Item1;
        situID = GameManager.Instance.NumToSitu[situNum];

        //랜덤하게 게시글세트, 선택지세트의 ID 추출
        (postSetID, choiceSetID) = SituIDData[situID][Program.GetRandomIndices(SituIDData[situID].Count, 1)[0]];

        //포스트는 하루동안 고정
        postIdx = Program.GetRandomIndices(PostIDData[postSetID].Count,numberOfPost);
        GenTimeLine();
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    Dictionary<string,List<(string,string)>> ReadSituIDCSV()
    {
        Dictionary<string, List<(string, string)>> data = new Dictionary<string, List<(string, string)>>();

        TextAsset csvData = Resources.Load<TextAsset>("ChatterSituID");

        if (csvData == null)
        {
            Debug.Log("파일을 찾을 수 없습니다");
            return data;
        }

        string[] lines = csvData.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

        if (lines.Length == 0) 
        {
            Debug.Log("파일이 비었습니다");
            return data;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] fields = lines[i].Split(',');

                //상황ID
                string key = fields[1];

                //게시글세트ID, 선택지세트ID
                (string, string) val = (fields[2], fields[3]);

                if (!data.ContainsKey(key))
                {
                    data[key] = new List<(string, string)>();
                }

                data[key].Add(val);
            }
        }

        return data;
    }

    Dictionary<string, List<(string, string)>> ReadPostIDCSV()
    {
        Dictionary<string, List<(string, string)>> data = new Dictionary<string, List<(string, string)>>();

        TextAsset csvData = Resources.Load<TextAsset>("ChatterPostID");

        if (csvData == null) return data;

        string[] lines = csvData.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

        if (lines.Length == 0) return data;

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] fields = lines[i].Split(',');

                //게시글세트ID
                string key = fields[1];

                //작성자, 작성내용
                (string, string) val = (fields[4], fields[5].Replace("\\n","\n"));

                if (!data.ContainsKey(key))
                {
                    data[key] = new List<(string, string)>();
                }

                data[key].Add(val);
            }
        }

        return data;
    }

    Dictionary<string, List<Dictionary<string, string>>> ReadChoiceIDCSV()
    {
        Dictionary<string, List<Dictionary<string, string>>> data = new Dictionary<string, List<Dictionary<string, string>>>();

        TextAsset csvData = Resources.Load<TextAsset>("ChatterChoiceID");

        if (csvData == null) return data;

        string[] lines = csvData.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

        if (lines.Length == 0) return data;

        //첫번째 줄을 각 원소 내 딕셔너리의 헤더로
        string[] headers = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            if (!string.IsNullOrEmpty(lines[i]))
            {
                string[] fields = lines[i].Split(',');

                //선택지세트ID
                string key = fields[1];

                //내용 ~ 현금까지
                Dictionary<string,string> val = new Dictionary<string,string>();
                for (int j = 3; j < fields[j].Length; j++)
                {
                    val[headers[j]] = fields[j];
                }

                if (!data.ContainsKey(key))
                {
                    data[key] = new List<Dictionary<string, string>>();
                }

                data[key].Add(val);
            }
        }

        return data;
    }

    void GenTimeLine()
    {
        for (int i = 0; i < numberOfPost; i++)
        {
            GameObject postOb = Instantiate(postPrefab, transform.position, transform.rotation);
            postOb.transform.Find("Name").GetComponent<TMP_Text>().text = PostIDData[postSetID][postIdx[i]].Item1;
            postOb.transform.Find("Post").GetComponent<TMP_Text>().text = PostIDData[postSetID][postIdx[i]].Item2;

            postOb.transform.SetParent(content.transform);
        }
    }
}
