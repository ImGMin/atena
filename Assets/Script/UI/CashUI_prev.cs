using UnityEngine;
using TMPro;

public class CashUI_prev : MonoBehaviour
{
    private TMP_Text thisText;

    [SerializeField]
    private string format = "{0}";

    [SerializeField]
    private bool useCommaSep = true;

    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        UpdateText(GameManager_prev.Instance.gameData.cash);
    }

    private void OnEnable()
    {
        GameManager_prev.Instance.gameData.OnCashChanged += UpdateText;
        UpdateText(GameManager_prev.Instance.gameData.cash);
    }

    private void OnDisable()
    {
        GameManager_prev.Instance.gameData.OnCashChanged -= UpdateText;
    }

    private void UpdateText(int newText)
    {
        if (useCommaSep)
        {
            thisText.text = string.Format(format, newText.ToString("N0"));
        }
        else
        {
            thisText.text = string.Format(format, newText.ToString());
        }
    }
}
