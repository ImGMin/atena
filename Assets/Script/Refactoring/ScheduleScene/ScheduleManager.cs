using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScheduleManager : MonoBehaviour
{
    private string[] situList = new string[5];
    private string[] workList = new string[5] { "Rest", "Rest", "Rest", "Rest", "Rest" };
    private bool[] canWork = new bool[5];
    

    public Button[] WeekDayButtonList = new Button[5];
    public Image[] SituImage = new Image[5];
    public Image SelectRect;
    int SelectIdx = -1;

    public Button[] WorkButtonList = new Button[4];
    public Image[] WorkImage = new Image[5];
    bool[] check = new bool[5];

    public Button CompleteButton;
    bool ActiveCB;

    private void Awake()
    {
        LoadData();
        ButtonSetting();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        foreach (Button button in WeekDayButtonList)
        {
            button.onClick.RemoveAllListeners();
        }

        foreach (Button button in WorkButtonList)
        {
            button.onClick.RemoveAllListeners();
        }

        CompleteButton.onClick.RemoveAllListeners();
    }

    void LoadData()
    {
        string streamingFilePath = Application.streamingAssetsPath + $"/DateData.db";

        using (var connection = new SqliteConnection("URI=file:" + streamingFilePath))
        {
            connection.Open();


            using (var command = connection.CreateCommand())
            {
                string name = $"Situ{GameManager.Instance.atenaDate.year}_{GameManager.Instance.atenaDate.month}";
                int day = (GameManager.Instance.atenaDate.day-1) / 5 * 5 + 1;
                Debug.Log(day);

                command.CommandText = $"SELECT * FROM {name} WHERE 일 >= {day} AND 일 < {day+5}";
                using (IDataReader reader = command.ExecuteReader())
                {

                    int idx = 0;
                    while (reader.Read())
                    {
                        situList[idx] = (reader["상황ID"].ToString());
                        if (reader["근무ID"].ToString() == "TRUE")
                        {
                            canWork[idx] = true;
                        }
                        else
                        {
                            canWork[idx] = false;
                        }
                        idx++;
                    }
                }
            }
        }
    }

    void ButtonSetting()
    {
        for (int i = 0; i < canWork.Length; i++)
        {
            if (canWork[i])
                continue;

            SituImage[i].gameObject.SetActive(true);
            TMP_Text buttonText = SituImage[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = $"상황\n{situList[i]}";
            check[i] = true;
        }

        foreach (Button button in WeekDayButtonList)
        {
            button.onClick.AddListener(() => OnWeekdayButtonClick(button));
        }

        foreach (Button button in WorkButtonList)
        {
            if (button == null) break;
            button.onClick.AddListener(() => OnWorkButtonClick(button));
        }

        CompleteButton.onClick.AddListener(OnCompleteButtonClick);

    }
    bool CheckComplete()
    {
        for (int i = 0; i < check.Length; ++i)
        {
            if (!check[i]) return false;
        }

        return true;
    }

    void OnWeekdayButtonClick(Button clickedButton)
    {
        int index = -1;
        for (int i = 0; i < WeekDayButtonList.Length; i++)
        {
            if (clickedButton == WeekDayButtonList[i])
            {
                index = i;
                break;
            }
        }

        if (SelectIdx == index)
        {
            SelectIdx = -1;
            SelectRect.gameObject.SetActive(false);
        }
        else if (canWork[index])
        {
            SelectIdx = index;
            SelectRect.transform.SetParent(clickedButton.transform);
            SelectRect.rectTransform.anchoredPosition = Vector2.zero;
            SelectRect.gameObject.SetActive(true);
        }
    }

    void OnWorkButtonClick(Button clickedButton)
    {
        if (SelectIdx == -1) return;

        int index = -1;
        for (int i = 0; i < WorkButtonList.Length; i++)
        {
            if (clickedButton == WorkButtonList[i])
            {
                index = i;
                break;
            }
        }

        WorkImage[SelectIdx].gameObject.SetActive(true);
        TMP_Text buttonText = WorkImage[SelectIdx].GetComponentInChildren<TMP_Text>();
        if (index == 0)
        {
            buttonText.text = $"휴식";
            workList[SelectIdx] = "Rest";
        }
        else
        {
            buttonText.text = $"알바{index}";
            workList[SelectIdx] = $"Work{index}";
        }

        check[SelectIdx] = true;

        SelectIdx = -1;
        SelectRect.gameObject.SetActive(false);
    }

    void OnCompleteButtonClick()
    {
        if (!CheckComplete())
        {
            return;
        }

        for (int i = 0; i < workList.Length; i++)
        {
            GameManager.Instance.situData[i] = situList[i];
            GameManager.Instance.workData[i] = workList[i];
        }

        GameManager.Instance.SaveGameData("WeekData");
        if (situList[(GameManager.Instance.atenaDate.day-1)%5] == "Situ_02_01_01")
        {
            SceneManager.LoadScene("OfflineScene");
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}