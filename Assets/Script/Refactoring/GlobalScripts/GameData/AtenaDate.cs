using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtenaDate : IIndexer<object>//, IComparable<AtenaDate_prev>
{
    public int year { get; set; }
    public int month { get; set; }

    private int _day;
    public int day 
    { 
        get { return _day; } 
        set 
        { 
            _day = value;
            UpdateDate();
        } 
    }
    public float hour { get; set; }
    public int weekday { get; set; }

    private object[] data;
    
    public AtenaDate()
    {
        data = new object[] { year, month, day, hour };
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

        weekday = (day - 1) % 5;
    }

    public object this[int index]
    {
        get { return data[index]; }
        set { data[index] = value; }
    }

    public int Length
    {
        get { return data.Length; }
    }
}
