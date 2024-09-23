using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class minigametimer : MonoBehaviour
{

    public GameObject StartImage;
    public GameObject timerSliderOb;
    private Image timerSlider; //타이머 이미지
    public float timerValue;
    private float timerDuration = 30f; //제한시간
    public GameObject minigame1PopupOb; //미니게임1 팝업
    private bool isTimerRunning = true; //타이머 작동여부


    void OnEnable()
        {
        GameObject gm = Instantiate(StartImage);
        gm.transform.SetParent(this.transform);
        gm.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        gm.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "start";

        timerValue = timerDuration;
    }

    void Start()
    {
        timerSlider = timerSliderOb.GetComponent<Image>();
        
        //timerSlider.maxValue = timerDuration;
        //timerSlider.value = timerDuration;
    }

    void Update()
    {
        if (isTimerRunning && timerValue > 0)
        {
            timerValue -= Time.deltaTime;
            UpdateSlider();
        }
        else if (timerValue <= 0 && isTimerRunning)
        {
            timerValue = 0;
            UpdateSlider();
            StopTimer();
            Debug.Log("30초 끝");
            Debug.Log("@@@@@@@@@2실패@@@@@@@@@@@@");
            ClosePopup();
        }
    }

    public void ReduceTime(float amount)
    {
        timerValue -= amount;
        if (timerValue < 0)
        {
            timerValue = 0;
        }
        Debug.Log($"남은시간 : {timerValue}");
        UpdateSlider();
        Canvas.ForceUpdateCanvases();
    }

    public void StopTimer() //타이머 멈춤
    {
        Canvas.ForceUpdateCanvases();
        isTimerRunning = false;
        Debug.Log($"게임종료, 남은시간:{timerValue}");
    }
    private void UpdateSlider()
    {
        float tmp = timerSlider.fillAmount - timerValue / timerDuration;
        timerSlider.fillAmount -= tmp;
        
        //Debug.Log("타이머 업데이트: " + timerValue);
    }

    void ClosePopup()
    {
        minigame1PopupOb.SetActive(false); // 팝업 패널 비활성화
    }
}

