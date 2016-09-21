using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class CallBackFunction : UnityEvent { }


public class TriggerChecker : MonoBehaviour {

    public string _tag;

    public bool _EnterCheck = false;
    public bool _StayCheck = false;
    public bool _ExitCheck = false;

    public CallBackFunction _callBack;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == _tag)
        {
            if (_EnterCheck)
            {
                _callBack.Invoke();
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == _tag)
        {
            if (_StayCheck)
            {
                _callBack.Invoke();
            }
        }
    }

}
