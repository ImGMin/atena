using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    int situ;
    int work;
    int cur;
    AtenaDate GameDataToday;

    public Image cooldownMask;
    public TMP_Text text;

    private float maxTime = 12f;
    private int countDown = 12;

    public GameObject home;
    public GameObject arbeitScene;
    int arbeit = 0;
    bool arbeitFlag = true;

    void Awake()
    {
        cur = GameManager.Instance.gameData.curTime.day;
        GameDataToday = GameManager.Instance.gameData.curTime;
        GameDataToday.hour = 0;

        (situ, work) = GameManager.Instance.gameData.Schedule[GameDataToday.weekday];
        if (situ != 0)
        {
            Debug.Log($"오프라인 이벤트 {GameManager.Instance.NumToSitu[situ]}");
        }
        else if (work != 0)
        {
            Debug.Log($"즐거운 알바 {work}, 3시간 15000원");
            if (work == 1)
            {
                GameManager.Instance.earnings += 15000;
                arbeit = 3;
            }
        }
        else
        {
            Debug.Log($"더 즐거운 휴식");
        }
    }

    private void Start()
    {
        cooldownMask.fillAmount = 0f;
        home.SetActive(false);
        arbeitScene.SetActive(true);
    }

    void Update()
    {
        if (GameDataToday.hour < maxTime)
        {
            if (GameDataToday.hour < arbeit)
            {
                GameDataToday.hour += Time.deltaTime*arbeit;

            }
            else
            {
                if (arbeitFlag)
                {
                    home.SetActive(true);
                    arbeitScene.SetActive(false);
                    arbeitFlag = false;
                }

                GameDataToday.hour += Time.deltaTime * (12f / 300f);
            }
            cooldownMask.fillAmount = GameDataToday.hour / maxTime;

            countDown = Mathf.CeilToInt(maxTime - GameDataToday.hour);
            text.text = $"{countDown}";
        }
        else
        {
            GameDataToday.hour = 0f;
            cooldownMask.fillAmount = 1f;
            GameManager.Instance.ChangeValue(curTime: 1); 
        }

        if (GameDataToday.day != cur)
        {
            SceneManager.LoadScene("ChangeDate");
        }
    }
}
