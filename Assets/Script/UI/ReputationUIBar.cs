using UnityEngine;
using UnityEngine.UI;

public class ReputationUIBar : MonoBehaviour
{
    private Image thisImage;

    private void Awake()
    {
        thisImage = GetComponent<Image>();
        UpdateImage(GameManager.Instance.gameData.reputation);
    }

    private void OnEnable()
    {
        GameManager.Instance.gameData.OnReputationChanged += UpdateImage;
        UpdateImage(GameManager.Instance.gameData.reputation);
    }

    private void OnDisable()
    {
        GameManager.Instance.gameData.OnReputationChanged -= UpdateImage;
    }

    private void UpdateImage(int newValue)
    {
        thisImage.fillAmount = (newValue+100f)/200f;
    }
}
