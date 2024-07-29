using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AttemptResult
{
    Fail,
    Success,
    bigSuccess
}

public class JudgeBarController : MonoBehaviour
{
    public GameObject minigame2Ob; //미니게임2 전체
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
    private GameObject[] attemptImages; // 기회 이미지 저장 배열

    private List<AttemptResult> attemptResults; // 시도 결과 저장 리스트

    // 클론 생성 위치 배열
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

        attemptResults = new List<AttemptResult>(); // 시도 결과 리스트 초기화
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
            if (judgeBar.anchoredPosition.x >= (GetComponent<RectTransform>().rect.width / 3))
            {
                movingRight = false;
            }
        }
        else
        {
            judgeBar.anchoredPosition -= new Vector2(step, 0);
            if (judgeBar.anchoredPosition.x <= -(GetComponent<RectTransform>().rect.width / 3))
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
        float bigSuccessMin = bigSuccess.anchoredPosition.x - (bigSuccess.rect.width / 2);
        float bigSuccessMax = bigSuccess.anchoredPosition.x + (bigSuccess.rect.width / 2);

        AttemptResult result = AttemptResult.Fail;

        if (judgeBarX >= bigSuccessMin && judgeBarX <= bigSuccessMax)
        {
            Debug.Log("대성공!");
            result = AttemptResult.bigSuccess;
        }
        else if (judgeBarX >= successMin && judgeBarX <= successMax)
        {
            Debug.Log("성공!");
            result = AttemptResult.Success;
        }
        else
        {
            Debug.Log("실패..");
            result = AttemptResult.Fail;
        }

        attemptResults.Add(result); // 시도 결과를 리스트에 추가
        UpdateAttempts(result);

        // 모든 시도가 끝났는지 확인
        if (attempts == 0)
        {
            Debug.Log(string.Join(", ", attemptResults));
            resultMessage();
            //ClosePopup();
        }
    }

void UpdateAttempts(AttemptResult result)
{
    if (attempts > 0)
    {
        int attemptIndex = 3 - attempts; // 현재 시도 인덱스 계산
        Image attemptImage = attemptImages[attemptIndex].GetComponent<Image>();
        Color color;
        switch (result)
        {
            case AttemptResult.bigSuccess:
                if (ColorUtility.TryParseHtmlString("#FFB400", out color)) // 대성공 시 귤색
                {
                    attemptImage.color = color;
                }
                break;
            case AttemptResult.Success:
                if (ColorUtility.TryParseHtmlString("#FFDD00", out color)) // 성공 시 노란색
                {
                    attemptImage.color = color;
                }
                break;
            case AttemptResult.Fail:
                if (ColorUtility.TryParseHtmlString("#808080", out color)) // 실패 시 회색
                {
                    attemptImage.color = color;
                }
                break;
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
        int bigSuccessCount = 0;
        int successCount = 0;

        foreach (AttemptResult result in attemptResults)
        {
            if (result == AttemptResult.bigSuccess)
            {
                bigSuccessCount++;
            }
            else if (result == AttemptResult.Success)
            {
                successCount++;
            }
        }

        if (successCount == 3 || bigSuccessCount ==1)
        {
            Debug.Log("A");
        }
        else if (successCount == 2)
        {
            Debug.Log("B");
        }
        else if (successCount == 1)
        {
            Debug.Log("C");
        }
        else
        {
            Debug.Log("실패");
        }
    }
}
