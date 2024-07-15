using System.Collections;
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

    private Dictionary<NoteType, List<NoteButton>> noteButtonMapping = new Dictionary<NoteType, List<NoteButton>>();

    void Start()
    {
        if (buttonGenerator == null)
        {
            Debug.LogError("Button Generator is not assigned.");
            return;
        }

        AssignGeneratedButtonsToNotes();
        //buttonGenerator.OnButtonsGenerated += AssignGeneratedButtonsToNotes; // 버튼 생성 완료 이벤트에 리스너 추가
        AssignNoteButtonActions();
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
        // Initialize the dictionary
        foreach (NoteType noteType in System.Enum.GetValues(typeof(NoteType)))
        {
            Debug.Log(noteType.ToString());
            noteButtonMapping[noteType] = new List<NoteButton>();
            Debug.Log(noteButtonMapping);
        }

        // Shuffle the generated buttons list
        List<NoteButton> shuffledButtons = new List<NoteButton>(buttonGenerator.generatedButtons);
        for (int i = 0; i < shuffledButtons.Count; i++)
        {
            NoteButton temp = shuffledButtons[i];
            int randomIndex = Random.Range(i, shuffledButtons.Count);
            shuffledButtons[i] = shuffledButtons[randomIndex];
            shuffledButtons[randomIndex] = temp;
        }


        // Assign shuffled buttons to note types
        for (int i = 0; i < shuffledButtons.Count; i++)
        {
            NoteType noteType = (NoteType)(i % 4); // Cycle through the NoteTypes
            noteButtonMapping[noteType].Add(shuffledButtons[i]);
        }

        // Debug output
        foreach (var entry in noteButtonMapping)
        {
            Debug.Log(entry.Key + " has " + entry.Value.Count + " buttons assigned.");
        }
    }

    void OnNoteButtonClick(NoteType noteType)
    {
        Debug.Log(noteType.ToString() + " Note Button clicked");

        if (!noteButtonMapping.ContainsKey(noteType))
        {
            Debug.LogError("No buttons assigned for NoteType: " + noteType);
            return;
        }

        // 각 노트 타입에 따라 10개의 버튼과 상호작용하도록 구현
        foreach (var button in noteButtonMapping[noteType])
        {
            Debug.Log("Button with NoteType " + noteType + " is linked to position: " + button.transform.position);
        }
    }

    public enum NoteType
    {
        Up,
        Down,
        Left,
        Right
    }
}




//-87 -131
//-44 -89
//-2 -69
//27 -39
//68 -13
//96 17
//59 56
//20 94
//-35 121 
//-87 146