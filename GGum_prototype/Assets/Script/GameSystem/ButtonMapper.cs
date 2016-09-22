using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class ButtonMapper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    bool _isDown = false;
    bool _isPress = false;

    public EButtonCode _buttonCode;

    public bool _downMapping = false;
    public bool _pressMapping = false;
    

	// Use this for initialization
	void Start () {

        if(_downMapping)
            ButtonManager.This.RegisterDown(_buttonCode, IsDown);
        if (_pressMapping)
            ButtonManager.This.RegisterPress(_buttonCode, IsPress);


    }
	

    public bool IsDown()
    {
        bool isDown = _isDown;

        if (isDown)
        {
            _isDown = false;
            return isDown;
        }
        else
            return isDown;
    }

    public bool IsPress()
    {
        return _isPress;
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("Down!!");
        _isDown = true;
        _isPress = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        _isPress = false;
    }

}
