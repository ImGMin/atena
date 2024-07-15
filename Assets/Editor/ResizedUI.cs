using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class ResizeUIEditor : EditorWindow
{
    [MenuItem("Window/Resize UI")]
    public static void ShowWindow()
    {
        GetWindow<ResizeUIEditor>("Resize UI");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Resize All UI Elements"))
        {
            ResizeAllUI();
        }
        if (GUILayout.Button("Undo All UI Elements"))
        {
            UndoAllUI();
        }
    }

    void ResizeAllUI()
    {
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 현재 크기를 가져옵니다.
                Vector2 currentSize = rectTransform.sizeDelta;

                // 크기를 2배로 설정합니다.
                rectTransform.sizeDelta = currentSize * 2;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(rectTransform);
            }

            TextMeshProUGUI tmpText = go.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                // 현재 폰트 크기를 가져옵니다.
                float currentFontSize = tmpText.fontSize;

                // 폰트 크기를 2배로 설정합니다.
                tmpText.fontSize = currentFontSize * 2;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(tmpText);
            }
        }

        // 변경 사항을 저장합니다.
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void UndoAllUI()
    {
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 현재 크기를 가져옵니다.
                Vector2 currentSize = rectTransform.sizeDelta;

                // 크기를 2배로 설정합니다.
                rectTransform.sizeDelta = currentSize / 2;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(rectTransform);
            }

            TextMeshProUGUI tmpText = go.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                // 현재 폰트 크기를 가져옵니다.
                float currentFontSize = tmpText.fontSize;

                // 폰트 크기를 2배로 설정합니다.
                tmpText.fontSize = currentFontSize / 2;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(tmpText);
            }
        }

        // 변경 사항을 저장합니다.
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
