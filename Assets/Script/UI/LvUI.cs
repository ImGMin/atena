using UnityEngine;
using TMPro;

public class LvUI : MonoBehaviour
{
    private TMP_Text LvText;

    private void Awake()
    {
        LvText = GetComponent<TMP_Text>();
        UpdateLvText(GameManager.Instance.gameData.level);
    }

    private void OnEnable()
    {
        GameManager.Instance.gameData.OnLvChanged += UpdateLvText;
    }

    private void OnDisable()
    {
        GameManager.Instance.gameData.OnLvChanged -= UpdateLvText;
    }

    private void UpdateLvText(int newLv)
    {
        LvText.text = $"·¹º§ : {newLv}";
    }
}
