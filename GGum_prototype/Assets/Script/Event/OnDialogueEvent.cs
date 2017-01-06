using UnityEngine;
using System.Collections;
using System;

public class OnDialogueEvent : Event {

    #region 변수

    [SerializeField]
    DialogueID _ID;

    #endregion

    #region 메소드

    // 트리거용
    public override void Execute()
    {
        Dialogue.Display(_ID);
    }

    // 리스너용
    public override void Execute(Event_Type eventType, Component sender, object param)
    {
        if (eventType != Event_Type.OnDialogue)
        {
            Dialogue.Display(_ID);
        }
    }

    #endregion
}
