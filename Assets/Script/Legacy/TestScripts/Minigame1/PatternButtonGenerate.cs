using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternButtonGenerator : MonoBehaviour
{
    public GameObject noteButtonPrefab; // 노트 버튼 프리팹
    public Transform parentTransform; // 버튼이 배치될 부모 객체
    //public Vector2[] buttonPositions; // 버튼의 위치 배열
    public List<NoteButton> generatedButtons = new List<NoteButton>(); // 생성된 버튼 리스트

    public delegate void ButtonsGeneratedHandler();
    public event ButtonsGeneratedHandler OnButtonsGenerated;

    // 200x200 좌표계에서의 버튼 배치 좌표
    private Vector2[] buttonPositions = new Vector2[]
    {
        new Vector2(-226, 293),
        new Vector2(-136, 233),
        new Vector2(-46, 173),
        new Vector2(44, 113),
        new Vector2(134, 53),
        new Vector2(224, -7),
        new Vector2(140, -107),
        new Vector2(50, -167),
        new Vector2(-40, -227),
        new Vector2(-130, -287)
    };
void Start()
    {
        if (noteButtonPrefab == null || parentTransform == null)
        {
            //Debug.LogError("NoteButtonPrefab 또는 ParentTransform이 설정되지 않았습니다.");
            return;
        }

        if (buttonPositions == null || buttonPositions.Length == 0)
        {
            //Debug.LogError("buttonPositions 배열이 비어 있습니다.");
            return;
        }

        Debug.Log("PatternButtonGenerator: Start 호출됨");
        GeneratePatternButtons();
    }

void GeneratePatternButtons()
{
    generatedButtons.Clear(); // 리스트 초기화
    for (int i = 0; i < buttonPositions.Length; i++)
    {
        GameObject newButton = Instantiate(noteButtonPrefab, parentTransform);
        RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
        if (buttonRectTransform != null)
        {
            buttonRectTransform.anchoredPosition = buttonPositions[i];
            buttonRectTransform.sizeDelta = new Vector2(140, 140); // 버튼 크기 설정
            // 랜덤한 노트 타입 할당
            NoteType randomNoteType = (NoteType)Random.Range(0, System.Enum.GetValues(typeof(NoteType)).Length);
            NoteButton noteButton = newButton.GetComponent<NoteButton>();
            if (noteButton != null)
            {
                noteButton.noteType = randomNoteType;
                noteButton.buttonText.text = randomNoteType.ToString();
                generatedButtons.Add(noteButton);

            // 이미지 회전 처리
                Image noteImage = newButton.GetComponent<Image>();
                if (noteImage != null)
                {
                    switch (randomNoteType)
                    {
                        case NoteType.Up:
                            noteImage.rectTransform.rotation = Quaternion.Euler(0, 0, 0); // 기본 회전
                            break;
                        case NoteType.Down:
                            noteImage.rectTransform.rotation = Quaternion.Euler(0, 0, 180); // Z축 180도 회전
                            break;
                        case NoteType.Left:
                            noteImage.rectTransform.rotation = Quaternion.Euler(0, 0, 90); // Z축 90도 회전
                            break;
                        case NoteType.Right:
                            noteImage.rectTransform.rotation = Quaternion.Euler(0, 0, 270); // Z축 270도 회전
                            break;
                    }
                }
                //Debug.Log("생성된 버튼 리스트에 추가됨: " + noteButton.name + " 타입: " + noteButton.noteType);
            }
            else
            {
                //Debug.LogError("NoteButton 컴포넌트를 찾을 수 없습니다.");
            }
            // 버튼 클릭 이벤트를 설정
            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => OnButtonClick(noteButton));
            }
            else
            {
                //Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            //Debug.LogError("Button RectTransform not found on prefab.");
        }
    }

    Debug.Log("모든 버튼이 생성되었습니다. 총 버튼 수: " + generatedButtons.Count);
    OnButtonsGenerated?.Invoke(); // 버튼 생성 완료 이벤트 호출
    Debug.Log("OnButtonsGenerated 이벤트 호출됨");
}

    void OnButtonClick(NoteButton noteButton)
    {
        Debug.Log("Generated Button clicked: " + noteButton.noteType);
        // 원하는 동작을 추가할 수 있습니다.
    }
}