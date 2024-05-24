using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("SampleScene");
        }

        //테스트 씬 이동
        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene("TestScene");
        }
    }
}
