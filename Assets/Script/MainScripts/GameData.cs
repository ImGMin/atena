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
    public string playerName {  get; set; }

    public string myFavorite {  get; set; }

    //1번은 상황ID, 2번은 근무ID
    public (int,int)[] Schedule = new (int,int)[5];

    public event Action<int> OnLvChanged;
    public event Action<int> OnExpChanged;
    public event Action<int> OnEnergyChanged;
    public event Action<int> OnFriendChanged;
    public event Action<int> OnCashChanged;
    public event Action<int> OnReputationChanged;
    public event Action<int> OnAtenaGrowthChanged;
    public event Action<AtenaDate> OnAtenaDateChanged;

    public int[] LvUpEXP = { 0, 20, 26, 35, 47, 62, 80, 101, 1000000};

    public GameData()
    {
        level = 1;
        exp = 0;
        energy = 100;
        friends = 0;
        cash = 100000;
        reputation = 0;
        atenaGrowth = 0;
        curTime = new AtenaDate(2025, 1, 1);
        Schedule = new (int, int)[5];
        /*if (playerName != null)
        {
            this.playerName = playerName;
        }
        else
        {
            this.playerName = "민규";
        }*/
        playerName = "민규";
        myFavorite = "신냐";
    }

    public void ChangeLvExp()
    {
        OnLvChanged?.Invoke(level);
        OnExpChanged?.Invoke(exp);
    }

    public void ChangeEnergy()
    {
        OnEnergyChanged?.Invoke(energy);
    }

    public void ChangeFriend()
    {
        OnFriendChanged?.Invoke(friends);
    }

    public void ChangeCash()
    {
        OnCashChanged?.Invoke(cash);
    }

    public void ChangeReputation()
    {
        OnReputationChanged?.Invoke(reputation);
    }

    public void ChangeAtenaGrowth()
    {
        OnAtenaGrowthChanged?.Invoke(atenaGrowth);
    }
    
    public void ChangeAtenaDate()
    {
        OnAtenaDateChanged?.Invoke(curTime);
    }
}