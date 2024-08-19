using UnityEngine;
using UnityEngine.UI;

public class ReputationUIBar : MonoBehaviour
{
    private Image thisImage;

    private void Awake()
    {
        thisImage = GetComponent<Image>();
        UpdateImage(GameManager_prev.Instance.gameData.reputation);
    }

    private void OnEnable()
    {
        GameManager_prev.Instance.gameData.OnReputationChanged += UpdateImage;
        UpdateImage(GameManager_prev.Instance.gameData.reputation);
    }

    private void OnDisable()
    {
        GameManager_prev.Instance.gameData.OnReputationChanged -= UpdateImage;
    }

    private void UpdateImage(int newValue)
    {
        thisImage.fillAmount = (newValue+100f)/200f;
    }
}
