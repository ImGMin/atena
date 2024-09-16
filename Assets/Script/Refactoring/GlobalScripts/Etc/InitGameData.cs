using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitGameData : MonoBehaviour
{
    private Button button;

    private void OnEnable()
    {   
        if (button == null)
            button = GetComponent<Button>();
        
        button.onClick.AddListener(UpdateValue);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(UpdateValue);
    }

    public void UpdateValue()
    {
        GameManager.Instance.InitGameData();
    }
}
