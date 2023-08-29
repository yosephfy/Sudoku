using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesNumbers : ActionsInterface
{
    private int Index, Number, prevNumber;

    public NotesNumbers(int ind, int num, int prevNum)
    {
        this.Index = ind;
        this.Number = num;
        this.prevNumber = prevNum;
    }

    public int GetIndex()
    {
        return this.Index;
    }

    public int GetNumber()
    {
        return this.Number;
    }

    public int GetPrevNumber()
    {
        return this.prevNumber;
    }

    public void SetIndex(int ind)
    {
        this.Index = ind;
    }

    public void SetNumber(int num)
    {
        this.Number = num;
    }

    public void SetPrevNumber(int num)
    {
        this.prevNumber = num;
    }
}