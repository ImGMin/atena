using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class testbuttonsccript : MonoBehaviour
{
    public Button testButton;
    public GameObject minigame2Ob;
    void Start()
    {
        testButton = GetComponent<Button>();
        testButton.onClick.AddListener(onButtonClick);
    }

    void Update()
    {
        
    }

    void onButtonClick()
    {
        Debug.Log("입력");
        minigame2Ob.SetActive(true);
    }
}
