using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField]
    private string SceneName;

    protected Button Button;

    protected virtual void OnEnable()
    {
        if (Button == null)
            Button = GetComponent<Button>();

        Button.onClick.AddListener(LoadScene);
    }

    protected virtual void OnDisable()
    {
        Button.onClick.RemoveListener(LoadScene);
    }

    protected virtual void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }

}
