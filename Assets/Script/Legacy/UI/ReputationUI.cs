using UnityEngine;
using TMPro;

public class ReputationUI : MonoBehaviour
{
    private TMP_Text thisText;

    [SerializeField]
    private string format = "{0}";

    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        UpdateText(GameManager_prev.Instance.gameData.reputation);
    }

    private void OnEnable()
    {
        GameManager_prev.Instance.gameData.OnReputationChanged += UpdateText;
        UpdateText(GameManager_prev.Instance.gameData.reputation);
    }

    private void OnDisable()
    {
        GameManager_prev.Instance.gameData.OnReputationChanged -= UpdateText;
    }

    private void UpdateText(int newText)
    {
        thisText.text = string.Format(format, newText);
    }
}
