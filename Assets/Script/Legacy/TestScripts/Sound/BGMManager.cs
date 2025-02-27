using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource;  // 배경음악 오디오 소스
    public AudioClip defaultBGM;      // 기본 배경음악
    public AudioClip specialBGM;      // 특정 오브젝트 활성화 시 나올 음악
    public GameObject minigamescreen;  // 특정 오브젝트 (활성화 여부 체크)

    private void Start()
    {
        // 기본 배경음악 재생
        PlayBGM(defaultBGM);
    }

    private void Update()
    {
        // 특정 오브젝트가 활성화되었는지 체크
        if (minigamescreen.activeSelf)
        {
            PlayBGM(specialBGM);
        }
        else
        {
            PlayBGM(defaultBGM);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
