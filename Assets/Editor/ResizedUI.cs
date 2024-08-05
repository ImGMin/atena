using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ResizeUIEditor : EditorWindow
{
    public TMP_FontAsset newFont;

    [MenuItem("Window/Resize UI")]


    public static void ShowWindow()
    {
        GetWindow<ResizeUIEditor>("Resize UI");
    }

    void OnGUI()
    {
        GUILayout.Label("Select TMP Font Asset", EditorStyles.boldLabel);
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font Asset", newFont, typeof(TMP_FontAsset), false);
        if (GUILayout.Button("Resize All UI Elements"))
        {
            ResizeAllUI();
        }
        if (GUILayout.Button("Undo All UI Elements"))
        {
            UndoAllUI();
        }
        if (GUILayout.Button("Change Text Asset"))
        {

            ChangeTextAsset();
        }
        if (GUILayout.Button("Func"))
        {

            Func();
        }
        if (GUILayout.Button("FuncRev"))
        {

            FuncRev();
        }
    }

    void FindChildOj(Transform parent, HashSet<Transform> checkedObjects, bool flag)
    {
        if (checkedObjects.Contains(parent))
            return;

        checkedObjects.Add(parent);

        //flag |= !parent.gameObject.activeSelf;
        if (flag)
        {
            RectTransform rectTransform = parent.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 현재 크기를 가져옵니다.
                Vector2 currentSize = rectTransform.sizeDelta;

                // 크기를 2배로 설정합니다.
                rectTransform.sizeDelta = currentSize * 2;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y) * 2;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(rectTransform);
            }

            TextMeshProUGUI tmpText = parent.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                // 현재 폰트 크기를 가져옵니다.
                float currentFontSize = tmpText.fontSize;

                // 폰트 크기를 2배로 설정합니다.
                tmpText.fontSize = currentFontSize * 2;
                //tmpText.rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
                tmpText.font = newFont;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(tmpText);
            }
        }

        Transform[] allTransforms = parent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allTransforms)
        {
            FindChildOj(child, checkedObjects, flag);
        }
    }

    void FuncRev()
    {
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            if (go.activeSelf)
                continue;

            RectTransform rectTransform = go.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 현재 크기를 가져옵니다.
                Vector2 currentSize = rectTransform.sizeDelta;

                // 크기를 2배로 설정합니다.
                rectTransform.sizeDelta = currentSize / 2;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y) / 2;

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
                //tmpText.rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
                tmpText.font = newFont;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(tmpText);
            }


        }

        // 변경 사항을 저장합니다.
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    void Func()
    {
        HashSet<Transform> checkedObjects = new HashSet<Transform>();

        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            if (!go.activeSelf)
                FindChildOj(go.transform, checkedObjects, true);

            /*RectTransform rectTransform = go.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 현재 크기를 가져옵니다.
                Vector2 currentSize = rectTransform.sizeDelta;

                // 크기를 2배로 설정합니다.
                rectTransform.sizeDelta = currentSize * 2;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y) * 2;

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
                //tmpText.rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
                tmpText.font = newFont;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(tmpText);
            }*/


        }

        // 변경 사항을 저장합니다.
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y) * 2;

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
                //tmpText.rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);


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
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y) / 2;

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
                //tmpText.rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y) / 2;

                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(tmpText);
            }
        }

        // 변경 사항을 저장합니다.
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void ChangeTextAsset()
    {
        if (newFont == null)
        {
            Debug.Log("폰트 없음");
            return;
        }

        Debug.Log(newFont.name);


        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        
        foreach (GameObject go in allGameObjects)
        {

            TextMeshProUGUI tmpText = go.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.font = newFont;
                
                // 변경 사항을 저장합니다.
                EditorUtility.SetDirty(tmpText);
            }
        }

        // 변경 사항을 저장합니다.
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
    }
}
