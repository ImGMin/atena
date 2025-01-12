using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Dictionary 사용을 위한 네임스페이스 추가

public class AlbumManager : MonoBehaviour
{
    public Transform imagesParent; // 이미지들이 포함된 부모 오브젝트
    public Button uploadButton;    // 업로드 버튼
    public Image selectedImage;    // 선택된 이미지
    public GameObject imageClickObject; // ImageClick 오브젝트
    private bool isUploaded = false; // 업로드 상태 (기본값: false)

    // 업로드 상태를 관리하는 딕셔너리
    private Dictionary<Image, bool> imageUploadStatus = new Dictionary<Image, bool>();

    void Start()
    {
        // 모든 자식 오브젝트에서 버튼 이벤트를 자동으로 설정
        foreach (Transform child in imagesParent)
        {
            Image image = child.GetComponent<Image>();
            Button button = child.GetComponent<Button>();

            if (image != null)
            {
                // 버튼 컴포넌트가 없으면 추가
                if (button == null) button = child.gameObject.AddComponent<Button>();

                // 버튼 클릭 이벤트에 SelectImage() 연결
                button.onClick.AddListener(() => SelectImage(image));

                // 이미지 클릭 시 업로드 상태 확인
                button.onClick.AddListener(() => ShowUploadStatus(image));

                // 업로드 상태 딕셔너리에 이미지 추가
                imageUploadStatus[image] = false;  // 기본값은 업로드되지 않은 상태
            }
        }

        // 업로드 버튼 클릭 이벤트 연결
        uploadButton.onClick.AddListener(UploadImage);
    }

    public void SelectImage(Image image)
    {
        selectedImage = image; // 선택된 이미지 저장
        Debug.Log("이미지가 선택되었습니다: " + selectedImage.name);

        if (imageClickObject != null)
        {
            imageClickObject.SetActive(true); // ImageClick 오브젝트 활성화
        }
    }

    void UploadImage()
    {
        if (selectedImage != null)
        {
            // 업로드된 상태인 이미지를 다시 업로드하려고 할 때
            if (imageUploadStatus[selectedImage])
            {
                Debug.Log(selectedImage.name + " 이미 업로드된 이미지입니다.");
                return; // 업로드된 이미지는 다시 업로드하지 않음
            }

            isUploaded = true; // 업로드 상태를 true로 설정
            imageUploadStatus[selectedImage] = true; // 해당 이미지의 업로드 상태를 true로 설정

            Debug.Log(selectedImage.name + " 업로드 완료! 상태: " + isUploaded);

            // 업로드된 이미지를 업로드되지 않은 이미지들 뒤로 위치시킴
            MoveImageToBack(selectedImage);

            // ImageClick 오브젝트 비활성화
            if (imageClickObject != null)
            {
                imageClickObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("선택된 이미지가 없습니다!");
        }
    }

    void MoveImageToBack(Image image)
    {
        // 이미지를 업로드되지 않은 이미지들 뒤로 위치시킴
        image.transform.SetSiblingIndex(imagesParent.childCount - 1);  // 가장 뒤로 배치

        // 밝기 낮추기
        DimImage(image);
    }

    void DimImage(Image image)
    {
        // 밝기 낮추기 (어두운 색으로 처리)
        Color currentColor = image.color;
        image.color = new Color(currentColor.r * 0.5f, currentColor.g * 0.5f, currentColor.b * 0.5f); // 밝기 50% 감소
    }

    void RestoreImage(Image image)
    {
        // 원래 색으로 복원
        image.color = Color.white;
    }

    // 업로드 상태 확인 및 디버그 로그 출력
    void ShowUploadStatus(Image image)
    {
        // 업로드 상태 출력
        if (imageUploadStatus[image])
        {
            Debug.Log(image.name + " 업로드 상태: TRUE");
        }
        else
        {
            Debug.Log(image.name + " 업로드 상태: FALSE");
        }
    }
}
