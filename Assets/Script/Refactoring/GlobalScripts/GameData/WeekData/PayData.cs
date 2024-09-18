using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
public class PayData : IIndexer<object>
{
    public int pay { get; set; }

    private readonly PropertyInfo[] fields;

    public PayData()
    {
        fields = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(int))
            .ToArray();
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
