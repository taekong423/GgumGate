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
        return _buttonManager.GetButtonPress(buttonCode);
    }

    public bool ButtonUp(EButtonCode buttonCode)
    {
        return _buttonManager.GetButtonUp(buttonCode);
    }

    //ButtonName : String

    public bool ButtonDown(string buttonName)
    {
        return _buttonManager.GetButtonDown(buttonName);
    }

    public bool ButtonPress(string buttonName)
    {
        return _buttonManager.GetButtonPress(buttonName);
    }

    public bool ButtonUp(string buttonName)
    {
        return _buttonManager.GetButtonUp(buttonName);
    }


    public float ButtonAxis(EButtonCode buttonCode)
    {
        return _buttonManager.GetButtonAxis(buttonCode);
    }
}
