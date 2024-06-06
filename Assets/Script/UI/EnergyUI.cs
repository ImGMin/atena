using UnityEngine;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    private TMP_Text thisText;

    [SerializeField]
    private string format = "{0}";

    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        UpdateText(GameManager.Instance.gameData.energy);
    }

    private void OnEnable()
    {
        GameManager.Instance.gameData.OnEnergyChanged += UpdateText;
    }

    private void OnDisable()
    {
        GameManager.Instance.gameData.OnEnergyChanged -= UpdateText;
    }

    private void UpdateText(int newText)
    {
        thisText.text = string.Format(format, newText);
    }
}
