using UnityEngine;
using UnityEngine.UI;

public class AudioSettings
{
    public float Volume;
    public float bgmVolume;
    public float effectVolume;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Range(0f, 1f)] public float Volume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float effectVolume = 1f;
    public static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType(typeof(SoundManager)) as SoundManager;

                if (_instance == null)
                {
                //     return null;
                //     GameObject singletonObject = new GameObject();
                //     _instance = singletonObject.AddComponent<SoundManager>();
                //     singletonObject.name = typeof(SoundManager).ToString();
                //     DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
    }
    public void SetbgmVolume(float volume)
    {
        bgmVolume = volume;
    }
    public void SeteffectVolume(float volume)
    {
        effectVolume = volume;
    }
}