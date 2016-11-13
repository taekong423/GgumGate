using UnityEngine;
using System.Collections;

public class NewSquirrel : NewEnemy {

    [Header("Squirrel Setting")]
    public bool _isTuto;

    void OnEnable()
    {
        if (_isTuto)
            SetState("Cinematic");
        else
            SetState("Idle");
    }

    protected override void SetStateList()
    {
        _stateList.Add("Idle", new NewIdleState(this, "Idle", _stayTime, "Move"));
        _stateList.Add("Move", new NewMoveState(this, "Move", "Idle"));
        _stateList.Add("Chase", new NewChaseState(this, "Chase"));
        _stateList.Add("AttackIdle", new AttackIdleState(this, "Idle"));
        _stateList.Add("Attack", new NewAttackState(this, "Attack"));

        if (_isTuto)
            _stateList.Add("Cinematic", new CinematicState(this, "Cinematic"));

    }

    class CinematicState : State
    {
        readonly NewEnemy _enemy;
        readonly GameManager _gm;

        float _delay;
        bool _onCineatic = false;

        public CinematicState(NewEnemy enemy, string id) : base(id)
        {
            _enemy = enemy;
            _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public override void Enter()
        {
            _enemy.IsInvincible = true;
            _delay = 2;
        }

        public override void Excute()
        {

            if (_gm.flags[_gm.flagKeys[0]])
            {
                if (!_onCineatic)
                {
                    _onCineatic = true;
                    _enemy.PlayAnimation("TutoSturn");
                }
                else
                {

                    _delay -= Time.fixedDeltaTime;

                    if (_delay <= 0)
                    {
                        _enemy.SetState("Idle");
                    }

                }
            }

        }

        public override void Exit()
        {
            _enemy.IsInvincible = false;
        }

    }

}
