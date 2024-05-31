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

    public void Init()
    {
        GameManager.Instance.InitGameData();
        GameManager.Instance.SaveGameData();
        Debug.Log(GameManager.Instance.gameData.level.ToString());
    }
}
