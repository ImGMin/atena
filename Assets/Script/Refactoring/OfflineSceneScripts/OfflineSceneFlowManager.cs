using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineSceneFlowManager : MonoBehaviour
{
    [Header("이미지 리스트 (총 5개)")]
    public Sprite[] sprites;  // 5개의 사진 (Sprite)

    [Header("UI Image 오브젝트")]
    public Image[] imageSlots;  // 4개의 Image 오브젝트

    [Header("고정할 이미지 인덱스 (0~4)")]
    public int fixedImageIndex = 0;  // 4번째(마지막) 슬롯에 고정될 사진의 인덱스

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
        ShuffleAndAssign();
        startPosition = panel.anchoredPosition; // 시작 위치 저장
        StartCoroutine(GameSequence());

        button2.onClick.AddListener(() => ActivateObjectAndDisableButton(miniGame2, button2));
        button3.onClick.AddListener(() => ActivateObjectAndDisableButton(miniGame3, button3));
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
        yield return StartCoroutine(MovePanel());
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
        availableSprites.RemoveAt(fixedImageIndex); // 고정된 이미지는 제거

        // 3. 셔플 후 랜덤 배치
        ShuffleList(availableSprites);

        for (int i = 0; i < 4; i++) // 0~3번 슬롯에 배치
        {
            imageSlots[i].sprite = availableSprites[i];
        }

        Debug.Log($"[랜덤 배치] {fixedImageIndex}번 사진은 4번째 칸에 고정됨.");
    }

    void ShuffleList(List<Sprite> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]); // Swap
        }
    }

    IEnumerator MovePanel()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPosition; // 마지막 위치 고정
    }

    IEnumerator MovePanel2()
    {
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
                selectButton1.onClick.AddListener(() => { if (!isMoving) { StartCoroutine(MovePanel2()); clicked = true; } });
                selectButton2.onClick.AddListener(() => { if (!isMoving) { StartCoroutine(MovePanel2()); clicked = true; } });
            }
            else
            {
                selectButton1.onClick.AddListener(() => { if (!isMoving) { StartCoroutine(MovePanel()); clicked = true; } });
                selectButton2.onClick.AddListener(() => { if (!isMoving) { StartCoroutine(MovePanel()); clicked = true; } });
            }
            // 버튼 활성화 (이전 이동이 끝난 후)
            yield return new WaitUntil(() => !isMoving);
            selectButton1.gameObject.SetActive(true);
            selectButton2.gameObject.SetActive(true);

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
}
