using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDateManager : MonoBehaviour
{

    float time;
    AtenaDate date;

    void Start()
    {
        time = 0;
        date = GameManager_prev.Instance.gameData.curTime;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            if ((date.day % 5 == 1 && !GameManager_prev.Instance.Tutorial) || (date.day == 2 && GameManager_prev.Instance.Tutorial))
            {
                UpdateSchedule();
                SceneManager.LoadScene("ScheduleScene");
            }
            else
            {
                SceneManager.LoadScene("PhoneScene");
            }

        }
    }

    void UpdateSchedule()
    {
        GetComponent<ScheduleCSV>().UpdateSchedule();
    }
}
