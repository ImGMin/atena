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
    public BGMManager bgmManager; // BGMManager 참조

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

        private void Start()
    {
        // BGMManager 자동 찾기
        if (bgmManager == null)
        {
            bgmManager = FindObjectOfType<BGMManager>();
        }

        if (bgmManager == null || bgmManager.audioSource == null)
        {
            Debug.LogWarning("BGMManager 또는 AudioSource를 찾을 수 없습니다!");
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
        ApplyVolume();
        Debug.Log("Master Volume 변경됨: " + Volume);

    }
    public void SetbgmVolume(float volume)
    {
        bgmVolume = volume;
        Debug.Log("BGM 볼륨 변경됨: " + bgmVolume); // 디버깅 로그 추가
        if (bgmManager != null && bgmManager.audioSource != null)
        {
            bgmManager.audioSource.volume = bgmVolume;
            Debug.Log("AudioSource 볼륨 적용됨: " + bgmManager.audioSource.volume); // 로그 추가
        }
        else
        {
            Debug.LogWarning("BGMManager 또는 AudioSource가 설정되지 않았습니다!");
        }
    }
    public void SeteffectVolume(float volume)
    {
        effectVolume = volume;
    }

    private void ApplyVolume()
    {
        // 🎵 전체 볼륨 * 개별 볼륨을 적용
        if (bgmManager != null && bgmManager.audioSource != null)
        {
            bgmManager.audioSource.volume = Volume * bgmVolume;
            Debug.Log("BGM 최종 적용 볼륨: " + bgmManager.audioSource.volume);
        }

        // 여기에 효과음 오디오 소스도 추가 가능
    }
}