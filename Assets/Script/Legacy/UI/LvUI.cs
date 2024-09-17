using UnityEngine;
using TMPro;

public class LvUI : MonoBehaviour
{
    private TMP_Text thisText;

    [SerializeField]
    private string format = "{0}";

    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        UpdateText(GameManager_prev.Instance.gameData.level);
    }

    private void OnEnable()
    {
        GameManager_prev.Instance.gameData.OnLvChanged += UpdateText;
        UpdateText(GameManager_prev.Instance.gameData.level);
    }

    private void OnDisable()
    {
        GameManager_prev.Instance.gameData.OnLvChanged -= UpdateText;
    }

    private void UpdateText(int newText)
    {
        thisText.text = string.Format(format, newText);
    }
}
