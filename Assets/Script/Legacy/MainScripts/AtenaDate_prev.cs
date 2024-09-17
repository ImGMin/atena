using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AtenaDate_prev : IComparable<AtenaDate_prev>
{
    public int year;
    public int month;
    public int day;
    public float hour;
    public int weekday;

    public AtenaDate_prev(int year, int month, int day)
    {
        if (year < 100) year += 2000;
        this.year = year;
        this.month = month;
        this.day = day;
        hour = 0f;
        weekday = (day-1)%5;
    }

    public void UpdateDate()
    {
        if (day > 20)
        {
            day = 20;
            month += 1;
        }

        if (month > 12)
        {
            month -= 12;
            year += 1;
        }

        weekday = (day-1) % 5;
    }

    public int CompareTo(AtenaDate_prev other)
    {
        if (other == null) return 1;

        // 년도 비교
        int yearComparison = year.CompareTo(other.year);
        if (yearComparison != 0) return yearComparison;

        // 월 비교
        int monthComparison = month.CompareTo(other.month);
        if (monthComparison != 0) return monthComparison;

        // 일 비교
        return day.CompareTo(other.day);
    }

    // 연산자 오버로딩
    public static bool operator <(AtenaDate_prev lhs, AtenaDate_prev rhs)
    {
        return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator >(AtenaDate_prev lhs, AtenaDate_prev rhs)
    {
        return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator <=(AtenaDate_prev lhs, AtenaDate_prev rhs)
    {
        return lhs.CompareTo(rhs) <= 0;
    }

    public static bool operator >=(AtenaDate_prev lhs, AtenaDate_prev rhs)
    {
        return lhs.CompareTo(rhs) >= 0;
    }

    public override string ToString()
    {
        return $"{year:D2}.{month:D2}.{day:D2}";
    }
}
