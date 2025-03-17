using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfflineSceneFlowManager : MonoBehaviour
{

    [Header("이미지 리스트 (총 5개)")]
    public Sprite[] sprites;  // 5개의 사진 (Sprite)

    [Header("UI Image 오브젝트")]
    public Image[] imageSlots;  // 4개의 Image 오브젝트

    [Header("고정할 이미지 인덱스 (0~4)")]
    public int fixedImageIndex = 0;  // 4번째(마지막) 슬롯에 고정될 사진의 인덱스

    List<string> fileNames = new List<string> { "보안", "하나", "진설", "신아", "예현" };
    List<string> numericHeaders = new List<string> { "경험치", "평판", "친구수", "성장도", "호감도", "에너지" };
    private Dictionary<string, string> numericHeaderMap = new Dictionary<string, string>
    {
        { "경험치", "exp" },
        { "평판", "reputation" },
        { "친구수", "friends" },
        { "성장도", "atenaGrowth" },
        { "호감도", "favor" },
        { "에너지", "energy" }
    };


    Dictionary<string, List<Dictionary<string, string>>> allData = new Dictionary<string, List<Dictionary<string, string>>>();

    List<Dictionary<string, string>> question;

    List<int> num = new List<int>() { 0, 1, 2, 3, 4 };
    List<int> rank = new List<int> { 0, 1, 2, 3, 4 };

    bool miniGame1Result = true;

    public RectTransform panel;  // 이동할 UI 패널
    public Vector2 targetPosition; // 최종 목적지
    public float duration = 2.0f;  // 이동 시간

    public GameObject miniGame1;
    public GameObject miniGame2;
    public GameObject miniGame3;

    public Button button2;
    public Button button3;

    public Button selectButton1;
    public Button selectButton2;

    Dictionary<string, string> bt1;
    Dictionary<string, string> bt2;

    public Image answer;
    public Image emoji;

    public List<Sprite> emojis;
    private Dictionary<string, int> emotionMap = new Dictionary<string, int>
    {
        { "고민", 0 },
        { "감사", 1 },
        { "기쁨", 2 },
        { "당황", 3 },
        { "슬픔", 4 },
        { "화남", 5 }
    };

    public GameObject SceneManager;

    private Vector2 startPosition;

    private int moveCount = 0;  // 이동 횟수 카운트
    private bool isMoving = false; // 현재 이동 중인지 여부
    private float moveDistance = 700f; // 이동 거리 (예제: 100픽셀)
    private float moveDuration = 1.0f; // 이동 시간 (1초)
    private float maxTime = 120f; // 최대 제한 시간 (120초)

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (string fileName in fileNames)
        {
            List<Dictionary<string, string>> csvData = CSVLoader.LoadCSV(fileName);
            if (csvData != null)
            {
                allData[fileName] = csvData;
            }
        }

        question = CSVLoader.LoadCSV("질문");

        ShuffleAndAssign();
        startPosition = panel.anchoredPosition; // 시작 위치 저장
        StartCoroutine(GameSequence());

        button2.onClick.AddListener(() => ActivateObjectAndDisableButton(miniGame2, button2));
        button3.onClick.AddListener(() => ActivateObjectAndDisableButton(miniGame3, button3));

        // 데이터 출력 (디버깅용)
        /*foreach (var file in allData)
        {
            Debug.Log($"파일명: {file.Key}");
            foreach (var row in file.Value)
            {
                foreach (var kvp in row)
                {
                    Debug.Log($"{kvp.Key}: {kvp.Value}");
                }
            }
        }*/
    }

    void OnDestroy()
    {
        // 씬이 전환되거나 오브젝트가 삭제될 때 리스너 제거
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
    }

    IEnumerator GameSequence()
    {
        Debug.Log("이벤트 1: 애니메이션 시작");
        yield return StartCoroutine(MovePanel(0));
        yield return new WaitForSeconds(0.5f);

        Debug.Log("이벤트 2: 미니게임 1 시작");
        yield return StartCoroutine(MiniGameStart1());

        Debug.Log("이벤트 3: 일대일 대화 미구현(3초 대기)");
        yield return StartCoroutine(OneOnOneChat());

        Debug.Log("이벤트 4: 대기 중 미니게임 버튼 활성화");
        yield return StartCoroutine(SelectButtonEnable());

        Debug.Log("귀가");
        yield return new WaitForSeconds(1f);

        Debug.Log("씬 이동");
        SceneManager.GetComponent<OfflineSceneChangeManager>().Exit();
    }

    void ActivateObjectAndDisableButton(GameObject targetObject, Button targetButton)
    {
        // 특정 오브젝트 활성화
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }

        // 버튼 비활성화
        if (targetButton != null)
        {
            targetButton.interactable = false; // 버튼 비활성화 (회색 처리)
            // 또는 완전히 숨기려면: targetButton.gameObject.SetActive(false);
        }
    }


    void ShuffleAndAssign()
    {
        if (sprites.Length != 5 || imageSlots.Length != 5)
        {
            Debug.LogError("스프라이트는 정확히 5개, Image 슬롯은 정확히 4개여야 합니다!");
            return;
        }

        // 1. 고정된 이미지 배치
        imageSlots[4].sprite = sprites[fixedImageIndex];

        // 2. 나머지 4개의 스프라이트 리스트 만들기 (고정된 이미지는 제외)
        List<Sprite> availableSprites = new List<Sprite>(sprites);

        num.RemoveAt(fixedImageIndex);
        ShuffleList(num);
        num.Add(fixedImageIndex);

        for (int i = 0; i < 5; i++)
        {
            rank[num[i]] = i;
        }

        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"{rank[i]} {allData[fileNames[rank[i]]][0]["선택지ID"]}");
        }

        for (int i = 0; i < 4; i++) // 0~3번 슬롯에 배치
        {
            imageSlots[i].sprite = availableSprites[num[i]];
        }

        Debug.Log($"[랜덤 배치] {fixedImageIndex}번 사진은 4번째 칸에 고정됨.");
    }

    void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]); // Swap
        }
    }

    IEnumerator MovePanel(int p)
    {
        preApplyData(p);
        if (p != 0)
        {
            yield return new WaitForSeconds(3f);
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPosition; // 마지막 위치 고정
    }

    IEnumerator MovePanel2(int p)
    {
        preApplyData(p);
        if (p != 0)
        {
            yield return new WaitForSeconds(3f);
        }

        isMoving = true;
        Vector3 startPos = panel.transform.position;
        Vector3 endPos = startPos + (Vector3.right * moveDistance);

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            panel.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.transform.position = endPos;
        isMoving = false;
    }

    IEnumerator MiniGameStart1()
    {
        miniGame1.SetActive(true);
        while (miniGame1.activeSelf)
        {
            yield return null;
        }
    }

    IEnumerator OneOnOneChat()
    {
        float startTime = Time.time; // 시작 시간 기록
        int idx = 4;

        List<int> curBt;
        string curName;

        selectButton1.gameObject.SetActive(true);
        selectButton2.gameObject.SetActive(true);

        while (moveCount < 5)
        {
            // 120초가 지나면 종료
            if (Time.time - startTime >= maxTime)
            {
                Debug.Log("시간 초과! 코루틴 종료.");
                yield break;
            }

            // 버튼 비활성화 (이전 이동이 끝나야 활성화됨)
            selectButton1.gameObject.SetActive(false);
            selectButton2.gameObject.SetActive(false);

            // 버튼 클릭 대기
            bool clicked = false;
            if (moveCount != 4){
                selectButton1.onClick.AddListener(() => { if (!isMoving) { StartCoroutine(MovePanel2(1)); clicked = true; } });
                selectButton2.onClick.AddListener(() => { if (!isMoving) { StartCoroutine(MovePanel2(2)); clicked = true; } });
            }
            else
            {
                selectButton1.onClick.AddListener(() => { if (!isMoving) { StartCoroutine(MovePanel(1)); clicked = true; } });
                selectButton2.onClick.AddListener(() => { if (!isMoving) { StartCoroutine(MovePanel(2)); clicked = true; } });
            }

            // 버튼 활성화 (이전 이동이 끝난 후)
            yield return new WaitUntil(() => !isMoving);

            if (moveCount != 0)
                yield return new WaitForSeconds(3f);

            curBt = Program.GetRandomIndices(15, 2); //이후 범위 대응 가능하도록 ID에 맞게 수정
            curName = fileNames[rank[idx]];
            idx--;

            bt1 = allData[curName][curBt[0]];
            bt2 = allData[curName][curBt[1]];

            selectButton1.gameObject.SetActive(true);
            selectButton1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = question[curBt[0]][$"선택지내용"];
            selectButton2.gameObject.SetActive(true);
            selectButton2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = question[curBt[1]][$"선택지내용"];

            // 클릭될 때까지 대기
            yield return new WaitUntil(() => clicked);

            // 버튼 리스너 제거 (중복 방지)
            selectButton1.onClick.RemoveAllListeners();
            selectButton2.onClick.RemoveAllListeners();

            moveCount++; // 이동 횟수 증가
        }

        selectButton1.gameObject.SetActive(false);
        selectButton2.gameObject.SetActive(false);

        Debug.Log("이동 완료! 코루틴 종료.");
    }



    IEnumerator SelectButtonEnable()
    {
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float t = 60f;

        while ((miniGame2.activeSelf||miniGame3.activeSelf)||
            (elapsedTime < t && (button2.interactable||button3.interactable)))
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void preApplyData(int p)
    {
        if (p == 0) return;

        if (p == 1)
        {
            StartCoroutine(ApplyData(bt1));
            return;
        }

        if (p == 2)
        {
            StartCoroutine(ApplyData(bt2));
            return;
        }
    }

    IEnumerator ApplyData(Dictionary<string, string> curData)
    {
        foreach (string st in numericHeaders)
        {
            int tmp = int.Parse(curData[st]);
            if (!miniGame1Result)
            {
                if (tmp < 0)
                {
                    tmp *= 2;
                }
                else
                {
                    tmp /= 2;
                }
            }

            GameManager.Instance.ChangeValue(numericHeaderMap[st], tmp);
        }


        answer.gameObject.SetActive(true);
        answer.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = curData["답변"];
        yield return new WaitForSeconds(2f);
        answer.gameObject.SetActive(false);

        // 반응말풍선을 1초 동안 화면에 표시
        if (emotionMap.ContainsKey(curData["반응말풍선"]))
        {
            int emojiIndex = emotionMap[curData["반응말풍선"]];
            emoji.gameObject.SetActive(true);
            emoji.transform.GetChild(0).GetComponent<Image>().sprite = emojis[emojiIndex];
            yield return new WaitForSeconds(1f);
            emoji.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"잘못된 반응말풍선 데이터 {curData["반응말풍선"]}");
        }

        yield return null;

        yield return null;
    }
}
