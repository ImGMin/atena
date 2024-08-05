using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageController : MonoBehaviour
{
    public List<Image> images; // 사용할 이미지 리스트

    void Start()
    {
        // 모든 이미지 초기 비활성화
        foreach (var img in images)
        {
            img.enabled = false;
        }
    }

    // 특정 이미지 이름을 받아 해당 이미지를 일정 시간 동안 표시
    public IEnumerator ShowImageForSeconds(string imageName, float seconds)
    {
        // images 리스트에서 이름이 imageName과 일치하는 이미지 찾기
        Image selectedImage = images.Find(img => img.name == imageName);
        if (selectedImage != null)
        {
            selectedImage.enabled = true; // 이미지 활성화
            yield return new WaitForSeconds(seconds); // 지정된 시간 동안 대기
            selectedImage.enabled = false; // 이미지 비활성화
        }
        else
        {
            Debug.LogWarning($"Image with name {imageName} not found.");
        }
    }
}
