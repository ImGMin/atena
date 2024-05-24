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
        weekday = day%5;
    }
}
