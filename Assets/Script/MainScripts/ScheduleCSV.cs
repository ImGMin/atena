using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScheduleCSV : MonoBehaviour
{
    public string filePath = "Assets/Resources/test_date.csv";
    List<Dictionary<string, string>> data;

    void Start()
    {
        data = ReadCSV(filePath);
    }

    public void UpdateSchedule()
    {
        int start = GameManager.Instance.gameData.curTime.day;
        int idx = start;
        while (start==idx || (idx-1)%5 != 0)
        {
            string str = data[idx-1]["상황ID"];
            if (str == "Situ_Idle")
            {
                GameManager.Instance.gameData.Schedule[(idx - 1) % 5].Item1 = 0;
            }
            else if (str == "Situ_11_01")
            {
                GameManager.Instance.gameData.Schedule[(idx - 1) % 5].Item1 = 1;
            }
            else
            {
                GameManager.Instance.gameData.Schedule[(idx - 1) % 5].Item1 = 3;
            }
            idx++;
        }
    }

    List<Dictionary<string, string>> ReadCSV(string filePath)
    {
        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        // 파일 경로가 유효한지 확인
        if (File.Exists(filePath))
        {
            // CSV 파일의 모든 줄을 읽기
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length > 0)
            {
                // 첫 번째 줄은 헤더로 사용
                string[] headers = lines[0].Split(',');

                // 각 행의 데이터를 딕셔너리에 저장
                for (int i = 1; i < lines.Length; i++)
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
        else
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + filePath);
        }

        return data;
    }
}
