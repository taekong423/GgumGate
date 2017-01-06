using UnityEngine;
using System.Collections;
using System;

public class ChanageStageEvent : Event {


    #region 변수

    [SerializeField]
    StageID _nextStage;

    #endregion

    #region 메소드

    public override void Execute()
    {
        GameManager.ChangeStage(_nextStage);
    }

    public override void Execute(Event_Type eventType, Component sender, object param)
    {
        throw new NotImplementedException();
    }

    #endregion

}
