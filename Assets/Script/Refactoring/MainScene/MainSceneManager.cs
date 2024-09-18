using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Work();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Work()
    {
        switch ((string)GameManager.Instance.workData[(GameManager.Instance.atenaDate.day - 1)%5])
        {
            case "Work1":
                GameManager.Instance.payData.pay += 60000;
                Debug.Log($"6시간 일하고 6만원 현재 주급 {GameManager.Instance.payData.pay}");
                break;

            case "Work2":
                GameManager.Instance.payData.pay += 32000;
                Debug.Log($"4시간 일하고 32000원 현재 주급 {GameManager.Instance.payData.pay}");
                break;

            case "Work3":
                GameManager.Instance.payData.pay += 9000;
                Debug.Log($"3시간 일하고 9천원 현재 주급 {GameManager.Instance.payData.pay}");
                break;
        }
    }
}
