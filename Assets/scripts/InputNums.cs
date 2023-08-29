using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputNums : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public static InputNums Instance;

    public Text inputNums;
    private int inputIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        grid.Instance.SetUserInput(inputIndex, Events.GetIndex());
    }

    public void OnSubmit(BaseEventData eventData)
    {
    }

    public void SetNum(int num)
    {
        if (num != 0)
            inputNums.text = num.ToString();
        inputIndex = num;

        inputNums.color = ColorSetter.GiveColorButtons(ColorTheme.Instance.GetTheme());
    }

    private void OnMouseDown()
    {
    }
}