using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternButtonGenerator : MonoBehaviour
{
    public GameObject noteButtonPrefab; // 노트 버튼 프리팹
    public Transform parentTransform; // 버튼이 배치될 부모 객체
    public Vector2[] buttonPositions; // 버튼의 위치 배열
    public List<NoteButton> generatedButtons = new List<NoteButton>(); // 생성된 버튼 리스트

    public delegate void ButtonsGeneratedHandler();
    public event ButtonsGeneratedHandler OnButtonsGenerated;

    void Start()
    {
        GeneratePatternButtons();
    }

    void GeneratePatternButtons()
    {
        for (int i = 0; i < buttonPositions.Length; i++)
        {
            GameObject newButton = Instantiate(noteButtonPrefab, parentTransform);
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            if (buttonRectTransform != null)
            {
                buttonRectTransform.anchoredPosition = buttonPositions[i];
                buttonRectTransform.sizeDelta = new Vector2(70, 70); // 버튼 크기 설정 (원하는 크기로 조정 가능)

                // 랜덤한 노트 타입 할당
                NoteType randomNoteType = (NoteType)Random.Range(0, System.Enum.GetValues(typeof(NoteType)).Length);
                NoteButton noteButton = newButton.GetComponent<NoteButton>();
                if (noteButton != null)
                {
                    noteButton.noteType = randomNoteType;
                    generatedButtons.Add(noteButton);
                }

                // 버튼 클릭 이벤트를 설정
                Button buttonComponent = newButton.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.onClick.AddListener(() => OnButtonClick(noteButton));
                }
            }
            else
            {
                Debug.LogError("Button RectTransform not found on prefab.");
            }
        }

        OnButtonsGenerated?.Invoke(); // 버튼 생성 완료 이벤트 호출
    }

    void OnButtonClick(NoteButton noteButton)
    {
        Debug.Log("Generated Button clicked: " + noteButton.noteType);
        // 원하는 동작을 추가할 수 있습니다.
    }
}

public enum NoteType
{
    Up,
    Down,
    Left,
    Right
}

public class NoteButton : MonoBehaviour
{
    public NoteType noteType;
}
