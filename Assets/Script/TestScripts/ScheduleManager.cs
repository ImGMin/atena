using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeValue(cash: GameManager.Instance.earnings);
        GameManager.Instance.earnings = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
