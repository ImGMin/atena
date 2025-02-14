using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NicknameInput : MonoBehaviour
{
    public GameObject NicknameInputPanel; // 닉네임 입력 창
    public GameObject NicknameConfirm; // 닉네임 입력 확인 창
    public TMP_InputField inputField; // 닉네임 입력 필드
    public TextMeshProUGUI ConfirmText; // 확인창 텍스트
    public GameObject SelectCharacter; //캐릭터 선택 창창
    private string SavedNickname; // 저장된 닉네임

    void Start()
    {
        // 시작 시 닉네임 입력창 활성화, 확인창 비활성화
        NicknameInputPanel.SetActive(true);
        NicknameConfirm.SetActive(false);
    }

    // 닉네임 입력창의 확인 버튼 클릭 시 호출
    public void ConfirmButtonClick()
    {
        string inputText = inputField.text; // 입력된 텍스트 가져오기

        if (!string.IsNullOrEmpty(inputText))
        {
            SavedNickname = inputText;

            // 확인창 텍스트 업데이트
            ConfirmText.text = $"‘{SavedNickname}’로 진행하시겠습니까?";

            // 패널 전환
            NicknameInputPanel.SetActive(false);
            NicknameConfirm.SetActive(true);
        }
        else
        {
            Debug.Log("닉네임을 입력해주세요.");
        }
    }

    // 확인창의 확인 버튼 클릭 시 호출
    public void SaveName()
    {
        if (!string.IsNullOrEmpty(SavedNickname))
        {
            Debug.Log($"저장된 닉네임: {SavedNickname}");

            // 닉네임 저장 처리 (예: PlayerPrefs)
            PlayerPrefs.SetString("Nickname", SavedNickname);
            PlayerPrefs.Save();
            //컨펌 패널 비활성화
            NicknameConfirm.SetActive(false);

            // 캐릭터 선택 창 활성화
            SelectCharacter.SetActive(true);
        }
        else
        {
            Debug.Log("닉네임이 비어 있습니다.");
        }
    }

    // 확인창의 취소 버튼 클릭 시 호출
    public void CancelButtonClick()
    {
        // 확인창 비활성화, 닉네임 입력창 활성화
        NicknameConfirm.SetActive(false);
        NicknameInputPanel.SetActive(true);

        Debug.Log("닉네임 입력으로 돌아갑니다.");
    }
}
