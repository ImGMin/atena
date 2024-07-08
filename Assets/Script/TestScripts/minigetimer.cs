using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // Silder class 사용하기 위해 추가합니다.
public class SliderTimer : MonoBehaviour
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
            timerSlider.value = timerValue;
        }
        else
        {
            timerValue = 0;
            Debug.Log("0000000000000000000000000");
        }
    }
}