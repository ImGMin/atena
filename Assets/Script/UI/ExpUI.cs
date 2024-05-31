using UnityEngine;
using TMPro;

public class ExpUI : MonoBehaviour
{
    private TMP_Text ExpText;

    private void Awake()
    {
        ExpText = GetComponent<TMP_Text>();
        UpdateExpText(GameManager.Instance.gameData.exp);
    }

    private void OnEnable()
    {
        GameManager.Instance.gameData.OnExpChanged += UpdateExpText;
    }

    private void OnDisable()
    {
        GameManager.Instance.gameData.OnExpChanged -= UpdateExpText;
    }

    private void UpdateExpText(int newExp)
    {
        ExpText.text = $"°æÇèÄ¡ : {newExp}";
    }
}
