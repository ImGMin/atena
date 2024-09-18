using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDateSceneManager : BaseSceneManager
{
    [SerializeField]
    private string nextSceneName2 = "OfflineScene";

    [SerializeField]
    private string nextSceneName3 = "MainScene";

    float time;

    protected override void Start()
    {
        base.Start();
        time = 0f;
    }

    protected override void Update()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            bool isTuto = isFirstWeek();
            if ((isTuto && Today == 2) || (!isTuto && (Today - 1) % 5 == 0))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                if ((string)GameManager.Instance.situData[(Today-1)%5] == "Situ_02_01_01")
                {
                    SceneManager.LoadScene(nextSceneName2);
                }
                else
                {
                    SceneManager.LoadScene(nextSceneName3);
                }
            }
        }
    }

    bool isFirstWeek()
    {
        if (GameManager.Instance.atenaDate.year != 25)
            return false;

        if (GameManager.Instance.atenaDate.month != 1) 
            return false;

        if (GameManager.Instance.atenaDate.day > 5)
            return false;

        return true;
    }
}
