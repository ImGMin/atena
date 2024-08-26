using System.Collections;
using System.Collections.Generic;
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

    private readonly PropertyInfo[] fields;

    public GameData()
    {
        fields = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
