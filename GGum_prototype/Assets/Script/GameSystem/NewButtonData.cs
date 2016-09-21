using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NewButtonData : MonoBehaviour {

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



    public void RegisterButtonDownFunc(ButtonFunction func)
    {

    }
   
}
