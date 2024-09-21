using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkChecker : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject[] workAnimation = new GameObject[4];

    [SerializeField]
    BaseTimeManager timeManager;

    [SerializeField]
    private float animationTime = 5f;

    bool work = true;
    float workTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (isWork())
        {
            WorkAnimationPlayer();

            float TimeScale = workTime / animationTime * 30f;
            timeManager.SetTimeScale(TimeScale);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (work && GameManager.Instance.atenaDate.hour > workTime)
        {
            timeManager.SetTimeScale(1f);
            work = false;
        }
    }

    bool isWork()
    {
        switch ((string)GameManager.Instance.workData[(GameManager.Instance.atenaDate.day - 1)%5])
        {
            case "Work1":
                GameManager.Instance.payData.pay += 60000;
                workTime = 6f;
                Debug.Log($"6시간 일하고 6만원 현재 주급 {GameManager.Instance.payData.pay}");
                break;

            case "Work2":
                GameManager.Instance.payData.pay += 32000;
                workTime = 4f;
                Debug.Log($"4시간 일하고 32000원 현재 주급 {GameManager.Instance.payData.pay}");
                break;

            case "Work3":
                GameManager.Instance.payData.pay += 9000;
                workTime = 3f;
                Debug.Log($"3시간 일하고 9천원 현재 주급 {GameManager.Instance.payData.pay}");
                break;

            default:
                work = false;
                Debug.Log("편하게 휴식");
                break;
        }

        return work;
    }

    void WorkAnimationPlayer()
    {
        int idx = Program.GetRandomIndices(2, 1)[0];

        GameObject ani = Instantiate(workAnimation[idx], canvas.transform);
        Destroy(ani, animationTime);
    }
}
