using UnityEngine;
using System.Collections;

public class GameController {

    static GameController _instance;

    GameButton _gameButton;

    public static GameController This
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameController();
            }

            return _instance;
        }

    }

    public GameController()
    {
        _gameButton = new GameButton();
    }

    //ButtonCode : EButtonCode

    public bool ButtonDown(EButtonCode buttonCode)
    {
        return _gameButton.ButtonDown(buttonCode);
    }

    public bool ButtonPress(EButtonCode buttonCode)
    {
        return _gameButton.ButtonPress(buttonCode);
    }

    public bool ButtonUp(EButtonCode buttonCode)
    {
        return _gameButton.ButtonUp(buttonCode);
    }


    //ButtonName : String

    public bool ButtonDown(string buttonName)
    {
        return _gameButton.ButtonDown(buttonName);
    }

    public bool ButtonPress(string buttonName)
    {
        return _gameButton.ButtonPress(buttonName);
    }

    public bool ButtonUp(string buttonName)
    {
        return _gameButton.ButtonUp(buttonName);
    }


    //Axis

    public float ButtonAxis(EButtonCode buttonCode)
    {
        return _gameButton.ButtonAxis(buttonCode);
    }

    public float ButtonAxisRaw(EButtonCode buttonCode)
    {
        return _gameButton.ButtonAxisRaw(buttonCode);
    }
}

