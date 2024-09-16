using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class GameData : IIndexer<object>
{
    public int level { get; set; }
    public int exp { get; set; }
    public int energy { get; set; }
    public int friends { get; set; }
    public int cash { get; set; }
    public int reputation { get; set; }
    public int atenaGrowth { get; set; }
    public int favor { get; set; }

    public int[] LvUpEXP = { 0, 20, 26, 35, 47, 62, 80, 101, 1000000 };

    public event Action<int,int> OnValueChanged;

    public event Action<int> LvUpEvent;

    private readonly PropertyInfo[] fields;

    public GameData()
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

    public void ChangeValue(int idx)
    {
        OnValueChanged?.Invoke(idx, (int)fields[idx].GetValue(this));
    }

    public void EventDiscard()
    {
        OnValueChanged = null;
    }

    public void LvUp()
    {
        while (exp >= LvUpEXP[level])
        {
            exp -= LvUpEXP[level];
            level++;
        }
        ChangeValue(0);
        ChangeValue(1);
        LvUpEvent?.Invoke(level);
    }
}
