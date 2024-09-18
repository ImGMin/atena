using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    public Image Image;
    public TMP_Text text;

    private float d = 12f;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Image.fillAmount = GameManager.Instance.atenaDate.hour / d;
        text.text = ((int)GameManager.Instance.atenaDate.hour).ToString();

        if (GameManager.Instance.atenaDate.hour > d)
        {
            Image.fillAmount = 1;
        }
    }
}
