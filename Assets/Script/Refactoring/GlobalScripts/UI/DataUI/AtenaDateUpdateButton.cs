using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtenaDateUpdateButton : MonoBehaviour
{
    private BaseTimeManager timeManager;

    private Button button;

    [SerializeField]
    private float scale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        timeManager = FindObjectOfType<BaseTimeManager>();
    }

    private void OnEnable()
    {
        if (button == null)
            button = GetComponent<Button>();
        
        button.onClick.AddListener(SetTimeScale);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SetTimeScale);
    }

    private void SetTimeScale()
    {
        if (timeManager != null)
        {
            timeManager.SetTimeScale(scale);
        }
    }
}
