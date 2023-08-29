using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    private static string level;

    public GameObject ContineBtn;
    public Text LevelObj, TimeObj;

    public void SetLevel(string lev)
    {
        level = lev;
    }

    public static int GetLevel()
    {
        if (level == "continue" && BoardDataSave.GameDataFileExists())
        {
            grid.SetContinue(true);
        }
        else
        {
            grid.SetContinue(false);
        }

        return GetLevel(level);
    }

    public static int GetLevel(string level)
    {
        if (level == "easy")
        {
            return 20;
        }
        else if (level == "medium")
        {
            return 30;
        }
        else if (level == "hard")
        {
            return 40;
        }
        else if (level == "vhard")
        {
            return 50;
        }
        else
        {
            return GetLevel(BoardDataSave.GetLevelData());
        }
    }

    public static string GetLevel(int level)
    {
        if (level == 20)
        {
            return "easy";
        }
        else if (level == 30)
        {
            return "medium";
        }
        else if (level == 40)
        {
            return "hard";
        }
        else if (level == 50)
        {
            return "vhard";
        }
        else
        {
            return (BoardDataSave.GetLevelData());
        }
    }

    public static string GetLevelDisplay(int level)
    {
        if (level == 20)
        {
            return "Easy";
        }
        else if (level == 30)
        {
            return "Medium";
        }
        else if (level == 40)
        {
            return "Hard";
        }
        else if (level == 50)
        {
            return "Very Hard";
        }
        else
        {
            return (BoardDataSave.GetLevelData());
        }
    }

    public static string GetLevelDisplay(string level)
    {
        if (level == "easy")
        {
            return "Easy";
        }
        else if (level == "medium")
        {
            return "Medium";
        }
        else if (level == "hard")
        {
            return "Hard";
        }
        else if (level == "vhard")
        {
            return "Very Hard";
        }
        else
        {
            return (BoardDataSave.GetLevelData());
        }
    }

    private void Start()
    {
        ContineBtn.SetActive(BoardDataSave.GameDataFileExists());
        if (BoardDataSave.GameDataFileExists())
        {
            LevelObj.text = BoardDataSave.GetLevelData();
            TimeObj.text = TimeCounter.toString(BoardDataSave.GetTimeData());
        }
    }
}