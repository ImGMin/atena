using System;
using System.Collections.Generic;

class Program
{
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
        Random rand = new Random();
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
}
