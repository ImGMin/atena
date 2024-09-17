using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    int WritableCount = 1;
    public Button WriteButton;
    public Button UploadButton;
    public GameObject WritePanel;

    public Button[] ChoiceButtonList = new Button[3];
    string choiceSetID;
    List<int> choiceIdx;

    int SelectIdx = -1;
    public TMP_Text SelectText;

    private void Awake()
    {
        SituIDData = ReadSituIDCSV();
        PostIDData = ReadPostIDCSV();
        ChoiceIDData = ReadChoiceIDCSV();

        //오늘의 상황ID의 번호, ID
        situNum = GameManager_prev.Instance.gameData.Schedule[GameManager_prev.Instance.gameData.curTime.weekday].Item1;
        situID = GameManager_prev.Instance.NumToSitu[situNum];

        //랜덤하게 게시글세트, 선택지세트의 ID 추출
        (postSetID, choiceSetID) = SituIDData[situID][Program.GetRandomIndices(SituIDData[situID].Count, 1)[0]];

        //포스트는 하루동안 고정
        postIdx = Program.GetRandomIndices(PostIDData[postSetID].Count,numberOfPost);
        GenTimeLine();

        WriteButton.onClick.AddListener(() => OnWriteButtonClick());
        UploadButton.onClick.AddListener(() => OnUploadButtonClick(UploadButton));

        foreach (Button button in ChoiceButtonList)
        {
            button.onClick.AddListener(() => OnChoiceButtonClick(button));
        }
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
                (string, string) val = (fields[4], fields[5].Replace("\\n","\n").Replace("\\c",","));

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
                for (int j = 3; j < fields.Length; j++)
                {
                    val[headers[j]] = fields[j].Replace("\\n", "\n").Replace("\\c", ",");
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

            TMP_Text postText = postOb.transform.Find("Post").GetComponent<TMP_Text>();
            postText.text = PostIDData[postSetID][postIdx[i]].Item2;

            float height = Program.CalculateTextHeight(postText);
            RectTransform rect = postOb.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + (height - 55f));

            postOb.transform.SetParent(content.transform);
        }
    }

    void OnWriteButtonClick()
    {
        if (WritableCount == 0)
        {
            Debug.Log("글쓰기 횟수 모두 소모");
            return;
        }

        WritePanel.SetActive(true);
        choiceIdx = Program.GetRandomIndices(ChoiceIDData[choiceSetID].Count, 3);
        for (int i = 0; i < 3; i++)
        {
            ChoiceButtonList[i].transform.Find("Post").GetComponent<TMP_Text>().text = 
                ChoiceIDData[choiceSetID][choiceIdx[i]]["내용"].Replace("@최애", $"@{GameManager_prev.Instance.gameData.myFavorite}");

            ChoiceButtonList[i].transform.Find("Resource").GetComponent<TMP_Text>().text = PenaltyString(i);
        }
    }

    void OnChoiceButtonClick(Button clickedButton)
    {
        int index = -1;
        for (int i = 0; i < ChoiceButtonList.Length; i++)
        {
            if (clickedButton == ChoiceButtonList[i])
            {
                index = i;
                break;
            }
        }

        if (SelectIdx == index)
        {
            SelectText.text = "";
            SelectIdx = -1;
        }
        else
        {
            SelectText.text = ChoiceIDData[choiceSetID][choiceIdx[index]]["내용"].Replace("@최애", $"@{GameManager_prev.Instance.gameData.myFavorite}");
            SelectIdx = index;
        }
    }

    void OnUploadButtonClick(Button clickedButton)
    {
        if (SelectIdx == -1) return;
        if (!CheckPenalty()) return;

        Dictionary<string, string> tmp = ChoiceIDData[choiceSetID][choiceIdx[SelectIdx]];
        int exp = int.Parse(tmp["경험치"]);
        int reputation = int.Parse(tmp["평판"]);
        int friends = int.Parse(tmp["친구수"]);
        int atenaGrowth = int.Parse(tmp["성장도"]);
        int energy = int.Parse(tmp["에너지"]);
        int cash = int.Parse(tmp["현금"]);

        GameManager_prev.Instance.ChangeValue(exp: exp, energy: energy,friends:friends , cash: cash, reputation: reputation, atenaGrowth:atenaGrowth);

        GameObject postOb = Instantiate(postPrefab, transform.position, transform.rotation);
        postOb.transform.Find("Name").GetComponent<TMP_Text>().text = GameManager_prev.Instance.gameData.playerName;

        TMP_Text postText = postOb.transform.Find("Post").GetComponent<TMP_Text>();
        postText.text = tmp["내용"].Replace("@최애", $"@{GameManager_prev.Instance.gameData.myFavorite}");

        float height = Program.CalculateTextHeight(postText);
        RectTransform rect = postOb.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + (height - 55f));

        postOb.transform.SetParent(content.transform);

        WritePanel.SetActive(false);
        WritableCount--;
    }

    bool CheckPenalty()
    {
        Dictionary<string, string> tmp = ChoiceIDData[choiceSetID][choiceIdx[SelectIdx]];
        int energy = int.Parse(tmp["에너지"]);
        int cash = int.Parse(tmp["현금"]);

        if (GameManager_prev.Instance.gameData.energy < energy) return false;
        if (GameManager_prev.Instance.gameData.cash < cash) return false;

        return true;
    }

    string PenaltyString(int idx)
    {
        string res = "";
        int enengy = int.Parse(ChoiceIDData[choiceSetID][choiceIdx[idx]]["에너지"]);
        int cash = int.Parse(ChoiceIDData[choiceSetID][choiceIdx[idx]]["현금"]);
        if (enengy < 0 && cash < 0)
        {
            res = $"에너지 {enengy} / 현금 {cash}원";
        }
        else if (enengy < 0)
        {
            res = $"에너지 {enengy}";
        }
        else if (cash < 0)
        {
            res = $"현금 {cash}원";
        }

        return res;
    }
}
