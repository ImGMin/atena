using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineSceneChangeManager : BaseSceneManager
{
    protected override void Update()
    {
        if (GameManager.Instance.atenaDate.hour >= 8f)
        {
            GameManager.Instance.SaveAllData();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
