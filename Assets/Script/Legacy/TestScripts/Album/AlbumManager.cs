using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class AlbumManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform imagesParent; // 이미지들이 포함된 부모 오브젝트
    public Button uploadButton;    // 업로드 버튼
    public Button testButton;      // 이미지 생성 테스트 버튼
    public GameObject imageClickObject; // ImageClick 오브젝트
    public GameObject imagePrefab; // 이미지 프리팹
    public TextMeshProUGUI imageDataText; // 데이터를 표시할 UI 텍스트

    [Header("Image Management")]
    public List<Sprite> spriteList = new List<Sprite>(); // 이미지에 사용할 Sprite 리스트
    private int currentSpriteIndex = 0; // Sprite 리스트의 현재 인덱스

    private Dictionary<Image, ImageData> imageDataDict = new Dictionary<Image, ImageData>(); // 이미지 데이터 관리
    private Image selectedImage; // 선택된 이미지

    void Start()
    {
        // Test 버튼 클릭 이벤트 연결
        if (testButton != null)
        {
            testButton.onClick.AddListener(CreateNewImage);
        }

        // 업로드 버튼 클릭 이벤트 연결
        if (uploadButton != null)
        {
            uploadButton.onClick.AddListener(UploadImage);
        }
    }

    // 새로운 이미지 생성
    public void CreateNewImage()
    {
        if (imagePrefab == null || spriteList.Count == 0)
        {
            Debug.LogWarning("이미지 프리팹 또는 Sprite 리스트가 설정되지 않았습니다.");
            return;
        }

        // 이미지 프리팹을 생성하고 부모 오브젝트에 추가
        GameObject newImageObj = Instantiate(imagePrefab, imagesParent);
        Image image = newImageObj.GetComponent<Image>();

        if (image != null)
        {
            // Sprite 리스트에서 순서대로 이미지 할당
            Sprite assignedSprite = spriteList[currentSpriteIndex];
            image.sprite = assignedSprite;

            // 다음 인덱스로 이동 (리스트 끝에 도달하면 처음으로 돌아감)
            currentSpriteIndex = (currentSpriteIndex + 1) % spriteList.Count;

            // 고유한 데이터 생성
            ImageData newData = new ImageData(
                $"Image_{imagesParent.childCount}",
                assignedSprite,
                Random.Range(10, 100),   // 경험치 랜덤값
                Random.Range(0, 10),    // 평판 랜덤값
                Random.Range(0, 10),   // 친구 수 랜덤값
                Random.Range(100, 1000),// 돈 랜덤값
                Random.Range(1, 10)     // 성장도 랜덤값
            );

            // 데이터 등록
            imageDataDict[image] = newData;

            // 클릭 이벤트 연결
            Button button = newImageObj.GetComponent<Button>() ?? newImageObj.AddComponent<Button>();
            button.onClick.AddListener(() => SelectImage(image));

            Debug.Log($"이미지 생성: {newData.imageName}");
        }
    }

    // 이미지 선택
    public void SelectImage(Image image)
    {
        selectedImage = image; // 선택된 이미지 저장
        Debug.Log($"이미지 선택: {imageDataDict[image].imageName}");

        if (imageClickObject != null)
        {
            imageClickObject.SetActive(true); // ImageClick 오브젝트 활성화
        }

        UpdateImageData(); // 데이터 업데이트
    }

    // 이미지 데이터 업데이트
    private void UpdateImageData()
    {
        if (imageDataText != null && selectedImage != null && imageDataDict.ContainsKey(selectedImage))
        {
            ImageData data = imageDataDict[selectedImage];
            imageDataText.text = $"이름: {data.imageName}\n" +
                                 $"경험치: {data.exp}\n" +
                                 $"평판: {data.reputation}\n" +
                                 $"돈: {data.money}\n" +
                                 $"성장도: {data.atenaGrowth}";
        }
    }

    // 이미지 업로드
    public void UploadImage()
    {
        if (selectedImage == null)
        {
            Debug.LogWarning("선택된 이미지가 없습니다!");
            return;
        }

        if (!imageDataDict.ContainsKey(selectedImage))
        {
            Debug.LogWarning("선택된 이미지의 데이터가 없습니다!");
            return;
        }

        ImageData data = imageDataDict[selectedImage];

        if (data.isUploaded)
        {
            Debug.Log($"{data.imageName} 이미 업로드된 이미지입니다.");
            return;
        }

        // 업로드 상태 변경 및 처리
        data.isUploaded = true;
        Debug.Log($"{data.imageName} 업로드 완료!");
        // 업로드된 이미지를 업로드되지 않은 이미지들 뒤로 위치시킴
            MoveImageToBack(selectedImage);

        // 업로드된 이미지를 시각적으로 구분 (밝기 감소)
        DimImage(selectedImage);

        // ImageClick 오브젝트 비활성화
        if (imageClickObject != null)
        {
            imageClickObject.SetActive(false);
        }
    }

        void MoveImageToBack(Image image)
    {
        // 이미지를 업로드되지 않은 이미지들 뒤로 위치시킴
        image.transform.SetSiblingIndex(imagesParent.childCount - 1);  // 가장 뒤로 배치

        // 밝기 낮추기
        DimImage(image);
    }
    
    // 이미지 밝기 낮추기
    private void DimImage(Image image)
    {
        Color currentColor = image.color;
        image.color = new Color(currentColor.r * 0.5f, currentColor.g * 0.5f, currentColor.b * 0.5f); // 밝기 50% 감소
    }
}