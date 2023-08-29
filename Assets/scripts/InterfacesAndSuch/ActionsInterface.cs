using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActionsInterface
{
    public int GetPrevNumber();

    public void SetPrevNumber(int num);

    public int GetIndex();

    public void SetIndex(int ind);

    public int GetNumber();

    public void SetNumber(int num);
}