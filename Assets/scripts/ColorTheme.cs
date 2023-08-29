using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTheme : MonoBehaviour
{
    public enum Theme
    { LIGHT, DARK, BROWN };

    public GameObject ThemeButton;
    public List<GameObject> ButtonIndicators;

    private bool IsThemeButtonToggle = false;

    private static Theme thisTheme = Theme.LIGHT;

    public static ColorTheme Instance;

    public static void SetTheme(Theme theme)
    {
        thisTheme = theme;
    }

    public string GetTheme()
    {
        return thisTheme.ToString();
    }

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;

        SetTheme(Theme.LIGHT);
        IsThemeButtonToggle = false;
        ThemeButton.SetActive(false);
    }

    public void OnThemeButtonClick(string theme)
    {
        PlayerPrefs.SetString("theme", theme);
        if (theme == "LIGHT")
        {
            OnThemeButtonClick(Theme.LIGHT);
        }
        else if (theme == "DARK")
        {
            OnThemeButtonClick(Theme.DARK);
        }
        else if (theme == "BROWN")
        {
            OnThemeButtonClick(Theme.BROWN);
        }
    }

    public void OnThemeButtonClick(Theme theme)
    {
        int index = 0;

        switch (theme.ToString())
        {
            case "LIGHT":
                index = 0;
                break;

            case "DARK":
                index = 1;
                break;

            case "BROWN":
                index = 2;
                break;
        }

        ColorTheme.SetTheme(theme);

        setIndicatorActive(index);
        ColorSetter.Instance.ColorSet(theme);
        if (grid.Instance)
            grid.Instance.SetAllBoxColor(theme.ToString());

        void setIndicatorActive(int index)
        {
            foreach (var btn in ButtonIndicators)
            {
                btn.SetActive(false);
            }

            ButtonIndicators[index].SetActive(true);
        }
    }

    public void ThemeButtonToggle()
    {
        ThemeButton.SetActive(!IsThemeButtonToggle);
        IsThemeButtonToggle = !IsThemeButtonToggle;
    }

    public Theme ReturnThemeFromString(string theme)
    {
        return theme switch
        {
            "LIGHT" => Theme.LIGHT,
            "DARK" => Theme.DARK,
            "BROWN" => Theme.BROWN,
            _ => Theme.LIGHT,
        };
    }

    private void Start()
    {
        ColorSetter.Instance.ColorSet(ReturnThemeFromString(PlayerPrefs.GetString("theme", "LIGHT")));
        OnThemeButtonClick(ReturnThemeFromString(PlayerPrefs.GetString("theme", "LIGHT")));
    }
}