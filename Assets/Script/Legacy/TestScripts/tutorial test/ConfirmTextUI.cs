using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConfirmTextUI : MonoBehaviour
{
    // 패널과 UI 요소
    public GameObject NicknameConfirm; // 컨펌 패널
    public TextMeshProUGUI ConfirmText; // 컨펌 텍스트
    public Button CancelButton; // 취소 버튼
    public Button SetButton; // 확인 버튼

    private string SavedNickname; // 저장된 닉네임

    void Start()
    {
        // 컨펌 패널 비활성화
        NicknameConfirm.SetActive(false);

        // 버튼 클릭 이벤트 연결
        CancelButton.onClick.AddListener(OnCancel);
        //SetButton.onClick.AddListener(OnConfirm);
    }

    // 닉네임 입력 후 확인 버튼을 눌렀을 때 호출
    public void ShowNicknameConfirm(string nickname)
    {
        SavedNickname = nickname; // 입력된 닉네임 저장
        ConfirmText.text = $"‘{SavedNickname}’로 진행하시겠습니까?"; // 텍스트 업데이트

        // 컨펌 패널 활성화
        NicknameConfirm.SetActive(true);
    }

    // 취소 버튼 클릭 이벤트
    public void OnCancel()
    {
        // 컨펌 패널 비활성화
        NicknameConfirm.SetActive(false);

        // 필요하면 닉네임 입력 패널 활성화 코드 추가
        Debug.Log("취소되었습니다.");
    }
}
