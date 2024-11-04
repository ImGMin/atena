using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventEntryManager : MonoBehaviour
{
    [SerializeField] private Button panelButton;

    [SerializeField] private Button entryButton;

    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private TMP_Text cntText;
    [SerializeField] private TMP_Text costText;

    private int cnt = 1;
    [SerializeField] private int cost;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.isEventEntryDay.array[(GameManager.Instance.atenaDate.day - 1) % 5])
        {
            panelButton.interactable = true;
        }
        else
        {
            GameManager.Instance.isEventDay.array[(GameManager.Instance.atenaDate.day - 1) % 5] = 0;
            panelButton.interactable = false;
        }
        GameManager.Instance.SaveAllData();

        cntText.text = "1";
        costText.text = $"{cost}";
        downButton.interactable = false;

        upButton.onClick.AddListener(Up);
        downButton.onClick.AddListener(Down);
        entryButton.onClick.AddListener(Entry);
    }

    void Up()
    {
        cnt++;
        if ((cnt+1)*cost > GameManager.Instance.gameData.cash)
        {
            upButton.interactable = false;
        }
        cntText.text = $"{cnt}";
        costText.text = $"{cnt * cost}";
        if (downButton.interactable == false)
        {
            downButton.interactable = true;
        }
    }

    void Down()
    {
        cnt--;
        if (cnt == 1)
        {
            downButton.interactable = false;
        }
        cntText.text = $"{cnt}";
        costText.text = $"{cnt * cost}";
        if (upButton.interactable == false)
        {
            upButton.interactable = true;
        }
    }

    void Entry()
    {
        if (GameManager.Instance.isEventDay.array[(GameManager.Instance.atenaDate.day - 1) % 5] == 1)
        {
            Debug.Log("이미 당첨");
            return;
        }

        GameManager.Instance.ChangeValue("cash", -cnt * cost);

        int cut = GameManager.Instance.gameData.atenaGrowth / (GameManager.Instance.gameData.level * 10);
        if (cnt >= cut)
        {
            Cal(100f);
            return;
        }

        float p = (float)GameManager.Instance.gameData.energy / (cut - cnt) * 1000;
        Cal(p);
    }

    void Cal(float p)
    {
        if (Program.CheckProbability(p))
        {
            Debug.Log("당첨!");
            GameManager.Instance.isEventDay.array[(GameManager.Instance.atenaDate.day - 1)%5] = 1;
        }
        else
        {
            Debug.Log("실패..");
            GameManager.Instance.isEventDay.array[(GameManager.Instance.atenaDate.day - 1) % 5] = 0;
        }
    }
}
