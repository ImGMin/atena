using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    int cur;
    int situ;
    int work;
    AtenaDate GameDataToday;

    void Awake()
    {
        Debug.Log(gameObject.name);
        cur = GameManager.Instance.gameData.curTime.day;
        GameDataToday = GameManager.Instance.gameData.curTime;

        (situ, work) = GameManager.Instance.gameData.Schedule[GameDataToday.weekday];
        if (situ != 0)
        {
            Debug.Log($"오프라인 이벤트 {situ}");
        }
        else if (work != 0)
        {
            Debug.Log($"즐거운 알바 {work}, 3시간 15000원");
            if (work == 1)
            {
                GameManager.Instance.earnings += 15000;
                GameManager.Instance.gameData.curTime.hour += 3;
            }
        }
        else
        {
            Debug.Log($"더 즐거운 휴식");
        }
    }

    void Update()
    {
        if (cur != GameDataToday.day)
        {
            SceneManager.LoadScene("ChangeDate");
        }
    }
}
