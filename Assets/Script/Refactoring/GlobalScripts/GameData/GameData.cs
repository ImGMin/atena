using System.Collections;
using System.Collections.Generic;


public class GameData : IIndexer<object>
{
    public int level { get; set; }
    public int exp { get; set; }
    public int energy { get; set; }
    public int friends { get; set; }
    public int cash { get; set; }
    public int reputation { get; set; }
    public int atenaGrowth { get; set; }
<<<<<<< Updated upstream
    public int favor { get; set; }
=======
>>>>>>> Stashed changes

    public int[] LvUpEXP = { 0, 20, 26, 35, 47, 62, 80, 101, 1000000 };

    private object[] data;

    public GameData()
    {
<<<<<<< Updated upstream
        data = new object[] {level, exp, energy, friends, cash, reputation, atenaGrowth, favor};
=======
        data = new object[] {level, exp, energy, friends, cash, reputation, atenaGrowth};
>>>>>>> Stashed changes
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
