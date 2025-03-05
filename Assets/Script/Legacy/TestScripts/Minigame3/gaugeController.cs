using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class gaugeController : MonoBehaviour
{
    public GameObject minigame3Ob; //미니게임3 전체
    public ImageController imageController; //시작,실패,성공이미지 컨트롤러
    public Image gauge; //게이지 이미지
    public Button button; //클릭버튼
    public GameObject startImage; //시작이미지팝업

    public Image memberImage; //멤버이미지 컴포넌트
    public Sprite memberImage1; //멤버 좌측 이미지
    public Sprite memberImage2; //멤버 우측 이미지
    public Sprite memberImage3; //멤버 정면 이미지
    public Image playerImage; //플레이어이미지 컴포넌트
    public Sprite playerImage1; //플레이어 일반이미지
    public Sprite playerImage2; //손을 반 올린 이미지
    public Sprite playerImage3; //완전히 올린 이미지

    public float time = 15f; //제한시간
    private float fillAmount = 0f; // 초기 게이지 양
    private float decreaseRate = 0.1f; //1초마다 줄어드는 게이지 양
    private bool isActive = true;   // 게이지 동작 활성화 여부
    private int clickCount = 0; //버튼 클릭 횟수
    private int n =2;//나중에 레벨값과 연결
    private Coroutine imageCoroutine; //이미지 변경 코루틴 임시



    void Start()
    {
        StartCoroutine(imageController.ShowImageForSeconds("StartImage", 5.0f)); // 이름이 "StartImage"인 이미지 5초 동안 표시
        button.onClick.AddListener(OnButtonClick);
        imageCoroutine = StartCoroutine(CycleMemberImages()); //멤버 이미지 변경 코루틴
        SetGaugeValue(fillAmount); //시작시 플레이어 이미지 업데이트
    }

    void Update()
    {
        if (isActive)
        {
            //게이지 1 미만일때만 시간 감소
            if (fillAmount < 1f)
            {
                time -= Time.deltaTime; //타이머 감소
                //Debug.Log(time);
                if (time <= 0f) //제한시간 끝났을 때
                {
                    if (imageCoroutine != null)
                    {
                        StopCoroutine(imageCoroutine); //멤버이미지변경 코루틴 멈춤
                        imageCoroutine = null;
                    }

                    Debug.Log("@@@@@@@@@@시간 끝@@@@@@@@@@@@");
                    isActive = false;
                    // minigame3Ob.SetActive(false);
                    StartCoroutine(ClosePopup());
                    /*DelayManager.ExecuteAfterDelay(this, 3f, () => { 
                        ClosePopup();
                    });*/
                    return;
                }
            }

            if (fillAmount >= 1f)
            {
                return;
            }

            if (fillAmount > 0f) //1초마다 게이지 줄어듦
            {
                fillAmount -= decreaseRate * Time.deltaTime;
                fillAmount = Mathf.Clamp01(fillAmount);
                SetGaugeValue(fillAmount);
            }

        }
    }
    void OnButtonClick() //버튼 클릭 시 게이지 채우기
    {
        if (isActive)
        {
            clickCount ++;
            Debug.Log(clickCount);

            if (clickCount >= GetClickPerIncrement())
            {
                fillAmount +=0.075f; //한번에 차는 게이지 양
                fillAmount = Mathf.Clamp01(fillAmount); //게이지 범위 0~1로 제한
                SetGaugeValue(fillAmount);

                if (fillAmount >= 1f) //게이지 값이 1이상
                {
                    Debug.Log("@@@@@@@@@@@@@@@@게이지 꽉 참@@@@@@@@@@@@@@@@@@@@");
                    Debug.Log("얻은 에너지: (time+5)/5");
                    //3초 후 팝업닫힘
                    StartCoroutine(ClosePopup());
                    /*DelayManager.ExecuteAfterDelay(this, 3f, () => { 
                        ClosePopup();
                    });*/
                } 
                clickCount = 0;
            }
        }
    }

    //게이지 값 설정 함수
    public void SetGaugeValue(float value)
    {
        gauge.fillAmount = value; // 게이지 채우기 설정

        //게이지 범위에 따라 플레이어 이미지 변경
        if (value <= 0.3f)
        {
            playerImage.sprite = playerImage1;// 플레이어 일반이미지
        }
        else if (value <= 0.6f)
        {
            playerImage.sprite = playerImage2; //손을 반 올린 이미지
        }
        else
        {
            playerImage.sprite = playerImage3; //완전히 올린 이미지
        }
        if (value >= 1f)
        {
            if (imageCoroutine != null)
            {
                StopCoroutine(imageCoroutine);
                imageCoroutine =  null;
            }
            memberImage.sprite = memberImage3; //게이지 꽉 차면 정면이미지 고정

            Debug.Log("@@@@@@@@@@@@@@@@성공@@@@@@@@@@@@@@@@@@2");
            //성공 이미지 추가
        }
    }

    //멤버 사진 병경 코루틴
    IEnumerator CycleMemberImages()
    {
        while (true)
        {
            memberImage.sprite = memberImage1;
            yield return new WaitForSeconds(1f);
            memberImage.sprite = memberImage2;
            yield return new WaitForSeconds(1f);
        }
    }

    //클릭 횟수 결정함수
    int GetClickPerIncrement()
    {
        return (n/2)+1;
    }

    //팝업창 닫기
    IEnumerator ClosePopup()
    {
        yield return new WaitForSeconds(3f);
        minigame3Ob.SetActive(false); // 팝업 패널 비활성화
    }
}
