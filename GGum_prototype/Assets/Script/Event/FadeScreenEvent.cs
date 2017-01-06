using UnityEngine;
using System.Collections;
using System;

public class FadeScreenEvent : Event {


    #region 변수

    [SerializeField]
    float _fadeTime = 1.0f;
    [SerializeField]
    bool _fadeIn = true;

    #endregion

    #region 메소드

    public override void Execute()
    {
        CameraController.Fade(_fadeTime, _fadeIn);
    }

    public override void Execute(Event_Type eventType, Component sender, object param)
    {
        if (eventType != Event_Type.FadeScreen)
        {
            CameraController.Fade(_fadeTime, _fadeIn);
        }
    }

    #endregion
}
