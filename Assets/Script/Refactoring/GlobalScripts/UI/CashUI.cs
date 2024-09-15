using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashUI : GameDataUI
{
    [SerializeField]
    private bool csv;

    protected override void UpdateText(int newText)
    {
        if (csv)
        {
            thisText.text = string.Format(format, newText.ToString("N0"));
        }
        else
        {
            thisText.text = string.Format(format, newText);
        }

    }
}
