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
        GameManager_prev.Instance.ChangeValue(exp: 5);
    }

    public void DayUp()
    {
        GameManager_prev.Instance.ChangeValue(curTime: 1);
    }

    public void HourUp()
    {
        GameManager_prev.Instance.gameData.curTime.hour++;
    }
    

    public void Init()
    {
        GameManager_prev.Instance.InitGameData();
    }
}
