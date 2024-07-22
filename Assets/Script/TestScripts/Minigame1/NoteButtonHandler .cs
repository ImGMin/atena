using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteButtonHandler : MonoBehaviour
{
    public Button upNoteButton; // Upnote 버튼
    public Button downNoteButton; // Downnote 버튼
    public Button leftNoteButton; // Leftnote 버튼
    public Button rightNoteButton; // Rightnote 버튼
    public PatternButtonGenerator buttonGenerator; // PatternButtonGenerator 스크립트 참조
    public minigametimer sliderTimer; // minigametimer 스크립트 참조

    private Dictionary<NoteType, List<NoteButton>> noteButtonMapping = new Dictionary<NoteType, List<NoteButton>>();
    private List<NoteButton> orderedButtons = new List<NoteButton>(); // 순서대로 버튼을 저장할 리스트

    void Start()
    {
        if (buttonGenerator == null)
        {
            Debug.LogError("Button Generator가 할당되지 않았습니다.");
            return;
        }
        if (sliderTimer == null)
        {
            Debug.LogError("Slider Timer가 할당되지 않았습니다.");
            return;
        }
        Debug.Log("NoteButtonHandler: Start 호출됨");
        Debug.Log("Button Generator 할당됨: " + buttonGenerator);

        // 이벤트 구독 설정
        buttonGenerator.OnButtonsGenerated += AssignGeneratedButtonsToNotes;
        Debug.Log("OnButtonsGenerated 이벤트 구독 완료");

        AssignNoteButtonActions();

        // 강제 호출 (디버그용)
        AssignGeneratedButtonsToNotes();
    }

    void AssignNoteButtonActions()
    {
        if (upNoteButton != null)
            upNoteButton.onClick.AddListener(() => OnNoteButtonClick(NoteType.Up));

        if (downNoteButton != null)
            downNoteButton.onClick.AddListener(() => OnNoteButtonClick(NoteType.Down));

        if (leftNoteButton != null)
            leftNoteButton.onClick.AddListener(() => OnNoteButtonClick(NoteType.Left));

        if (rightNoteButton != null)
            rightNoteButton.onClick.AddListener(() => OnNoteButtonClick(NoteType.Right));
    }

    void AssignGeneratedButtonsToNotes()
    {
        Debug.Log("AssignGeneratedButtonsToNotes 호출됨");
        Debug.Log("buttonGenerator: " + buttonGenerator);
        Debug.Log("buttonGenerator.generatedButtons: " + buttonGenerator.generatedButtons);

        // Initialize the dictionary
        foreach (NoteType noteType in System.Enum.GetValues(typeof(NoteType)))
        {
            Debug.Log(noteType.ToString());
            noteButtonMapping[noteType] = new List<NoteButton>();
        }

        // generatedButtons 리스트가 null인지 확인
        if (buttonGenerator.generatedButtons == null)
        {
            Debug.LogError("generatedButtons 리스트가 null입니다.");
            return;
        }

        // 생성된 버튼 리스트가 비어있는지 확인
        if (buttonGenerator.generatedButtons.Count == 0)
        {
            Debug.LogError("생성된 버튼 리스트가 비어있습니다.");
            return;
        }

        // 생성된 버튼 리스트 출력
        foreach (var button in buttonGenerator.generatedButtons)
        {
            Debug.Log("생성된 버튼: " + button.name + " 타입: " + button.noteType);
        }

        // Shuffle the generated buttons list
        Debug.Log("Shuffling buttons");
        List<NoteButton> shuffledButtons = new List<NoteButton>(buttonGenerator.generatedButtons);
        for (int i = 0; i < shuffledButtons.Count; i++)
        {
            NoteButton temp = shuffledButtons[i];
            int randomIndex = Random.Range(i, shuffledButtons.Count);
            shuffledButtons[i] = shuffledButtons[randomIndex];
            shuffledButtons[randomIndex] = temp;
        }
        Debug.Log("Buttons shuffled");

        // Assign shuffled buttons to note types
        orderedButtons.Clear();
        for (int i = 0; i < shuffledButtons.Count; i++)
        {
            NoteType noteType = (NoteType)(i % 4); // Cycle through the NoteTypes
            noteButtonMapping[noteType].Add(shuffledButtons[i]);
            orderedButtons.Add(shuffledButtons[i]);
            Debug.Log("버튼 할당됨: " + shuffledButtons[i].name + " to " + noteType);
        }

        // 아래에 있는 노트부터 위로 정렬
        orderedButtons.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

        // Debug output
        foreach (var entry in noteButtonMapping)
        {
            Debug.Log(entry.Key + "에 " + entry.Value.Count + "개의 버튼이 할당되었습니다.");
        }
    }

    void OnNoteButtonClick(NoteType noteType)
    {
        Debug.Log(noteType.ToString() + " 노트 버튼이 클릭되었습니다");

        if (!noteButtonMapping.ContainsKey(noteType))
        {
            Debug.LogError("NoteType " + noteType + "에 할당된 버튼이 없습니다.");
            return;
        }

        // 현재 리스트에서 첫 번째 버튼이 맞는지 확인
        if (orderedButtons.Count > 0 && orderedButtons[0].noteType == noteType)
        {
            NoteButton firstButton = orderedButtons[0];
            orderedButtons.RemoveAt(0);
            noteButtonMapping[noteType].Remove(firstButton);
            Debug.Log("NoteType " + noteType + "의 버튼이 올바르게 클릭되었습니다: " + firstButton.transform.position);
            Destroy(firstButton.gameObject);
        }
        else
        {
            Debug.Log("잘못된 버튼이 클릭되었습니다.");
            sliderTimer.ReduceTime(5f); // 잘못된 버튼을 클릭할 때 5초 감소
        }
    }

    public enum noteType
    {
        Up,
        Down,
        Left,
        Right
    }
}