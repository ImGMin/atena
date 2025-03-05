using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfflineSceneChangeManager : BaseSceneManager
{
    public void Exit()
    {
        GameManager.Instance.atenaDate.hour = 8f; 
        GameManager.Instance.SaveAllData();
        SceneManager.LoadScene(nextSceneName);
    }
}
