using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gametest : MonoBehaviour
{
    public GameObject notePrefab;
    public RectTransform noteArea;
    public Button[] inputButtons;
    public int maxNotes = 10;
    private int noteCount = 0;

    void Start()
    {
        foreach (Button btn in inputButtons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }
        InvokeRepeating(nameof(SpawnNote), 1f, 1f); // 0.5초마다 노트 생성
    }

    void Update()
    {  

    }

    void SpawnNote()
    {
        if (noteCount < maxNotes)
        {
            
            RectTransform noteRect = notePrefab.GetComponent<RectTransform>();
            Vector2 noteSize = noteRect.sizeDelta;

            float randomX = Random.Range(noteSize.x / 2, noteArea.rect.width - noteSize.x / 2);
            float randomY = Random.Range(noteSize.y / 2, noteArea.rect.height - noteSize.y / 2);

            Vector2 randomPosition = new Vector2(randomX, randomY);
            GameObject note = Instantiate(notePrefab, noteArea);
            note.GetComponent<RectTransform>().anchoredPosition = randomPosition;
            noteCount++;
            Debug.Log("Note Created. Count: " + noteCount);
        }
        else
        {
            Debug.Log("Max notes reached: " + noteCount);
            CancelInvoke(nameof(SpawnNote)); // 노트가 maxNotes에 도달하면 InvokeRepeating을 중지합니다.
            Debug.Log("Note creation stopped.");
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        // 버튼 클릭 시 로직 추가
        Debug.Log("Button Clicked: " + clickedButton.name);
    }
}