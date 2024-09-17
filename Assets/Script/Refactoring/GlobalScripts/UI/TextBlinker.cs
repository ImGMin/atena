using UnityEngine;
using TMPro;

public class TextBlinker : MonoBehaviour
{
    public TMP_Text textToBlink;
    public float blinkSpeed = 1.0f; // 깜빡이는 속도

    private Color originalColor;
    private bool isFadingOut = true;

    void Start()
    {
        if (textToBlink == null)
        {
            textToBlink = GetComponent<TMP_Text>();
        }
        if (textToBlink != null)
        {
            originalColor = textToBlink.color;
        }
        else
        {
            Debug.LogError("TMP_Text 컴포넌트가 설정되지 않았습니다.");
        }
    }

    void Update()
    {
        if (textToBlink != null)
        {
            Color color = textToBlink.color;
            float alphaChange = blinkSpeed * Time.deltaTime;

            if (isFadingOut)
            {
                color.a -= alphaChange;
                if (color.a <= 0.3f)
                {
                    color.a = 0.3f;
                    isFadingOut = false;
                }
            }
            else
            {
                color.a += alphaChange;
                if (color.a >= originalColor.a)
                {
                    color.a = originalColor.a;
                    isFadingOut = true;
                }
            }

            textToBlink.color = color;
        }
    }
}
