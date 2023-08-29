using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stat : MonoBehaviour
{
    public GameObject statBlock;
    private List<GameObject> blocks = new List<GameObject>();

    public static Stat Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    public static void SaveCurrentStat(bool win, bool lose, float time, string level)
    {
        string timeGetter = level + "_" + "time";
        string winGetter = level + "_" + "win";
        string loseGetter = level + "_" + "lose";
        string startedGetter = level + "_" + "started";

        PlayerPrefs.SetInt(startedGetter, (int)GetStat(level, "started") + 1);

        if (win)
        {
            PlayerPrefs.SetInt(winGetter, (int)GetStat(level, "win") + 1);
        }

        if (lose)
        {
            PlayerPrefs.SetInt(loseGetter, (int)GetStat(level, "lose") + 1);
        }

        if (GetStat(level, "time") == 0 || (time != 0 && time < GetStat(level, "time")))
        {
            PlayerPrefs.SetFloat(timeGetter, time);
        }
    }

    public static void ResetData(string levelIndex)
    {
        ResetStats(levelIndex);

        static void ResetStats(string level)
        {
            PlayerPrefs.DeleteKey(level + "_" + "time");
            PlayerPrefs.DeleteKey(level + "_" + "win");
            PlayerPrefs.DeleteKey(level + "_" + "lose");
            PlayerPrefs.DeleteKey(level + "_" + "started");
        }

        Instance.SetValuesStatBlock();
    }

    public static float GetStat(string level, string stat)
    {
        string getter = level + "_" + stat;
        string winGetter = level + "_" + "win";
        string startedGetter = level + "_" + "started";

        if (stat == "time")
        {
            return PlayerPrefs.GetFloat(getter, 0);
        }
        else if (stat == "winRate")
        {
            return GetWinRate(GetStat(level, "win"), GetStat(level, "started"));
        }
        return PlayerPrefs.GetInt(getter, 0);
    }

    private static float GetWinRate(float win, float started)
    {
        float value = win / started;
        if (value == 0.0 || (started) == 0 || (win) == 0)
        {
            return 0.0f;
        }
        else
        {
            return (float)Math.Round(value * 100, 2);
        }
    }

    private void SpawnStatBlocks()
    {
        float pos = 780;
        for (int i = 0; i < 4; i++)
        {
            blocks.Add(Instantiate(statBlock) as GameObject);
            blocks[blocks.Count - 1].transform.SetParent(transform);
            blocks[blocks.Count - 1].transform.localPosition = new Vector3(0, pos + -275 - 5, 0);
            blocks[blocks.Count - 1].transform.localScale = new Vector3(1, 1, 1);

            pos = pos - 275 - 5;
        }
    }

    private void SetValuesStatBlock()
    {
        SetValues(blocks[0], "easy");
        SetValues(blocks[1], "medium");
        SetValues(blocks[2], "hard");
        SetValues(blocks[3], "vhard");

        static void SetValues(GameObject obj, string level)
        {
            obj.GetComponent<StatBlock>().SetValues(
                level,
                GetStat(level, "started"),
                GetStat(level, "win"),
                GetStat(level, "lose"),
                GetStat(level, "time"),
                GetStat(level, "winRate")
                );
        }
    }

    private void Start()
    {
        SpawnStatBlocks();
        SetValuesStatBlock();
    }
}