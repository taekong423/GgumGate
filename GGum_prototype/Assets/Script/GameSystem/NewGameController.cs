using UnityEngine;
using System.Collections;

public class NewGameController : MonoBehaviour {

    static NewGameController _instance;

    static object synLock = new object();

    public static NewGameController This
    {
        get
        {
            if (_instance == null)
            {
                lock (synLock)
                {
                    _instance = new NewGameController();
                }
            }

            return _instance;
        }
    }


    public static bool ButtonDown(EButtonCode code)
    {
        return NewButtonManager.ButtonDown(code);
    }

}
