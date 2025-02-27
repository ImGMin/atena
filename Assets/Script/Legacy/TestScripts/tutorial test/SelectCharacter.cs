using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectCharacter : MonoBehaviour
{
    public Button HanaProfile;
    public Button YehyunProfile;
    public Button JiseonProfile;
    public Button SinaProfile;
    public Button BoanProfile;

    public GameObject SelectCharacterWindow; // 캐릭터 선택 창
    public TextMeshProUGUI CharacterConfirmText; // 확인 창에서 표시할 텍스트
    public GameObject CharacterConfirmWindow; // 캐릭터 확인 창
    public GameObject NicknameInputPanel; // 닉네임 입력창 참조 추가
    private string selectedCharacterName; // 현재 선택된 캐릭터 이름을 저장할 변수

    void Start()
    {
        // 각 버튼에 클릭 이벤트 연결
        HanaProfile.onClick.AddListener(() => OnCharacterSelected("하나"));
        YehyunProfile.onClick.AddListener(() => OnCharacterSelected("예현"));
        JiseonProfile.onClick.AddListener(() => OnCharacterSelected("진설"));
        SinaProfile.onClick.AddListener(() => OnCharacterSelected("신아"));
        BoanProfile.onClick.AddListener(() => OnCharacterSelected("보안"));
    }

    // 캐릭터 선택 시 호출되는 메서드
    public void OnCharacterSelected(string characterName)
    {
        selectedCharacterName = characterName; // 선택된 캐릭터 이름 저장
        Debug.Log($"선택된 캐릭터: {selectedCharacterName}");

        // 텍스트 업데이트
        if (CharacterConfirmText != null)
        {
            CharacterConfirmText.text = $"당신의 최애는 {selectedCharacterName} 입니까?";
        }

        // 캐릭터 확인 창 활성화
        if (CharacterConfirmWindow != null)
        {
            CharacterConfirmWindow.SetActive(true);
        }

        // 캐릭터 선택 창 비활성화
        if (SelectCharacterWindow != null)
        {
            SelectCharacterWindow.SetActive(false);
        }
    }

    // 확인창의 확인 버튼 클릭 시 호출
public void SaveName()
{
    if (!string.IsNullOrEmpty(selectedCharacterName))
    {
        Debug.Log($"저장된 캐릭터: {selectedCharacterName}");

        // 선택된 캐릭터 이름 저장
        PlayerPrefs.SetString("SelectedCharacter", selectedCharacterName);
        PlayerPrefs.Save();

        // 확인 창 비활성화
        if (CharacterConfirmWindow != null)
        {
            CharacterConfirmWindow.SetActive(false);
        }

        // **닉네임 입력창 비활성화 추가**
        if (NicknameInputPanel != null)
        {
            NicknameInputPanel.SetActive(false);
        }

        // 추가 로직: 게임 시작 또는 다른 UI 표시
        Debug.Log("캐릭터 저장 완료! 다음 단계로 진행합니다.");
    }
    else
    {
        Debug.LogError("캐릭터가 선택되지 않았습니다.");
    }
}

    // 확인창의 취소 버튼 클릭 시 호출
    public void CancelButtonClick()
    {
        // 확인창 비활성화
        if (CharacterConfirmWindow != null)
        {
            CharacterConfirmWindow.SetActive(false);
        }

        // 캐릭터 선택 창 활성화
        if (SelectCharacterWindow != null)
        {
            SelectCharacterWindow.SetActive(true);
        }

        Debug.Log("캐릭터 선택으로 돌아갑니다.");
    }
}
