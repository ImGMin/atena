using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JasmineManager : MonoBehaviour
{

    List<Dictionary<string, string>> data;
    List<Album> albums = new List<Album>();
    List<List<Button>> buttons = new List<List<Button>>();
    AtenaDate albumDate;

    public GameObject content;
    public GameObject albumPrefab;
    public GameObject musicPrefab;

    public Button playButton;
    public TMP_Text playMusicName;
    public Slider slider;

    public GameObject autoImage;
    public Button autoButton;

    bool autoPlaying = false;
    float autoTime = 0f;
    float maxAutoTime = 1f;

    bool playing = false;
    float time = 0f;
    float maxTime = 10f;

    public Image SelectRect;
    (int,int) SelectIdx = (-1,-1);
    (int, int) LastPlayIdx = (-1, -1);

    private void Awake()
    {
        data = ReadCSV();
        GenAlbumList();

        playButton.onClick.AddListener(() => OnPlayButtonClick());
        autoButton.onClick.AddListener(() => OnAutoButtonClick());
        autoImage.GetComponent<Button>().onClick.AddListener(() => OnAutoUIClick());
    }


    void Start()
    {
        slider.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayMusic();
        AutoPlay();
        UpdateSlider();
    }

    List<Dictionary<string, string>> ReadCSV()
    {
        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        TextAsset csvData = Resources.Load<TextAsset>("album_list");
        // 파일 경로가 유효한지 확인
        if (csvData != null)
        {
            // CSV 파일의 모든 줄을 읽기
            string[] lines = csvData.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            if (lines.Length > 0)
            {
                // 첫 번째 줄은 헤더로 사용
                string[] headers = lines[0].Split(',');

                // 각 행의 데이터를 딕셔너리에 저장
                for (int i = 1; i < lines.Length; i++)
                {
                    if (!string.IsNullOrEmpty(lines[i]))
                    {
                        string[] fields = lines[i].Split(',');

                        Dictionary<string, string> entry = new Dictionary<string, string>();

                        for (int j = 0; j < headers.Length; j++)
                        {
                            entry[headers[j]] = fields[j];
                        }

                        data.Add(entry);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다");
        }

        return data;
    }

    AtenaDate ParseAtenaDate(string dateString)
    {
        // "년", "월", "일"을 기준으로 문자열을 분리
        string[] parts = dateString.Split(new char[] { '년', '월', '일' }, StringSplitOptions.RemoveEmptyEntries);

        // 분리된 문자열의 앞뒤 공백 제거 및 정수로 변환
        int year = int.Parse(parts[0].Trim());
        int month = int.Parse(parts[1].Trim());
        int day = int.Parse(parts[2].Trim());

        // AtenaDate 객체 생성 및 반환
        return new AtenaDate(year, month, day);
    }

    void GenAlbumList()
    {
        for (int i = 0; i < data.Count; i++)
        {
            albumDate = ParseAtenaDate(data[i]["발매일"]);

            if (GameManager.Instance.gameData.curTime < albumDate)
            {
                break;
            }

            GameObject albumOb = Instantiate(albumPrefab, transform.position, transform.rotation);

            Album album = new Album(data[i]["앨범 이름"]);

            albumOb.transform.Find("Title").GetComponent<TMP_Text>().text = $"{album.name}";
            albumOb.transform.Find("Date").GetComponent<TMP_Text>().text = albumDate.ToString();

            List<Button> curButtons = new List<Button>();

            GameObject music_list = albumOb.transform.Find("Music Group").gameObject;
            for (int j = 1; j <= int.Parse(data[i]["곡 수"]); j++)
            {
                string name = data[i][$"곡 이름{j}"];
                Music music = new Music(name, album);
                album.included.Add(music);

                GameObject musicOb = Instantiate(musicPrefab, transform.position, transform.rotation);

                RectTransform rectTransform = musicOb.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, -25 + (-60 * j));

                musicOb.GetComponentInChildren<TMP_Text>().text = $" {j}. {name}";
                Button button = musicOb.GetComponent<Button>();
                button.onClick.AddListener(() => OnMusicButtonClick(button));
                curButtons.Add(button);

                musicOb.transform.SetParent(music_list.transform);
            }

            albumOb.transform.SetParent(content.transform);
            albums.Add(album);
            buttons.Add(curButtons);
        }
    }

    public void OnMusicButtonClick(Button clickedButton)
    {
        if (playing) return;

        (int, int) index = (-1, -1);
        for (int i = 0; i < buttons.Count; i++)
        {
            for (int j = 0; j < buttons[i].Count; j++)
            {
                if (clickedButton == buttons[i][j])
                {
                    index = (i,j); 
                    break;
                }
            }
        }

        if (SelectIdx == index)
        {
            SelectIdx = (-1, -1);
            SelectRect.gameObject.SetActive(false);
        }
        else
        {
            SelectIdx = index;
            SelectRect.transform.SetParent(clickedButton.transform);
            SelectRect.rectTransform.anchoredPosition = new Vector2(0,0);
            SelectRect.gameObject.SetActive(true);
        }
    }

    public void OnPlayButtonClick()
    {
        if (SelectIdx == (-1, -1)) return;
        if (playing) return;
        LastPlayIdx = SelectIdx;


        if (GameManager.Instance.gameData.cash < 300)
        {
            return;
        }
        GameManager.Instance.ChangeValue(cash: -300);

        Music playMusic = albums[SelectIdx.Item1].included[SelectIdx.Item2];
        playMusicName.text = playMusic.name;
        playing = true;
    }

    public void OnAutoButtonClick()
    {
        if (autoPlaying)
        {
            OnAutoUIClick();
            return;
        }

        if (GameManager.Instance.gameData.cash < 600)
        {
            return;
        }

        if (!playing)
        {
            OnPlayButtonClick();
        }
        
        GameManager.Instance.ChangeValue(cash: -300);
        autoImage.SetActive(true);
        autoPlaying = true;
    }

    public void OnAutoUIClick()
    {
        autoImage.SetActive(false);
        autoPlaying = false;
    }

    void PlayMusic()
    {
        if (!playing) return;

        time += Time.deltaTime;
        if (time > maxTime)
        {
            int growth = int.Parse(data[LastPlayIdx.Item1]["성장도"]);
            int exp = int.Parse(data[LastPlayIdx.Item1]["경험치"]);

            Debug.Log($"{growth},{exp}");
            GameManager.Instance.ChangeValue(exp:exp, atenaGrowth:growth);

            Init();
        }
    }

    void AutoPlay()
    {
        if (playing || !autoPlaying) return;
        if (GameManager.Instance.gameData.cash < 300)
        {
            autoPlaying = false;
            return;
        }

        autoTime += Time.deltaTime;
        if (autoTime > maxAutoTime)
        {
            GameManager.Instance.ChangeValue(cash: -300);

            Music playMusic = albums[LastPlayIdx.Item1].included[LastPlayIdx.Item2];
            playMusicName.text = playMusic.name;
            playing = true;
            autoTime = 0f;
        }
    }

    void UpdateSlider()
    {
        // time의 값이 0에서 10 사이일 때 슬라이더를 time만큼 채우기
        slider.value = time / 10.0f;
    }

    void Init()
    {
        playMusicName.text = "";
        SelectRect.gameObject.SetActive(false);

        time = 0f; 
        playing = false; 
        SelectIdx = (-1, -1); 
    }
}
