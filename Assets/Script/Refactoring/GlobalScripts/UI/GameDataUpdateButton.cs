using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataUpdateButton : MonoBehaviour
{
    [SerializeField]
    protected int resourceIndex;

    [SerializeField]
    protected int value;

    public void UpdateValue()
    {
        GameManager.Instance.ChangeValue(resourceIndex, value);
    }
}
