using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    public void StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ExpUp()
    {
        GameManager.Instance.ChangeValue(exp: 5);
    }

    public void DayUp()
    {
        GameManager.Instance.ChangeValue(curTime: 1);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    

    public void Init()
    {
        GameManager.Instance.InitGameData();
    }
}
