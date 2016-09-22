using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NewButtonData {

    List<ButtonFunction> _buttonDownFunc;
    List<ButtonFunction> _buttonPressFunc;
    List<ButtonFunction> _buttonUpFunc;

    List<AxisFunction> _axisFunc;
    List<AxisFunction> _axisRawFunc;

    public delegate bool ButtonFunction();
    public delegate float AxisFunction();

    [Header("Set Button Code Or Name")]
    public EButtonCode _buttonCode;
    public string _buttonName;

    [Header("Key Mapping")]
    public List<KeyCode> _keyCodes;
    public List<string> _keyNames;

    NewButtonData()
    {
        if (_buttonDownFunc == null)
            _buttonDownFunc = new List<ButtonFunction>();

        if (_buttonPressFunc == null)
            _buttonPressFunc = new List<ButtonFunction>();

        if (_buttonUpFunc == null)
            _buttonUpFunc = new List<ButtonFunction>();

        if (_axisFunc == null)
            _axisFunc = new List<AxisFunction>();

        if (_axisRawFunc == null)
            _axisRawFunc = new List<AxisFunction>();


    }

    bool GetDown()
    {
        if (_keyCodes.Count <= 0 && _keyNames.Count <= 0 && _buttonDownFunc.Count <= 0)
            return false;

        for (int i = 0; i < _keyCodes.Count; i++)
        {
            if (Input.GetKeyDown(_keyCodes[i]))
            {
                return true;
            }
        }

        for (int i = 0; i < _keyNames.Count; i++)
        {
            if (Input.GetKeyDown(_keyNames[i]))
            {
                return true;
            }
        }

        for (int i = 0; i < _buttonDownFunc.Count; i++)
        {
            if (_buttonDownFunc[i].Invoke())
            {
                return true;
            }
        }

        return false;
    }

    bool HasDwonFunc(ButtonFunction func)
    {
        return _buttonDownFunc.Contains(func);
    }

    public void RegisterButtonDownFunc(ButtonFunction func)
    {
        if (!HasDwonFunc(func))
        {
            _buttonDownFunc.Add(func);
        }
    }

    public void RemoveButtonDwonFunc(ButtonFunction func)
    {
        if (HasDwonFunc(func))
        {
            _buttonDownFunc.Remove(func);
        }
    }

    public bool GetButtonDown()
    {
        return GetDown();
    }
   
}
