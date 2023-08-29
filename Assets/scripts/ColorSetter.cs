using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSetter : MonoBehaviour
{
    public List<Color> TopTextColors, MenuTextColors, BackGroundColors, MenuColors, ToolsColors, GridColor, ButtonsColor, GridNumColor, SimilarNumColor;

    public List<GameObject> TopTextObj, MenuTextObj, BackgroundObj, MenuObj, ToolsObj, GridObj, ButtonsObj, SimilarNumObj;

    public static ColorSetter Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    public void ColorSet(ColorTheme.Theme theme)
    {
        int index = -1;

        switch (theme)
        {
            case ColorTheme.Theme.LIGHT:
                index = 0;
                break;

            case ColorTheme.Theme.DARK:
                index = 1;
                break;

            case ColorTheme.Theme.BROWN:
                index = 2;
                break;
        }

        GiveColorText(TopTextObj, TopTextColors[index]);
        GiveColorText(MenuTextObj, MenuTextColors[index]);
        GiveColorImage(BackgroundObj, BackGroundColors[index]);
        GiveColorImage(MenuObj, MenuColors[index]);
        GiveColorImage(ToolsObj, ToolsColors[index]);
        GiveColorImage(GridObj, GridColor[index]);
        GiveColorImage(SimilarNumObj, SimilarNumColor[index]);
        //GiveColorButtons(ButtonsObj, ButtonsColor[index]); ;
    }

    private void GiveColorImage(List<GameObject> objects, Color color)
    {
        foreach (var item in objects)
        {
            item.GetComponent<Image>().color = (color);
        }
    }

    private void GiveColorText(List<GameObject> objects, Color color)
    {
        foreach (var item in objects)
        {
            item.GetComponent<Text>().color = color;
        }
    }

    private void GiveColorButtons(List<GameObject> objects, Color color)
    {
        foreach (var item in objects)
        {
            item.GetComponent<Text>().color = color;
        }
    }

    public static Color GiveColorButtons(string theme)
    {
        return theme switch
        {
            "LIGHT" => ColorSetter.Instance.ButtonsColor[0],
            "DARK" => ColorSetter.Instance.ButtonsColor[1],
            "BROWN" => ColorSetter.Instance.ButtonsColor[2],
            _ => ColorSetter.Instance.ButtonsColor[0]
        };
    }

    public static Color GiveColorGrid(string theme)
    {
        return theme switch
        {
            "LIGHT" => ColorSetter.Instance.GridColor[0],
            "DARK" => ColorSetter.Instance.GridColor[1],
            "BROWN" => ColorSetter.Instance.GridColor[2],
            _ => ColorSetter.Instance.GridColor[0]
        };
    }

    public static Color GiveColorGridNum(string theme)
    {
        return theme switch
        {
            "LIGHT" => ColorSetter.Instance.GridNumColor[0],
            "DARK" => ColorSetter.Instance.GridNumColor[1],
            "BROWN" => ColorSetter.Instance.GridNumColor[2],
            _ => ColorSetter.Instance.GridNumColor[0]
        };
    }

    public static Color GiveColorSimilarNum(string theme)
    {
        return theme switch
        {
            "LIGHT" => ColorSetter.Instance.SimilarNumColor[0],
            "DARK" => ColorSetter.Instance.SimilarNumColor[1],
            "BROWN" => ColorSetter.Instance.SimilarNumColor[2],
            _ => ColorSetter.Instance.SimilarNumColor[0]
        };
    }
}