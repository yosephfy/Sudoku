using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSwitch : Selectable
{
    public GameObject Menu, Statistics;

    protected override void Start()
    {
        LoadMenu();
    }

    public void LoadMenu()
    {
        Menu.SetActive(true);
        Statistics.SetActive(false);
    }

    public void LoadSatistics()
    {
        Menu.SetActive(false);
        Statistics.SetActive(true);
    }
}