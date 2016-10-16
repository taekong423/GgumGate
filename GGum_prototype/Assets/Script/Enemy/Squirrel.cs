using UnityEngine;
using System.Collections;

public partial class Squirrel : Enemy {

    public bool isTuto = false;

    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(InitState), new InitState(this));
        _statePatternList.Add(typeof(IdleState), new IdleState(this, new Search_Chase_Attack(this)));
        _statePatternList.Add(typeof(MoveState), new MoveState(this, new Search_Chase_Attack(this)));
        _statePatternList.Add(typeof(ChaseState), new ChaseState(this, new Search_Move_Attack(this)));
        _statePatternList.Add(typeof(AttackState), new AttackState(this, new Search_Move_Chase(this)));
        _statePatternList.Add(typeof(HitState), new HitState(this, 1.5f, 0.5f, new NoSearch()));
        _statePatternList.Add(typeof(DeadState), new DeadState(this, new NoSearch()));

        if (isTuto)
        {
            _statePatternList.Add(typeof(CinematicState), new CinematicState(this, new NoSearch()));
            SetStatePattern<CinematicState>();
        }
        else
            SetStatePattern<InitState>();

        //_statePatternList.Add(typeof(Normal), new Normal(this));
        //_statePatternList.Add(typeof(Tutorial), new Tutorial(this));


        //SetStatePattern<Normal>();


    }

    protected override void Attack(HitData hitInfo)
    {
        _player.OnHit(hitInfo);
    }

    class CinematicState : EnemyState
    {
        GameManager _gm;

        public CinematicState(Enemy enemy, Searchable searchable) : base(enemy, searchable)
        {
        }

        protected override IEnumerator Enter()
        {
            _enemy.isInvincible = true;
            _gm = GameObject.Find("GameManager").GetComponent<GameManager>();

            yield return null;
        }

        protected override IEnumerator Execute()
        {
            while (!_gm.flags[_gm.flagKeys[0]])
            {
                yield return null;
            }

            yield return null;

            _enemy.animator.SetTrigger("TutoSturn");

            yield return new WaitForSeconds(2.0f);

        }

        protected override IEnumerator Exit()
        {
            _enemy.SetStatePattern<IdleState>();
            _enemy.isInvincible = false;

            yield return null;

            _enemy._statePattern.StartState();
        }
    }


}
