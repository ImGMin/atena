using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JudgeBarController : MonoBehaviour
{
    public RectTransform judgeBar; // 판정바 RectTransform
    public RectTransform success; // 성공 범위 RectTransform
    public RectTransform bigSuccess; // 대성공 범위 RectTransform
    public Button cameraButton; // 카메라 버튼
    public float speed = 50f; // 판정바 이동 속도
    private bool movingRight = true; // 판정바가 오른쪽으로 움직이는지 여부

    public GameObject attemptPrefab; // 기회 프리팹
    public Transform attemptsPanel; // 기회 이미지를 배치할 패널
    public GameObject popupPanel; // 팝업 패널 오브젝트
    private int attempts = 3; // 남은 기회 수
    private GameObject[] attemptImages; // 기회 이미지를 저장할 배열

    private List<bool> attemptResults; // 시도 결과를 저장할 리스트

    // 클론 생성 위치를 지정할 배열 (필요에 따라 위치를 설정하세요)
    public Vector2[] clonePositions = new Vector2[] 
    {
        new Vector2(-200, 0), // 첫 번째 클론 위치
        new Vector2(0, 0),    // 두 번째 클론 위치
        new Vector2(200, 0)   // 세 번째 클론 위치
    };

    void Start()
    {
        cameraButton.onClick.AddListener(CheckSuccess);

        // attempt 이미지 초기화
        attemptImages = new GameObject[attempts];
        for (int i = 0; i < attempts; i++)
        {
            GameObject attemptImage = Instantiate(attemptPrefab, attemptsPanel);
            attemptImage.GetComponent<RectTransform>().anchoredPosition = clonePositions[i]; // 위치 설정
            attemptImages[i] = attemptImage;
        }

        attemptResults = new List<bool>(); // 시도 결과 리스트 초기화
    }

    void Update()
    {
        MoveJudgeBar();
    }

    void MoveJudgeBar()
    {
        float step = speed * Time.deltaTime;
        if (movingRight)
        {
            judgeBar.anchoredPosition += new Vector2(step, 0);
            if (judgeBar.anchoredPosition.x >= (GetComponent<RectTransform>().rect.width / 3 ))
            {
                movingRight = false;
            }
        }
        else
        {
            judgeBar.anchoredPosition -= new Vector2(step, 0);
            if (judgeBar.anchoredPosition.x <= -(GetComponent<RectTransform>().rect.width / 3 ))
            {
                movingRight = true;
            }
        }
    }

    void CheckSuccess()
    {
        float judgeBarX = judgeBar.anchoredPosition.x;
        float successMin = success.anchoredPosition.x - (success.rect.width / 2);
        float successMax = success.anchoredPosition.x + (success.rect.width / 2);
        float greatSuccessMin = bigSuccess.anchoredPosition.x - (bigSuccess.rect.width / 2);
        float greatSuccessMax = bigSuccess.anchoredPosition.x + (bigSuccess.rect.width / 2);

        bool isSuccess = false;

        if (judgeBarX >= greatSuccessMin && judgeBarX <= greatSuccessMax)
        {
            Debug.Log("Great Success!");
            isSuccess = true;
        }
        else if (judgeBarX >= successMin && judgeBarX <= successMax)
        {
            Debug.Log("Success!");
            isSuccess = true;
        }
        else
        {
            Debug.Log("Fail");
            isSuccess = false;
        }

        attemptResults.Add(isSuccess); // 시도 결과를 리스트에 추가
        UpdateAttempts(isSuccess);

        // 모든 시도가 끝났는지 확인
        if (attempts == 0)
        {
            Debug.Log(string.Join(", ", attemptResults));
            resultMessage();
            ClosePopup();
        }
    }

    void UpdateAttempts(bool isSuccess)
    {
        if (attempts > 0)
        {
            int attemptIndex = 3 - attempts; // 현재 시도 인덱스 계산
            Image attemptImage = attemptImages[attemptIndex].GetComponent<Image>();
            if (isSuccess)
            {
                Color yellowColor;
                if (ColorUtility.TryParseHtmlString("#FFEC41", out yellowColor))
                {
                    attemptImage.color = yellowColor; // 성공 시 색상 변경
                }
            }
            attempts--;
        }
    }

    void ClosePopup()
    {
        popupPanel.SetActive(false); // 팝업 패널 비활성화
    }

    void resultMessage()
    {
        int truecount = 0;
        foreach (bool item in attemptResults)
        {
            if (item == true)
            {
                truecount += 1;
            }
        }
        
        if (truecount == 3)
        {
            Debug.Log("A");
        }
        else if (truecount == 2)
        {
            Debug.Log("B");
        }
        else if (truecount == 1)
        {
            Debug.Log("C");
        }
        else if (truecount == 0)
        {
            Debug.Log("실패");
        }
    }
}
