using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ButtonData {

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

    ButtonData()
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

    bool GetPress()
    {
        if (_keyCodes.Count <= 0 && _keyNames.Count <= 0 && _buttonPressFunc.Count <= 0)
            return false;

        for (int i = 0; i < _keyCodes.Count; i++)
        {
            if (Input.GetKey(_keyCodes[i]))
            {
                return true;
            }
        }

        for (int i = 0; i < _keyNames.Count; i++)
        {
            if (Input.GetKey(_keyNames[i]))
            {
                return true;
            }
        }

        for (int i = 0; i < _buttonPressFunc.Count; i++)
        {
            if (_buttonPressFunc[i].Invoke())
            {
                return true;
            }
        }

        return false;
    }

    bool GetUp()
    {
        if (_keyCodes.Count <= 0 && _keyNames.Count <= 0 && _buttonUpFunc.Count <= 0)
            return false;

        for (int i = 0; i < _keyCodes.Count; i++)
        {
            if (Input.GetKeyUp(_keyCodes[i]))
            {
                return true;
            }
        }

        for (int i = 0; i < _keyNames.Count; i++)
        {
            if (Input.GetKeyUp(_keyNames[i]))
            {
                return true;
            }
        }

        for (int i = 0; i < _buttonUpFunc.Count; i++)
        {
            if (_buttonUpFunc[i].Invoke())
            {
                return true;
            }
        }

        return false;
    }

    float GetAxisData()
    {
        if (_keyNames.Count <= 0 && _axisFunc.Count <= 0)
            return 0.0f;

        for (int i = 0; i < _keyNames.Count; i++)
        {
            if (Input.GetAxis(_keyNames[i]) != 0.0f)
            {
                return Input.GetAxis(_keyNames[i]);
            }
        }

        for (int i = 0; i < _axisFunc.Count; i++)
        {
            if (_axisFunc[i].Invoke() != 0.0f)
            {
                return _axisFunc[i].Invoke();
            }
        }

        return 0.0f;

    }

    float GetAxisRawData()
    {
        if (_keyNames.Count <= 0 && _axisRawFunc.Count <= 0)
            return 0.0f;

        for (int i = 0; i < _keyNames.Count; i++)
        {
            if (Input.GetAxisRaw(_keyNames[i]) != 0.0f)
            {
                return Input.GetAxisRaw(_keyNames[i]);
            }
        }

        for (int i = 0; i < _axisRawFunc.Count; i++)
        {
            if (_axisRawFunc[i].Invoke() != 0.0f)
            {
                return _axisRawFunc[i].Invoke();
            }
        }

        return 0.0f;

    }


    //Register, Remove, Has

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

    bool HasPressFunc(ButtonFunction func)
    {
        return _buttonPressFunc.Contains(func);
    }

    public void RegisterButtonPressFunc(ButtonFunction func)
    {
        if (!HasPressFunc(func))
        {
            _buttonPressFunc.Add(func);
        }
    }

    public void RemoveButtonPressFunc(ButtonFunction func)
    {
        if (HasPressFunc(func))
        {
            _buttonPressFunc.Remove(func);
        }
    }

    bool HasUpFunc(ButtonFunction func)
    {
        return _buttonUpFunc.Contains(func);
    }

    public void RegisterButtonUpFunc(ButtonFunction func)
    {
        if (!HasUpFunc(func))
        {
            _buttonUpFunc.Add(func);
        }
    }

    public void RemoveButtonUpFunc(ButtonFunction func)
    {
        if (HasUpFunc(func))
        {
            _buttonUpFunc.Remove(func);
        }
    }


    bool HasAxisFunc(AxisFunction func)
    {
        return _axisFunc.Contains(func);
    }

    public void RegisterAxisFunc(AxisFunction func)
    {
        if (!HasAxisFunc(func))
        {
            _axisFunc.Add(func);
        }
    }

    public void RemoveAxisFunc(AxisFunction func)
    {
        if (HasAxisFunc(func))
        {
            _axisFunc.Remove(func);
        }
    }

    bool HasAxisRawFunc(AxisFunction func)
    {
        return _axisRawFunc.Contains(func);
    }

    public void RegisterAxisRawFunc(AxisFunction func)
    {
        if (!HasAxisRawFunc(func))
        {
            _axisRawFunc.Add(func);
        }
    }

    public void RemoveAxisRawFunc(AxisFunction func)
    {
        if (HasAxisRawFunc(func))
        {
            _axisRawFunc.Remove(func);
        }
    }

    //ButtonData Interface

    public bool GetButtonDown()
    {
        return GetDown();
    }

    public bool GetButtonPress()
    {
        return GetPress();
    }

    public bool GetButtonUp()
    {
        return GetUp();
    }

    public float GetAxis()
    {
        return GetAxisData();
    }

    public float GetAxisRaw()
    {
        return GetAxisRawData();
    }

}
