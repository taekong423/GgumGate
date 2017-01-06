using UnityEngine;
using System.Collections;

public class EventListener : MonoBehaviour
{

    #region 변수

    Event[] _eventList;

    public Event_Type _type;

    public Component _target;

    #endregion

    #region 프로퍼티
    #endregion

    #region 메소드

    void Awake()
    {
        _eventList = GetComponents<Event>();
    }

    void Start()
    {
        EventManager.AddListener(_type, Execute);
    }

    // 리스트의 모든 이벤트를 실행함
    void Execute(Component sender, object param = null)
    {
        if (_target != null && sender != _target)
            return;

        foreach (Event entity in _eventList)
        {
            //entity.Execute();
        }
    }

    #endregion
} 

