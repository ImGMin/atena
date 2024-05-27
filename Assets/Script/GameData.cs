using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int level { get; set; }
    public int exp { get; set; }
    public int energy { get; set; }
    public int friends { get; set; }
    public int cash { get; set; }
    public int reputation { get; set; }
    public int atenaGrowth { get; set; }
    public AtenaDate curTime { get; set; }

    public event Action<string, int> OnValueChanged;

    public int[] LvUpEXP = { 0, 20, 26, 35 };

    public GameData()
    {
        level = 1;
        exp = 0;
        energy = 100;
        friends = 0;
        cash = 0;
        reputation = 0;
        atenaGrowth = 0;
        curTime = new AtenaDate(2025, 1, 1);
    }

    public override string ToString()
    {
        return $"level: {level}\n exp: {exp}\n level: {level}\n level: {level}\n level: {level}\n ";
    }
}