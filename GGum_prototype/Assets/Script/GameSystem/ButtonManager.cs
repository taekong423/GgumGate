using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{

    static string _prefabPath = "Prefab/Manager/ButtonManager";

    static ButtonManager _instance;

    static object synLock = new object();

    public List<ButtonData> _buttonDatas;


    public static ButtonManager This
    {
        get
        {
            if (_instance == null)
            {
                lock (synLock)
                {
                    GameObject prefab = Resources.Load<GameObject>(_prefabPath);

                    GameObject obj = GameObject.Instantiate(prefab);

                    obj.name = prefab.name;

                }
            }

            return _instance;

        }
    }

    // Use this for initialization
    void Awake()
    {

        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    ButtonData GetButtonData(EButtonCode buttonCode)
    {
        if (_buttonDatas.Count <= 0)
            return null;

        for (int i = 0; i < _buttonDatas.Count; i++)
        {
            if (_buttonDatas[i]._buttonCode == buttonCode)
            {
                return _buttonDatas[i];
            }
        }

        return null;
    }

    //Register, Remove

    public static void RegisterButtonDownFunc(EButtonCode buttonCode, ButtonData.ButtonFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RegisterButtonDownFunc(func);
        }

    }

    public static void RemoveButtonDwonFunc(EButtonCode buttonCode, ButtonData.ButtonFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RemoveButtonDwonFunc(func);
        }
    }

    public static void RegisterButtonPressFunc(EButtonCode buttonCode, ButtonData.ButtonFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RegisterButtonPressFunc(func);
        }

    }

    public static void RemoveButtonPressFunc(EButtonCode buttonCode, ButtonData.ButtonFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RemoveButtonPressFunc(func);
        }
    }

    public static void RegisterButtonUpFunc(EButtonCode buttonCode, ButtonData.ButtonFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RegisterButtonUpFunc(func);
        }

    }

    public static void RemoveButtonUpFunc(EButtonCode buttonCode, ButtonData.ButtonFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RemoveButtonUpFunc(func);
        }
    }

    public static void RegisterAxisFunc(EButtonCode buttonCode, ButtonData.AxisFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RegisterAxisFunc(func);
        }

    }

    public static void RemoveAxisFunc(EButtonCode buttonCode, ButtonData.AxisFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RemoveAxisFunc(func);
        }
    }

    public static void RegisterAxisRawFunc(EButtonCode buttonCode, ButtonData.AxisFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RegisterAxisRawFunc(func);
        }

    }

    public static void RemoveAxisRawFunc(EButtonCode buttonCode, ButtonData.AxisFunction func)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data != null)
        {
            data.RemoveAxisRawFunc(func);
        }
    }


    //ButtonManager Interface

    public static bool ButtonDown(EButtonCode buttonCode)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return false;
        }

        return data.GetButtonDown();
    }

    public static bool ButtonPress(EButtonCode buttonCode)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return false;
        }

        return data.GetButtonPress();
    }

    public static bool ButtonUp(EButtonCode buttonCode)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return false;
        }

        return data.GetButtonUp();
    }

    public static float GetAxis(EButtonCode buttonCode)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return 0.0f;
        }

        return data.GetAxis();
    }

    public static float GetAxisRaw(EButtonCode buttonCode)
    {
        ButtonData data = This.GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return 0.0f;
        }

        return data.GetAxisRaw();
    }
}


