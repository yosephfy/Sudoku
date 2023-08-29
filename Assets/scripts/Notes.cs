using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notes : MonoBehaviour
{
    private int Index;
    private bool isActive;
    public Text NotesObj;

    // Start is called before the first frame update
    private void Start()
    {
        NotesObj.color = ColorSetter.GiveColorButtons(ColorTheme.Instance.GetTheme());
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SetNotesIndex(int index)
    {
        this.Index = index;
        NotesObj.text = (index + 1).ToString();
    }

    public int GetNotesIndex()
    {
        return this.Index;
    }

    public void SetNotesActive(bool value)
    {
        this.isActive = value;

        NotesObj.gameObject.SetActive(value);
        //if (value) Debug.Log("here" + value);
    }

    public bool IsNotesActive()
    {
        return this.isActive;
    }
}