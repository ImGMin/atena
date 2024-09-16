using UnityEngine;
using TMPro;

public class GameDataUI : MonoBehaviour
{
    protected TMP_Text thisText;

    [SerializeField]
    protected string format = "{0}";

    [SerializeField]
    protected int resourceIndex;

    protected virtual void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        GameManager.Instance.gameData.OnValueChanged += OnValueChanged;
    }

    protected virtual void Start() 
    {
        UpdateText((int)GameManager.Instance.gameData[resourceIndex]);
    }

    protected virtual void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.gameData.OnValueChanged -= OnValueChanged;
    }

    protected virtual void OnValueChanged(int index, int newValue)
    {
        if (index == resourceIndex)
        {
            UpdateText(newValue);
        }
    }

    protected virtual void UpdateText(int newText)
    {
        thisText.text = string.Format(format, newText);
    }
}
