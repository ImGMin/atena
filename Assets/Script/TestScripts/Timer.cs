using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Image cooldownMask;
    public TMP_Text text;

    AtenaDate time;

    private float maxTime = 12f;
    private int countDown = 12;

    void Start()
    {
        cooldownMask.fillAmount = 0f;
        time = GameManager.Instance.gameData.curTime;
    }

    void Update()
    {
        if (time.hour < maxTime)
        {
            time.hour += Time.deltaTime*(12f/300f);
            cooldownMask.fillAmount = time.hour / maxTime;

            countDown = Mathf.CeilToInt(maxTime - time.hour);
            text.text = $"{countDown}";
        }
        else
        {
            time.hour = 0f;
            cooldownMask.fillAmount = 1f;
            GameManager.Instance.ChangeValue(curTime: 1);
        }
    }
}