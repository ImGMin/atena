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

        //�׽�Ʈ �� �̵�
        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene("TestScene");
        }
    }
}
