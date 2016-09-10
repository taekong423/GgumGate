using UnityEngine;
using System.Collections;
using System.Reflection;

public class Enemy : AICharacter {

    protected Animator _anim;
    protected HitInfo _hitinfo;


    public float AttackDelay = 1.0f;

	// Use this for initialization
	void Awake () {
        _player = GameObject.FindObjectOfType<PlayerCharacter>();
        _anim = GetComponentInChildren<Animator>();
	}

    void OnEnable()
    {
        StartCoroutine(InitState());
    }

    protected virtual IEnumerator InitState()
    {

        yield return null;
    }

    protected virtual IEnumerator IdleState()
    {
        
        yield return null;
    }

    protected virtual IEnumerator MoveState()
    {

        yield return null;
    }

    protected virtual IEnumerator AttackState()
    {
        yield return null;
    }

    protected virtual IEnumerator HitState()
    {



        yield return null;
    }

    protected virtual IEnumerator DeadState()
    {



        yield return null;
    }

    protected virtual IEnumerator Search()
    {
        float attackDelay = 0;

        while (state != State.Dead)
        {
            if (state == State.Idle || state == State.Move)
            {

                if (Search(_player.transform, _attackRange))
                {
                    if (attackDelay <= 0.0f)
                    {
                        state = State.Attack;
                        attackDelay = AttackDelay;
                    }
                    else
                        attackDelay -= Time.deltaTime;

                }
                else if (Search(_player.transform, _detectionRange))
                {

                    state = State.Move;
                    attackDelay = AttackDelay;
                    _target = _player.transform;
                    _currentMoveDleay = _moveDelay;

                }
                else
                {
                    attackDelay = AttackDelay;

                    if (_target == _player.transform && _wayPoints.Length != 0)
                    {

                            _target = _wayPoints[_numWayPoint];
                    }

                }

            }

            //if (state != State.Attack && state != State.Hit)
            //{

            //    if (Search(_player.transform, _detectionRange))
            //    {
            //        _currentMoveDleay = _moveDelay;
            //        _target = _player.transform;
            //        state = State.Move;
            //    }
            //    else
            //    {
            //        if (_target == _player.transform)
            //        {
            //            if (_wayPoints.Length != 0)
            //                _target = _wayPoints[_numWayPoint];
            //        }
            //    }

            //}

            yield return null;
        }

        yield return null;
    }

    public void NextState()
    {
        string methodName = state.ToString() + "State";
        MethodInfo info = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

}
