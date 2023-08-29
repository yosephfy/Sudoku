using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeCounter : MonoBehaviour
{
    public Text timeText;
    public GameObject PauseMenu;
    public GameObject GameWonMenu;
    public GameObject GameLoseMenu;
    private static bool timeStarted;
    private static float tickTime;

    public static TimeCounter Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    public static void SetTime(float num)
    {
        tickTime = num;
    }

    public static float GetTime()
    {
        return tickTime;
    }

    public static void StartTime(bool value)
    {
        timeStarted = value;
    }

    public void PausePlayGame()
    {
        //ScreenCapture.CaptureScreenshot("game_sc.png");
        StartTime(!timeStarted);

        grid.SetPaused(timeStarted);
        PauseMenu.SetActive(!timeStarted);
    }

    public void WinGame()
    {
        StartTime(false);

        GameWonMenu.SetActive(true);
    }

    public void LoseGame()
    {
        StartTime(false);

        GameLoseMenu.SetActive(true);
    }

    public void RestartGame()
    {
        SetTime(0);
        StartTime(true);

        GameLoseMenu.SetActive(false);
    }

    public void ContinueGame()
    {
        StartTime(true);

        GameLoseMenu.SetActive(false);
    }

    public static string toString(float tick)
    {
        TimeSpan span = TimeSpan.FromSeconds(tick);

        string hour_ = leading_zero(span.Hours);
        string minutes_ = leading_zero(span.Minutes);
        string seconds_ = leading_zero(span.Seconds);

        string result = hour_ + ":" + minutes_ + ":" + seconds_;
        return result;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //tickTime = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (timeStarted)
        {
            tickTime += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(tickTime);

            string hour_ = leading_zero(span.Hours);
            string minutes_ = leading_zero(span.Minutes);
            string seconds_ = leading_zero(span.Seconds);

            string result = hour_ + ":" + minutes_ + ":" + seconds_;
            timeText.text = result;
        }
    }

    private static string leading_zero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}