using UnityEngine;
using System.Collections;


public class EventTrigger : MonoBehaviour
{

    #region 변수

    Event[] _eventList;

    bool _once = false;

    public bool _isOnce = true;

    public string _targetTag;

    public bool _checkEnter = false;
    public bool _checkStay = false;
    public bool _checkExit = false;

    #endregion 

    #region 프로퍼티
    #endregion

    #region 메소드

    void Awake()
    {
        _eventList = GetComponents<Event>();
    }

    // 이벤트 리스트의 모든 이벤트를 실행하는 함수
    void Execute()
    {
        foreach (Event entity in _eventList)
        {
            entity.Execute();
        }
    }

    #region Trigger

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isOnce && _once)
            return;

        if (_checkEnter)
        {
            if (collision.CompareTag(_targetTag))
            {
                _once = true;
                Execute();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (_isOnce && _once)
            return;

        if (_checkStay)
        {
            if (collision.CompareTag(_targetTag))
            {
                _once = true;
                Execute();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (_isOnce && _once)
            return;

        if (_checkExit)
        {
            if (collision.CompareTag(_targetTag))
            {
                _once = true;
                Execute();
            }
        }
    }

    #endregion

    #region Collision

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isOnce && _once)
            return;

        if (_checkEnter)
        {
            if (collision.gameObject.CompareTag(_targetTag))
            {
                _once = true;
                Execute();
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (_isOnce && _once)
            return;

        if (_checkStay)
        {
            if (collision.gameObject.CompareTag(_targetTag))
            {
                _once = true;
                Execute();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (_isOnce && _once)
            return;

        if (_checkExit)
        {
            if (collision.gameObject.CompareTag(_targetTag))
            {
                _once = true;
                Execute();
            }
        }
    }

    #endregion

    #endregion
}

