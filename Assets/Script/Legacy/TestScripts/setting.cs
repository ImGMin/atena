using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setting : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider bgmVolumeSlider;
    public Slider effectVolumeSlider;
    public BGMManager bGMManager;
    private void Start()
    {
        Slider[] allSlider = Resources.FindObjectsOfTypeAll<Slider>();


        volumeSlider.value = SoundManager.Instance.Volume;
        bgmVolumeSlider.value = SoundManager.Instance.bgmVolume;
        effectVolumeSlider.value = SoundManager.Instance.effectVolume;


        volumeSlider.onValueChanged.AddListener(SetVolume);
        bgmVolumeSlider.onValueChanged.AddListener(SetbgmVolume);
        effectVolumeSlider.onValueChanged.AddListener(SeteffectVolume);
    }

    public void SetVolume(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }

    public void SetbgmVolume(float value)
    {
        Debug.Log("슬라이더 값: " + value); // 디버깅 로그 추가
        SoundManager.Instance.SetbgmVolume(value);
    }

    public void SeteffectVolume(float value)
    {
        SoundManager.Instance.SeteffectVolume(value);
    }
}