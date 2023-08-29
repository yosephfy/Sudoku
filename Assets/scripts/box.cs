using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class box : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    private int IndexNumber;
    private int theNumber;
    private int theCorrectNumber;
    public Text numText;
    private bool isDefaultNum = false;
    private bool isSelected;

    private bool isActiveNumber = true;
    private bool isNotesActive = false;

    private List<Text> notes = new List<Text>();
    public Text noteBox;

    private int[] Dimentions = new int[2];

    public void SetBoxIndex(int num)
    {
        this.IndexNumber = num;
    }

    public int GetBoxIndex()
    {
        return this.IndexNumber;
    }

    public void SetDefaultNum(bool value)
    {
        isDefaultNum = value;
    }

    public void SetCorrectNumber(int num)
    {
        theCorrectNumber = num;
    }

    public int GetCorrectNumber()
    {
        return theCorrectNumber;
    }

    public void SetNumber(int num)
    {
        if (!grid.IsNotesClicked())
        {
            numText.text = num != 0 ? num.ToString() : " ";
            theNumber = num;
            SetNotesActive(false);
        }
        else
        {
            if (num != 0 && !isDefaultNum)
            {
                numText.text = " ";
                theNumber = 0;
                SetNotes(num);
            }
            else
            {
                SetNotesActive(false);
            }
        }
        SetNumColor();

        if (grid.Instance.IsGridFull())
        {
            grid.CheckIfWon();
        }
    }

    public void SetNumColor()
    {
        if (isDefaultNum)
        {
            numText.color = ColorSetter.GiveColorGridNum(ColorTheme.Instance.GetTheme());
        }
        else
        {
            numText.color = ColorSetter.GiveColorButtons(ColorTheme.Instance.GetTheme());
        }
    }

    public void SetNotes(int num)
    {
        numText.text = " ";
        theNumber = 0;
        notes[num - 1].GetComponent<Notes>().SetNotesActive(!notes[num - 1].GetComponent<Notes>().IsNotesActive());
    }

    public void SetNotes()
    {
        if (BoardDataSave.GameDataFileExists())
        {
            if (grid.IsContinued() && BoardDataSave.GetNotesData(IndexNumber).Count != 0)
            {
                foreach (int not in BoardDataSave.GetNotesData(IndexNumber))
                {
                    Debug.Log(BoardDataSave.GetNotesData(IndexNumber).Count);
                    notes[not - 1].GetComponent<Notes>().SetNotesActive(true);
                }
            }
        }
    }

    public int GetNumber()
    {
        return theNumber;
    }

    public bool IsDefaultNum()
    {
        return this.isDefaultNum;
    }

    public void SetActiveNumber(bool value)
    {
        isActiveNumber = value;
        numText.gameObject.SetActive(value);
    }

    private void OnMouseDown()
    {
    }

    protected override void Start()
    {
        this.isSelected = false;
        SpawnNotes();
        SetPositionNotes();
        SetNotes();
    }

    public void SpawnNotes()
    {
        int index = 0;
        for (int i = 0; i < 9; i++)
        {
            notes.Add(Instantiate(noteBox) as Text);
            notes[notes.Count - 1].GetComponent<Notes>().SetNotesIndex(index);
            notes[notes.Count - 1].GetComponent<Notes>().SetNotesActive(false);
            notes[notes.Count - 1].transform.SetParent(transform);
            notes[notes.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            notes[notes.Count - 1].transform.localPosition = new Vector3(0, 0, 0);

            index++;
        }
    }

    public void SetPositionNotes()
    {
        int index = 0;
        for (int i = 33; i >= -33; i -= 33)
        {
            for (int j = -33; j <= 33; j += 33)
            {
                notes[index].GetComponent<RectTransform>().anchoredPosition = new Vector2(j, i);
                index++;
            }
        }
    }

    public void SetNotesActive(bool value)
    {
        //isNotesActive = value;

        foreach (var note in notes)
        {
            note.GetComponent<Notes>().SetNotesActive(value);
        }
    }

    public List<int> GetActiveNotes()
    {
        List<int> output = new List<int>();

        foreach (var note in notes)
        {
            if (note.GetComponent<Notes>().IsNotesActive())
            {
                output.Add(note.GetComponent<Notes>().GetNotesIndex() + 1);
            }
        }

        return output;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.isSelected = true;

        Events.SetIndex(IndexNumber);
        grid.Instance.ClickedBox(this.Dimentions, this.IndexNumber);
    }

    public void OnSubmit(BaseEventData eventData)
    {
    }

    public void SetBoxDimention(int i, int j)
    {
        this.Dimentions[0] = i;
        this.Dimentions[1] = j;
    }

    public int[] GetDimentions()
    {
        return this.Dimentions;
    }

    public void SetBoxColor(Color color)
    {
        this.GetComponent<Image>().color = color;
    }
}