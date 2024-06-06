using UnityEngine;
using TMPro;

public class ExpUI : MonoBehaviour
{
    private TMP_Text thisText;

    [SerializeField]
    private string format = "{0}";

    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        UpdateText(GameManager.Instance.gameData.exp);
    }

    private void OnEnable()
    {
        GameManager.Instance.gameData.OnExpChanged += UpdateText;
    }

    private void OnDisable()
    {
        GameManager.Instance.gameData.OnExpChanged -= UpdateText;
    }

    private void UpdateText(int newText)
    {
        thisText.text = string.Format(format, newText);
    }
}
