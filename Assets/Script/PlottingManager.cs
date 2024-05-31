using UnityEngine;
using TMPro;

public class PlottingManager : MonoBehaviour
{
    private static PlottingManager instance;
    public static PlottingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlottingManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("PlottingManager");
                    instance = go.AddComponent<PlottingManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
