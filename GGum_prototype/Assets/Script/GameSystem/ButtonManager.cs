using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EButtonCode
{
    MoveX,
    MoveY,
    Attack,
    Jump,
}

[System.Serializable]
public class ButtonData
{
    public delegate float AxisFunction();

    [Header("Set Button Code Or Name")]
    public EButtonCode _buttonCode;
    public string _buttonName;

    [Header("Key Mapping")]
    List<AxisFunction> _KeyAxis;
    public List<KeyCode> _keyCodes;
    public List<string> _keyNames;

    public void RegisterAxis(AxisFunction func)
    {
        if (_KeyAxis == null)
            _KeyAxis = new List<AxisFunction>();

        _KeyAxis.Add(func);
    }

    public bool GetButtonDown()
    {
        if (_keyCodes.Count <= 0)
            return false;

        for (int i = 0; i < _keyCodes.Count; i++)
        {
            if (Input.GetKeyDown(_keyCodes[i]))
            {
                return true;
            }
        }

        return false;

    }

    public float GetAxis()
    {
        if (_keyNames.Count <= 0)
            return 0.0f;

        for (int i = 0; i < _keyNames.Count; i++)
        {
            if (Input.GetAxis(_keyNames[i]) != 0.0f)
            {
                return Input.GetAxis(_keyNames[i]);
            }
        }

        return 0.0f;
    }

    public float GetAxisFunction()
    {
        if (_KeyAxis == null || _KeyAxis.Count <= 0)
            return 0.0f;

        for (int i = 0; i < _KeyAxis.Count; i++)
        {
            if (_KeyAxis[i].Invoke() != 0.0f)
            {
                return _KeyAxis[i].Invoke();
            }
        }

        return 0.0f;
    }

}

public class ButtonManager : MonoBehaviour {

    static ButtonManager _instance;

    public ButtonData[] _buttonDatas;


    public ButtonManager()
    {
        _instance = this;
    }

    public static ButtonManager This
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("ButtonManager");
                _instance = obj.AddComponent<ButtonManager>();
            }

            return _instance;
        }
    }

    ButtonData GetButtonData(EButtonCode buttonCode)
    {
        if (_buttonDatas.Length <= 0)
            return null;

        for (int i = 0; i < _buttonDatas.Length; i++)
        {
            if (_buttonDatas[i]._buttonCode == buttonCode)
            {
                return _buttonDatas[i];
            }
        }

        return null;
    }

    public void RegisterAxis(EButtonCode buttonCode, ButtonData.AxisFunction func)
    {
        ButtonData data = GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return;
        }

        data.RegisterAxis(func);
    }

    public bool GetButtonDown(EButtonCode buttonCode)
    {
        ButtonData data = GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return false;
        }

        return data.GetButtonDown();
    }

    public float GetButtonAxis(EButtonCode buttonCode)
    {
        ButtonData data = GetButtonData(buttonCode);

        if (data == null)
        {
            Debug.Log("ButtonData == null");
            return 0.0f;
        }

        if (data.GetAxisFunction() != 0.0f)
            return data.GetAxisFunction();
        else
            return data.GetAxis();
    }
}
