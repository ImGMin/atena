using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().name == "StartScene")
        {
            SceneManager.LoadScene("ChangeDate");
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene("TestScene");
        }
    }
}
