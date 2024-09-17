using UnityEngine;
using UnityEngine.UI;

public class ExpandableButton : MonoBehaviour
{
    public float expandedHeight = 200f; // 버튼이 확장되었을 때의 높이
    public float collapsedHeight = 100f; // 버튼이 원래 상태일 때의 높이
    private bool isExpanded = false;

    private RectTransform rectTransform;
    private GameObject MusicList;

    void Start()
    {
        MusicList = transform.Find("Music Group").gameObject;
        expandedHeight = 130f + MusicList.transform.childCount * 60f;

        rectTransform = GetComponent<RectTransform>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSize);
    }

    void ToggleSize()
    {
        if (isExpanded)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, collapsedHeight);
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, expandedHeight);
        }

        MusicList.SetActive(!isExpanded);
        isExpanded = !isExpanded;

        // 부모의 레이아웃 그룹 갱신
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}
