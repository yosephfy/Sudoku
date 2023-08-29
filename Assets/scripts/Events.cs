using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    private static int selectedIndex;

    public static void SetIndex(int ind)
    {
        selectedIndex = ind;
    }

    public static int GetIndex()
    {
        return selectedIndex;
    }
}