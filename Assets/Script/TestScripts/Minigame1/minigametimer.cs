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
    private Image timerSlider;
    public float timerValue;
    private float timerDuration = 30f;
    public GameObject minigame1PopupOb;

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
            //2초 딜레이 후 팝업제거
            DelayManager.ExecuteAfterDelay(this, 2f, () => {
                minigame1PopupOb.SetActive(false);
            });
            Debug.Log("없어짐");
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

    private void UpdateSlider()
    {
        float tmp = timerSlider.fillAmount - timerValue / timerDuration;
        timerSlider.fillAmount -= tmp;
        
        //Debug.Log("타이머 업데이트: " + timerValue);
    }
}

