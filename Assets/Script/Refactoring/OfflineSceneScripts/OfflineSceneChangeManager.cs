using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineSceneChangeManager : BaseSceneManager
{
    [SerializeField]
    GameObject miniGame1;

    protected override void Update()
    {
        if (GameManager.Instance.atenaDate.hour > 0.3f)
        {
            miniGame1.SetActive(true);
        }

        if (GameManager.Instance.atenaDate.hour >= 8f)
        {
            GameManager.Instance.SaveAllData();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
