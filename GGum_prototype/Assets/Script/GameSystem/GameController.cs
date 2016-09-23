using UnityEngine;
using System.Collections;

public class GameController {

    static GameController _instance;

    static object synLock = new object();

    public static GameController This
    {
        get
        {
            if (_instance == null)
            {
                lock (synLock)
                {
                    _instance = new GameController();
                }
            }

            return _instance;
        }
    }


    public static bool ButtonDown(EButtonCode code)
    {
        return ButtonManager.ButtonDown(code);
    }

    public static bool ButtonPress(EButtonCode code)
    {
        return ButtonManager.ButtonPress(code);
    }

    public static bool ButtonUp(EButtonCode code)
    {
        return ButtonManager.ButtonUp(code);
    }

    public static float GetAxis(EButtonCode code)
    {
        return ButtonManager.GetAxis(code);
    }

    public static float GetAxisRaw(EButtonCode code)
    {
        return ButtonManager.GetAxisRaw(code);
    }

}

