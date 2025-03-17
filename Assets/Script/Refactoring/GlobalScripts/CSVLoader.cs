using UnityEngine;
using System.Collections.Generic;

public class CSVLoader : MonoBehaviour
{
    public static List<Dictionary<string, string>> LoadCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {fileName}");
            return null;
        }

        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        string[] lines = csvFile.text.Split('\n');
        if (lines.Length < 2) return null; // 데이터가 없는 경우

        // 첫 줄은 헤더 처리
        string[] headers = lines[0].Trim().Split(',');

        // 데이터 파싱
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            // 특수 구분자 처리 ("\c" → "," , "\n" → 개행문자)
            line = line.Replace("\\c", ",").Replace("\\n", "\n");

            string[] values = line.Split(',');
            Dictionary<string, string> entry = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length; j++)
            {
                if (j < values.Length)
                {
                    entry[headers[j]] = values[j];
                }
                else
                {
                    entry[headers[j]] = ""; // 값이 비어있을 경우 빈 문자열
                }
            }

            dataList.Add(entry);
        }

        Debug.Log("check");
        return dataList;
    }
}
