using UnityEngine;
using System.Collections;

public class GameButton {

    ButtonManager _buttonManager;

    public GameButton()
    {
        _buttonManager = ButtonManager.This;
    }

    //ButtonCode : EButtonCode

    public bool ButtonDown(EButtonCode buttonCode)
    {
        return _buttonManager.GetButtonDown(buttonCode);
    }

    public bool ButtonPress(EButtonCode buttonCode)
    {
        return false;
    }

    public bool ButtonUp(EButtonCode buttonCode)
    {
        return false;
    }

    //ButtonName : String

    public bool ButtonDown(string buttonName)
    {
        return false;
    }

    public bool ButtonPress(string buttonName)
    {
        return false;
    }

    public bool ButtonUp(string buttonName)
    {
        return false;
    }


    public float ButtonAxis(EButtonCode buttonCode)
    {
        return _buttonManager.GetButtonAxis(buttonCode);
    }
}
