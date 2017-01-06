using UnityEngine;

public abstract class Event : MonoBehaviour
{
    #region 추상 메소드

    // 이벤트 실행 함수, 이벤트 트리거용
    public abstract void Execute();

    // 이벤트 실행 함수, 이벤트 리스너용
    public abstract void Execute(Event_Type eventType, Component sender, object param);

    #endregion
}
