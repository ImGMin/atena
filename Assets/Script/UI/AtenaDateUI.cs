using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class AtenaDateUI : MonoBehaviour
{
    private TMP_Text thisText;

    [SerializeField]
    private string format = "{0}";

    [SerializeField]
    private bool useFullYear = true;

    private List<string> weekday = new List<string>() { "월", "화", "수", "목", "금" };

    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
        UpdateText(GameManager_prev.Instance.gameData.curTime);
    }

    private void OnEnable()
    {
        GameManager_prev.Instance.gameData.OnAtenaDateChanged += UpdateText;
        UpdateText(GameManager_prev.Instance.gameData.curTime);
    }

    private void OnDisable()
    {
        GameManager_prev.Instance.gameData.OnAtenaDateChanged -= UpdateText;
    }

    private void UpdateText(AtenaDate_prev newText)
    {
        int formatYear;
        if (useFullYear)
        {
            formatYear = newText.year;
        }
        else
        {
            formatYear = newText.year % 100;
        }

        string processedFormat = format.Replace("\\n", "\n");

        thisText.text = string.Format(processedFormat, formatYear, newText.month, newText.day, weekday[newText.weekday]);
    }
}
