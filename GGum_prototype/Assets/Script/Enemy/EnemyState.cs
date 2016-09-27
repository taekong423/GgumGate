using UnityEngine;
using System.Collections;
using System;

public partial class Enemy {


    public class EnemyState : StatePattern
    {
        protected Enemy _enemy;

        public EnemyState(Enemy enemy) : base(enemy)
        {
            _enemy = enemy;
        }

        public override void StartState()
        {
            throw new NotImplementedException();
        }

        public virtual void SearchUpdate()
        {

        }

        protected virtual IEnumerator SearchUpdate<T>() where T : IConvertible
        {
            float attackDelay = 0;

            while (_currentState != "Dead")
            {
                if (_currentState == "Idle" || _currentState == "Move")
                {
                    if (_enemy.Search(_enemy._player.transform, _enemy._attackRange))
                    {
                        if (attackDelay <= 0.0f)
                        {
                            SetState("Attack");
                            attackDelay = _enemy.AttackDelay;
                        }
                        else
                            attackDelay -= Time.deltaTime;

                    }
                    else if (_enemy.Search(_enemy._player.transform, _enemy._detectionRange))
                    {

                        SetState("Move");
                        attackDelay = _enemy.AttackDelay;
                        _enemy._target = _enemy._player.transform;
                        _enemy._currentMoveDleay = _enemy._moveDelay;

                    }
                    else
                    {
                        attackDelay = _enemy.AttackDelay;

                        if (_enemy._target == _enemy._player.transform && _enemy._wayPoints.Length != 0)
                        {

                            _enemy._target = _enemy._wayPoints[_enemy._numWayPoint];
                        }

                    }

                }
                else
                {
                    Debug.Log("Attack And Hit");
                }

                yield return null;
            }

            yield return null;
        }

    }

}
