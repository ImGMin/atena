using UnityEngine;
using UnityEngine.UI;

public class MoveUIText : MonoBehaviour
{
    public float speed = 50f; // 텍스트 이동 속도

    private RectTransform rectTransform;

    void Start()
    {
        // RectTransform 컴포넌트를 가져옴
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 텍스트를 좌우로 움직임
        float movement = speed * Time.deltaTime;
        rectTransform.anchoredPosition += new Vector2(movement, 0);

        // 화면 경계를 벗어나지 않도록 제한
        if (rectTransform.anchoredPosition.x > Screen.width / 2 - 90 || rectTransform.anchoredPosition.x < -Screen.width / 2 + 90)
        {
            speed = -speed;
        }
    }
}
