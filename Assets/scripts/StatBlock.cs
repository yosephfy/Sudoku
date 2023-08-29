using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatBlock : MonoBehaviour
{
    public Text Level, Time, Started, Won, Lost, WinRate;

    private string LevelIndex;

    public void SetValues(string level, float started, float win, float lose, float time, float winRate)
    {
        LevelIndex = level;

        Level.text = LevelSelector.GetLevelDisplay(level);
        Time.text = TimeCounter.toString(time);
        Started.text = started.ToString();
        Won.text = win.ToString();
        Lost.text = lose.ToString();
        WinRate.text = winRate.ToString() + "%";
    }

    public string GetLevel()
    {
        return LevelIndex;
    }

    public void ResetStat()
    {
        Stat.ResetData(LevelIndex);
    }
}