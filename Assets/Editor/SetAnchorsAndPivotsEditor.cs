using UnityEngine;
using UnityEditor;

public class SetAnchorsAndPivotsEditor : EditorWindow
{
    public RectTransform levelText;
    public RectTransform experienceText;
    public RectTransform yearText;
    public RectTransform monthDayText;
    public RectTransform dayText;

    [MenuItem("Window/Set Anchors and Pivots")]
    public static void ShowWindow()
    {
        GetWindow<SetAnchorsAndPivotsEditor>("Set Anchors and Pivots");
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Anchors and Pivots for UI Elements", EditorStyles.boldLabel);

        levelText = (RectTransform)EditorGUILayout.ObjectField("Level Text", levelText, typeof(RectTransform), true);
        experienceText = (RectTransform)EditorGUILayout.ObjectField("Experience Text", experienceText, typeof(RectTransform), true);
        yearText = (RectTransform)EditorGUILayout.ObjectField("Year Text", yearText, typeof(RectTransform), true);
        monthDayText = (RectTransform)EditorGUILayout.ObjectField("Month Day Text", monthDayText, typeof(RectTransform), true);
        dayText = (RectTransform)EditorGUILayout.ObjectField("Day Text", dayText, typeof(RectTransform), true);

        if (GUILayout.Button("Set Anchors and Pivots"))
        {
            SetAnchors(levelText, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(10, -10));
            SetAnchors(experienceText, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(10, -50));
            SetAnchors(yearText, new Vector2(1, 1), new Vector2(1, 1), new Vector2(1, 1), new Vector2(-10, -10));
            SetAnchors(monthDayText, new Vector2(1, 1), new Vector2(1, 1), new Vector2(1, 1), new Vector2(-10, -50));
            SetAnchors(dayText, new Vector2(1, 1), new Vector2(1, 1), new Vector2(1, 1), new Vector2(-10, -90));
        }
    }

    void SetAnchors(RectTransform rectTransform, Vector2 minAnchor, Vector2 maxAnchor, Vector2 pivot, Vector2 anchoredPosition)
    {
        if (rectTransform == null)
            return;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
        rectTransform.pivot = pivot;
        rectTransform.anchoredPosition = anchoredPosition;

        EditorUtility.SetDirty(rectTransform);
    }
}
