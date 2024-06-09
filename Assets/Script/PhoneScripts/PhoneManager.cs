using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    public GameObject Home;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //폰 관련 버튼들 함수모음
    public void PhoneOn()
    {
        Home.SetActive(true);
    }

}
