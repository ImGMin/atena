using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

class Program
{
    public static List<int> GetRandomSampleWithReplacement(int n, int m)
    {
        List<int> Sample = new List<int>(n);

        System.Random rand = new System.Random();
        for (int i = 0; i < n; i++) 
        {
            Sample[i] = rand.Next(m);
        }

        return Sample;
    }


    public static List<int> GetRandomIndices(int n, int m)
    {
        if (m > n)
        {
            throw new ArgumentException("m cannot be greater than n");
        }

        // n 길이의 리스트 생성
        List<int> indices = new List<int>(n);
        for (int i = 0; i < n; i++)
        {
            indices.Add(i);
        }

        // 리스트 섞기
        System.Random rand = new System.Random();
        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }

        // 앞의 m개의 요소 선택
        return indices.GetRange(0, m);
    }

    public static float CalculateTextHeight(TMP_Text tmpText)
    {
        float preferredHeight = tmpText.preferredHeight;

        // 현재 텍스트 박스의 크기
        RectTransform textRectTransform = tmpText.GetComponent<RectTransform>();
        float textBoxHeight = textRectTransform.sizeDelta.y;

        return Mathf.Max(preferredHeight,textBoxHeight);
    }
}
