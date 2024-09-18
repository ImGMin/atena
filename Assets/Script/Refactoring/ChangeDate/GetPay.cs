using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if ((GameManager.Instance.atenaDate.day-1)%5 == 0)
        {
            GameManager.Instance.ChangeValue("cash", GameManager.Instance.payData.pay);
            GameManager.Instance.payData.pay = 0;
            GameManager.Instance.SaveGameData("WeekData");
        }
    }
}
