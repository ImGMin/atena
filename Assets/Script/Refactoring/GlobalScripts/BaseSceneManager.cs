using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneManager : MonoBehaviour
{
    protected int Today;

    [SerializeField]
    private string nextSceneName = "NewTestScene";

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Today = GameManager.Instance.atenaDate.day;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Today != GameManager.Instance.atenaDate.day)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}