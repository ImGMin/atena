using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTimeManager : MonoBehaviour
{
    [SerializeField]
    protected bool timeFlag = true; //시간 흐름 여부

    [SerializeField]
    protected float timeScale = 1f;  // 시간 배율

    private float baseTimeScale = 1 / 30f;

    //public event Action<AtenaDate> OnDayChanged;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (timeFlag)
            GameManager.Instance.atenaDate.hour += Time.deltaTime * baseTimeScale * timeScale;

        if (GameManager.Instance.atenaDate.hour >= 12f)
        {
            GameManager.Instance.atenaDate.hour = 0;
            GameManager.Instance.atenaDate.day += 1;
            //OnDayChanged?.Invoke(GameManager.Instance.atenaDate);
        }
    }
    
    public void SetTimeScale(float newTimeScale)
    {
        timeScale = newTimeScale;
    }

    public void SetTimeFlag(bool newTimeFlag)
    {
        timeFlag = newTimeFlag;
    }
}
