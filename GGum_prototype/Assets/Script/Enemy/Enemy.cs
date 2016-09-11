using UnityEngine;
using System.Collections;
using System.Reflection;

public class Enemy : AICharacter {

    protected HitData _hitinfo;

    protected bool _isHitEffectDelay = false;

    public float AttackDelay = 1.0f;

    // Use this for initialization
    void Awake () {
        animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindObjectOfType<PlayerCharacter>();
	}

    void OnEnable()
    {
        StartCoroutine(InitState());
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

            yield return null;
        }

        yield return null;
    }
}
