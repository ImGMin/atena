using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConfirmCharacterUI : MonoBehaviour
{
    public GameObject CharacterConfirmWindow; // 컨펌 패널
    public TextMeshProUGUI CharacterConfirmText; // 컨펌 텍스트
    public Button CharacterCancelButton; // 취소 버튼
    public Button CharacterSetButton; // 확인 버튼

    private SelectCharacter selectCharacter; // SelectCharacter 스크립트를 참조

    void Start()
    {
        // 취소 버튼 이벤트 등록
        if (CharacterCancelButton != null)
        {
            CharacterCancelButton.onClick.AddListener(OnCancel);
        }

        // 확인 버튼 이벤트 등록
        if (CharacterSetButton != null)
        {
            CharacterSetButton.onClick.AddListener(OnConfirm);
        }

        // SelectCharacter 스크립트 참조
        selectCharacter = FindObjectOfType<SelectCharacter>();
    }

    // 취소 버튼 클릭 이벤트
    public void OnCancel()
    {
        // 컨펌 패널 비활성화
        if (CharacterConfirmWindow != null)
        {
            CharacterConfirmWindow.SetActive(false);
        }

        // 캐릭터 선택 창 활성화
        if (selectCharacter != null && selectCharacter.SelectCharacterWindow != null)
        {
            selectCharacter.SelectCharacterWindow.SetActive(true);
        }

        Debug.Log("취소되었습니다.");
    }

    // 확인 버튼 클릭 이벤트
    public void OnConfirm()
    {
        if (selectCharacter != null)
        {
            selectCharacter.SaveName(); // SelectCharacter의 SaveName 메서드 호출
        }
    }
}
