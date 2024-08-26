using UnityEngine;
using TMPro;

public class FriendUI : MonoBehaviour
{
    private TMP_Text thisText;

    [SerializeField]
    private string format = "{0}";

    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        UpdateText(GameManager_prev.Instance.gameData.friends);
    }

    private void OnEnable()
    {
        GameManager_prev.Instance.gameData.OnFriendChanged += UpdateText;
        UpdateText(GameManager_prev.Instance.gameData.friends);
    }

    private void OnDisable()
    {
        GameManager_prev.Instance.gameData.OnFriendChanged -= UpdateText;
    }

    private void UpdateText(int newText)
    {
        thisText.text = string.Format(format, newText);
    }
}
