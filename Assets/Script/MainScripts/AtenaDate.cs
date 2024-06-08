using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtenaDate
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int weekday;

    public AtenaDate(int year, int month, int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        hour = 0;
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
}
