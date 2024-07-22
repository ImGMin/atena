using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class minigametimer : MonoBehaviour
{
    public Slider timerSlider;
    private float timerValue;
    private float timerDuration = 30f;

    void Start()
    {
        timerValue = timerDuration;
        timerSlider.maxValue = timerDuration;
        timerSlider.value = timerDuration;
    }

    void Update()
    {
        if (timerValue > 0)
        {
            timerValue -= Time.deltaTime;
            UpdateSlider();
        }
        else
        {
            timerValue = 0;
            UpdateSlider();
            Debug.Log("30초 끝");
        }
    }

    public void ReduceTime(float amount)
    {
        timerValue -= amount;
        if (timerValue < 0)
        {
            timerValue = 0;
        }
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        timerSlider.value = timerValue;
        Debug.Log("타이머 업데이트: " + timerValue);
    }
}
