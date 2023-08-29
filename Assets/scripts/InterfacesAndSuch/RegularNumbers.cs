using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularNumbers : ActionsInterface
{
    private int index, number, prevNumber;

    public RegularNumbers(int ind, int num, int prevNum)
    {
        this.index = ind;
        this.number = num;
        this.prevNumber = prevNum;
    }

    public int GetIndex()
    {
        return this.index;
    }

    public int GetNumber()
    {
        return this.number;
    }

    public int GetPrevNumber()
    {
        return this.prevNumber;
    }

    public void SetIndex(int ind)
    {
        this.index = ind;
    }

    public void SetNumber(int num)
    {
        this.number = num;
    }

    public void SetPrevNumber(int num)
    {
        this.prevNumber = num;
    }
}