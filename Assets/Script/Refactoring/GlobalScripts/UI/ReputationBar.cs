using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationBar : GameDataUI
{
    public Image Image;

    protected override void UpdateText(int newValue)
    {
        Image.fillAmount = (newValue + 100f) / 200f;
    }
}
