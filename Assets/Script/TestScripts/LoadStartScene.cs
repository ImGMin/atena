using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStartScene : MonoBehaviour
{
    public void StartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ExpUp()
    {
        GameManager.Instance.ChangeValue(exp: 5);
        GameManager.Instance.SaveGameData();
    }

    public void DayUp()
    {
        GameManager.Instance.ChangeValue(curTime: 1);
        GameManager.Instance.SaveGameData();
    }
    

    public void Init()
    {
        GameManager.Instance.InitGameData();
        GameManager.Instance.SaveGameData();
    }
}
