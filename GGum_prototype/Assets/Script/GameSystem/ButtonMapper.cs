using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class ButtonMapper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    bool _isDown = false;
    bool _isPress = false;
    bool _isUp = false;

    float _keepTime;

    public EButtonCode _buttonCode;

    public bool _downMapping = false;
    public bool _PressMapping = false;
    public bool _UpMapping = false;

    // Use this for initialization
    void Start()
    {

        _keepTime = Time.deltaTime;

        if (_downMapping)
            ButtonManager.RegisterButtonDownFunc(_buttonCode, IsDown);

        if (_PressMapping)
            ButtonManager.RegisterButtonPressFunc(_buttonCode, IsPress);

        if (_UpMapping)
            ButtonManager.RegisterButtonUpFunc(_buttonCode, IsUp);
    }

    void Update()
    {
        if (_keepTime <= 0)
        {
            _keepTime = Time.deltaTime;
            if (_isDown)
                _isDown = false;
            if (_isUp)
                _isUp = false;
        }
        else
        {
            _keepTime -= Time.deltaTime;
        }
    }

    void OnDestroy()
    {
        if (_downMapping)
            ButtonManager.RemoveButtonDwonFunc(_buttonCode, IsDown);
        if (_PressMapping)
            ButtonManager.RemoveButtonPressFunc(_buttonCode, IsPress);
        if (_UpMapping)
            ButtonManager.RemoveButtonUpFunc(_buttonCode, IsUp);
    }

    public bool IsDown()
    {
        return _isDown;
    }

    public bool IsPress()
    {
        return _isPress;
    }

    public bool IsUp()
    {
        return _isUp;
    }

    public void OnPointerDown(PointerEventData data)
    {
        _isDown = true;
        _isPress = true;

        _keepTime = Time.deltaTime;
    }

    public void OnPointerUp(PointerEventData data)
    {
        _isPress = false;
    }

}
