using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Reflection;
using System.Linq;

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

    private readonly PropertyInfo[] fields;

    public AtenaDate()
    {
        fields = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
        get { return fields[index].GetValue(this); }
        set { fields[index].SetValue(this, value); }
    }

    public int Length
    {
        get { return fields.Length; }
    }
}
