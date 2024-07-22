using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteButton : MonoBehaviour
{
    public NoteType noteType;
    public TMP_Text buttonText; // 버튼의 텍스트 컴포넌트를 참조

    void Awake()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
    }
}


public enum NoteType
{
    Up,
    Down,
    Left,
    Right
}
