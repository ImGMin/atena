using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class AlbumManager : MonoBehaviour
{
    public Transform imagesParent;       // 이미지들이 포함된 부모 오브젝트
    public Button uploadButton;          // 업로드 버튼
    public Button testButton;            // 이미지 생성 테스트 버튼
    public Image selectedImage;          // 선택된 이미지
    public GameObject imageClickObject;  // ImageClick 오브젝트
    public GameObject imagePrefab;       // 이미지 프리팹
    public TextMeshProUGUI imageDataText; // 데이터를 표시할 UI 텍스트 
    

    private Dictionary<Image, ImageData> imageDataDict = new Dictionary<Image, ImageData>(); // 이미지 데이터 관리

    void Start()
    {
        // 테스트 버튼 클릭 이벤트 연결
        if (testButton != null)
        {
            testButton.onClick.AddListener(CreateNewImage);
        }

        // 기존 자식 오브젝트에 버튼 이벤트 자동 설정
        foreach (Transform child in imagesParent)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                AddButtonEvents(image);
                AddDefaultImageData(image);
            }
        }

        // 업로드 버튼 클릭 이벤트 연결
        if (uploadButton != null)
        {
            uploadButton.onClick.AddListener(UploadImage);
        }
    }

    private void AddButtonEvents(Image image)
    {
        Button button = image.GetComponent<Button>();
        if (button == null)
        {
            button = image.gameObject.AddComponent<Button>();
        }

        button.onClick.AddListener(() => SelectImage(image));
    }

    private void AddDefaultImageData(Image image)
    {
        imageDataDict[image] = new ImageData(
            exp: Random.Range(0, 100),
            friends: Random.Range(0, 50),
            reputation: Random.Range(0, 30),
            atenaGrowth: Random.Range(0, 20),
            isUploaded: false,
            imageSprite: image.sprite
        );
    }

    public void CreateNewImage()
    {
        if (imagePrefab == null)
        {
            Debug.LogWarning("이미지 프리팹이 설정되지 않았습니다.");
            return;
        }

        GameObject newImageObj = Instantiate(imagePrefab, imagesParent);
        newImageObj.name = "Image_" + imagesParent.childCount;

        Image image = newImageObj.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogWarning("프리팹에 Image 컴포넌트가 없습니다.");
            return;
        }

        AddButtonEvents(image);

        // 새 이미지에 기본 데이터 추가
        AddDefaultImageData(image);

        Debug.Log(newImageObj.name + " 이미지가 생성되었습니다.");
    }

    public void SelectImage(Image image)
    {
        selectedImage = image;
        Debug.Log("이미지가 선택되었습니다: " + selectedImage.name);

        if (imageClickObject != null)
        {
            imageClickObject.SetActive(true);
        }

        UpdateImageData();
    }

    private void UpdateImageData()
    {
        if (imageDataText != null && selectedImage != null)
        {
            if (imageDataDict.TryGetValue(selectedImage, out ImageData data))
            {
                imageDataText.text = $"경험치: {data.exp} 친구: {data.friends} 평판: {data.reputation} 성장도: {data.atenaGrowth}";
            }
            else
            {
                Debug.LogWarning("선택된 이미지에 대한 데이터가 없습니다.");
            }
        }
    }

    public void UploadImage()
    {
        if (selectedImage != null)
        {
            if (imageDataDict.TryGetValue(selectedImage, out ImageData data))
            {
                if (data.isUploaded)
                {
                    Debug.Log(selectedImage.name + " 이미 업로드된 이미지입니다.");
                    return;
                }

                // 업로드 상태 업데이트
                data.isUploaded = true;

                // 업로드된 이미지를 뒤로 보내기
                MoveImageToBack(selectedImage);

                Debug.Log(selectedImage.name + " 업로드 완료!");
            }
            else
            {
                Debug.LogWarning("선택된 이미지에 대한 데이터가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("선택된 이미지가 없습니다!");
        }
    }

    private void MoveImageToBack(Image image)
    {
        image.transform.SetSiblingIndex(imagesParent.childCount - 1);
        DimImage(image);
    }

    private void DimImage(Image image)
    {
        Color currentColor = image.color;
        image.color = new Color(currentColor.r * 0.5f, currentColor.g * 0.5f, currentColor.b * 0.5f); // 밝기 50% 감소
    }
}
