using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    int cur;
    AtenaDate GameDataToday;

    void Awake()
    {
        cur = GameManager.Instance.gameData.curTime.day;
        GameDataToday = GameManager.Instance.gameData.curTime;
    }

    void Update()
    {
        if (cur != GameDataToday.day)
        {
            SceneManager.LoadScene("ChangeDate");
        }
    }
}
