using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScheduleManager : MonoBehaviour
{
    public Button[] WeekDayButtonList = new Button[5];
    public Image SelectRect;
    int SelectIdx = -1;

    public Button[] WorkButtonList = new Button[4];
    bool[] check = new bool[5];

    public Button CompleteButton;
    bool ActiveCB;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeValue(cash: GameManager.Instance.earnings);
        GameManager.Instance.earnings = 0;

        for (int i = 0; i < WeekDayButtonList.Length; i++)
        {
            if (GameManager.Instance.gameData.Schedule[i].Item1 != 0)
            {
                TMP_Text buttonText = WeekDayButtonList[i].GetComponentInChildren<TMP_Text>();
                buttonText.text = $"상황{GameManager.Instance.NumToSitu[GameManager.Instance.gameData.Schedule[i].Item1]}";
                check[i] = true;
            }
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

        CompleteButton.onClick.AddListener(() => OnCompleteButtonClick());

    }

    private void Update()
    {
        if (!ActiveCB && CheckComplete())
        {
            CompleteButton.interactable = true;
            ActiveCB = true;
        }
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

        //튜토리얼 : 월요일 제외
        if (GameManager.Instance.Tutorial && index == 0) { return ; }

        if (SelectIdx == index)
        {
            SelectIdx = -1;
            SelectRect.gameObject.SetActive(false);
        }
        else if (GameManager.Instance.gameData.Schedule[index].Item1 == 0)
        {
            SelectIdx = index;
            SelectRect.rectTransform.anchoredPosition = clickedButton.GetComponent<RectTransform>().anchoredPosition;
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

        GameManager.Instance.gameData.Schedule[SelectIdx].Item2 = index;
        TMP_Text buttonText = WeekDayButtonList[SelectIdx].GetComponentInChildren<TMP_Text>();
        if (index == 0)
        {
            buttonText.text = $"휴식";
        }
        else
        {
            buttonText.text = $"알바{index}";
        }

        check[SelectIdx] = true;


        SelectIdx = -1;
        SelectRect.gameObject.SetActive(false);
    }

    void OnCompleteButtonClick()
    {
        SceneManager.LoadScene("PhoneScene");
    }
}