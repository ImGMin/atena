using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTestScene : MonoBehaviour
{
    [SerializeField]
    private bool TestKey;

    // Update is called once per frame
    void Update()
    {
        if (TestKey && Input.GetKeyUp(KeyCode.T))
        {
            SceneManager.LoadScene("NewTestScene");
        }
    }
}
