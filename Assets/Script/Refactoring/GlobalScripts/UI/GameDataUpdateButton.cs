using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataUpdateButton : MonoBehaviour
{
    [SerializeField]
    protected int resourceIndex;

    [SerializeField]
    protected int value;

    private Button button;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(UpdateValue);
    }

    public void UpdateValue()
    {
        GameManager.Instance.ChangeValue(resourceIndex, value);
    }
}
