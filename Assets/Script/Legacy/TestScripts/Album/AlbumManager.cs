using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 
using TMPro;

public class AlbumManager : MonoBehaviour
{
    public Transform imagesParent; // 이미지들이 포함된 부모 오브젝트
    public Button uploadButton;    // 업로드 버튼
    public Button testButton;      //이미지 생성 테스트버튼
    public Image selectedImage;    // 선택된 이미지
    public GameObject imageClickObject; // ImageClick 오브젝트
    //private bool isUploaded = false; // 업로드 상태 (기본값: false)
    public GameObject imagePrefab; //이미지 프리펩
    private Dictionary<Image, bool> imageUploadStatus = new Dictionary<Image, bool>(); // 업로드 상태를 관리하는 딕셔너리


public TextMeshProUGUI imageDataText; // 데이터를 표시할 UI 텍스트

    //게임 데이터
    public int level { get; set; }
    public int exp { get; set; }
    public int energy { get; set; }
    public int friends { get; set; }
    public int cash { get; set; }
    public int reputation { get; set; }
    public int atenaGrowth { get; set; }


    void Start()
    {
                // testButton 클릭 이벤트 연결
        if (testButton != null)
        {
            testButton.onClick.AddListener(CreateNewImage); // 버튼 클릭 시 새 이미지 생성
        }

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

    Button button = newImageObj.GetComponent<Button>();
    if (button == null)
    {
        button = newImageObj.AddComponent<Button>();
    }

    button.onClick.AddListener(() => SelectImage(image));
    button.onClick.AddListener(() => ShowUploadStatus(image));

    // 업로드 상태 딕셔너리에 추가
    imageUploadStatus[image] = false;

    // 새로운 이미지를 업로드되지 않은 이미지들 중 가장 앞으로 삽입
    PlaceNewImage(image);

    Debug.Log(newImageObj.name + " 이미지가 생성되었습니다.");
}

// 새로운 이미지를 업로드되지 않은 이미지들 앞에 배치
private void PlaceNewImage(Image newImage)
{
    int insertIndex = 0;

    // 업로드되지 않은 이미지들만 탐색하여 가장 앞의 인덱스를 찾음
    foreach (Transform child in imagesParent)
    {
        Image childImage = child.GetComponent<Image>();
        if (childImage != null && !imageUploadStatus[childImage])
        {
            insertIndex = child.GetSiblingIndex();
            break;
        }
    }

    // 새로운 이미지를 해당 위치에 배치
    newImage.transform.SetSiblingIndex(insertIndex);
}



    public void SelectImage(Image image)
    {
        selectedImage = image; // 선택된 이미지 저장
        Debug.Log("이미지가 선택되었습니다: " + selectedImage.name);

        if (imageClickObject != null)
        {
            imageClickObject.SetActive(true); // ImageClick 오브젝트 활성화
        }
        
        UpdateImageData();  // 데이터 업데이트 (텍스트 표시)
    }


    private void UpdateImageData()
    {
        if (imageDataText != null)
        {
            // 데이터를 텍스트로 표시
            imageDataText.text = $"레벨: {level} 경험치: {exp} 에너지: {energy} 친구: {friends} 돈: {cash} 평판: {reputation} AtenaGrowth: {atenaGrowth}";
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

            //isUploaded = true; // 업로드 상태를 true로 설정
            imageUploadStatus[selectedImage] = true; // 해당 이미지의 업로드 상태를 true로 설정

            //Debug.Log(selectedImage.name + " 업로드 완료! 상태: " + isUploaded);

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
