using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextChangeEffect : GameDataUI
{
    [SerializeField]
    private GameObject text;

    private int prev;

    protected override void Awake()
    {
        GameManager.Instance.gameData.OnValueChanged += OnValueChanged;
        prev = (int)GameManager.Instance.gameData[resourceIndex];
    }
    protected override void UpdateText(int newText)
    {
        int diff = newText - prev;
        if (diff > 0)
        {
            text.GetComponent<TMP_Text>().text = $"+{diff}";
        }
        else if (diff < 0)
        {
            text.GetComponent<TMP_Text>().text = $"{diff}";
        }
        else
        {
            return;
        }

        Instantiate(text,transform);
        prev = newText;
    }
}
