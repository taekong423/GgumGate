using UnityEngine;
using System.Collections;

public partial class BossPig {

    //public class BossPigState : EnemyState
    //{
    //    public enum Stat
    //    {
    //        Init,
    //        Idle,
    //        Move,
    //        Attack,
    //        Hit,
    //        Dead,
    //    }


    //    protected BossPig _bossPig;

    //    protected Stat _state;

    //    protected CameraController _camera;

    //    public BossPigState(Enemy enemy) : base(enemy)
    //    {
    //        _bossPig = enemy as BossPig;
    //    }

    //    public override void SetState(string value)
    //    {
    //        SetState<Stat>(ref _state, value);
    //    }

    //    protected IEnumerator IdleStay()
    //    {
    //        while (_state == Stat.Idle)
    //        {
    //            if (_bossPig._currentMoveDleay <= 0)
    //            {
    //                SetState("Move");
    //                _bossPig._currentMoveDleay = _bossPig._moveDelay;
    //                _bossPig._target = _bossPig._wayPoints[_bossPig._numWayPoint];
    //            }
    //            else
    //            {
    //                _bossPig._currentMoveDleay -= Time.deltaTime;
    //            }

    //            yield return null;
    //        }

    //        NextState(_state.ToString());
    //    }

    //    protected IEnumerator HitState()
    //    {
    //        _bossPig.SoundPlay("Hit");
    //        _bossPig.animator.SetTrigger("Hit");
    //        _bossPig.OroraActive(true);
    //        _bossPig.isInvincible = false;
    //        _bossPig._isHide = false;
    //        _bossPig.m_collider.enabled = true;

    //        yield return new WaitForSeconds(1.0f);

    //        SetState("Idle");

    //        yield return null;

    //        NextState(_state.ToString());
    //    }

    //    protected IEnumerator DeadState()
    //    {
    //        _bossPig.ChildAllDead();

    //        _bossPig.OroraActive(true);
    //        _bossPig.animator.SetTrigger("Hit");
    //        BossHPBar.Conceal();
    //        //if (_bossPig._hpBar)
    //        //    _bossPig._hpBar.transform.parent.gameObject.SetActive(false);

    //        Global.shared<SoundManager>().ChangeBGM("Stage-000");

    //        _bossPig.isInvincible = true;
    //        _bossPig._isHide = true;
    //        _bossPig.m_collider.enabled = false;

    //        yield return new WaitForSeconds(0.5f);

    //        _camera.ShakeCamera(1.0f);
    //        _bossPig.animator.SetTrigger("Hide");

    //        yield return new WaitForSeconds(1.0f);

    //        _bossPig.gameObject.SetActive(false);

    //    }
    //}

    //public class Appear : BossPigState
    //{

    //    public Appear(Enemy enemy) : base(enemy)
    //    {
    //    }

    //    public override void StartState()
    //    {
    //        NextState("Appear");
    //    }

    //    IEnumerator AppearState()
    //    {
    //        _bossPig.isInvincible = true;
    //        _bossPig.m_collider.enabled = false;

    //        _bossPig.SetOrora(0);
    //        _bossPig.OroraActive(true);
    //        BossHPBar.Display(_bossPig);
    //        //if (_bossPig._hpBar)
    //        //    _bossPig._hpBar.transform.parent.gameObject.SetActive(false);

    //        Global.shared<SoundManager>().ChangeBGM("BossMode");

    //        yield return null;

    //        _camera = Global.shared<CameraController>();

    //        yield return new WaitForSeconds(1.0f);

    //        _camera.ShakeCamera(1.0f);

    //        _bossPig._isHide = true;

    //        yield return new WaitForSeconds(0.5f);

    //        _bossPig.animator.SetTrigger("Hide");

    //        yield return new WaitForSeconds(0.5f);

    //        EnemyState statePattern = _bossPig._statePatternList[typeof(Pattern0)] as Pattern0;

    //        _bossPig._statePattern = statePattern;
    //        statePattern.StartState();

    //    }
    //}

    //public class Pattern0 : BossPigState
    //{
    //    public Pattern0(Enemy enemy) : base(enemy)
    //    {
    //    }

    //    public override void StartState()
    //    {
    //        NextState("Init");
    //    }

    //    IEnumerator InitState()
    //    {
    //        SetState("Idle");
    //        _bossPig.moveSpeed = _bossPig._baseMoveSpeed;
    //        _bossPig.SetOrora(0);
    //        _bossPig.OroraActive(true);
    //        BossHPBar.Display(_bossPig);
    //        //if (_bossPig._hpBar)
    //        //    _bossPig._hpBar.transform.parent.gameObject.SetActive(true);

    //        Global.shared<SoundManager>().ChangeBGM("BossMode");

    //        yield return null;

    //        _camera = Global.shared<CameraController>();

    //        NextState(_state.ToString());
    //    }

    //    IEnumerator IdleState()
    //    {

    //        if (!_bossPig._isHide)
    //        {
    //            _camera.ShakeCamera(1.0f);
    //            _bossPig.isInvincible = true;
    //            _bossPig._isHide = true;
    //            _bossPig.m_collider.enabled = false;

    //            yield return new WaitForSeconds(0.5f);

    //            _bossPig.OroraActive(false);
    //            _bossPig.animator.SetTrigger("Hide");

    //            yield return new WaitForSeconds(0.5f);
    //        }

    //        yield return null;

    //        yield return IdleStay();

    //    }

    //    IEnumerator MoveState()
    //    {
    //        if (!_bossPig._isHide)
    //        {
    //            _camera.ShakeCamera(1.0f);
    //            _bossPig.isInvincible = true;
    //            _bossPig._isHide = true;
    //            _bossPig.m_collider.enabled = false;

    //            yield return new WaitForSeconds(0.5f);

    //            _bossPig.OroraActive(false);
    //            _bossPig.animator.SetTrigger("Hide");

    //            yield return new WaitForSeconds(0.5f);
    //        }

    //        yield return null;

    //        bool arrive = false;

    //        while (_state == Stat.Move)
    //        {
    //            if (_bossPig._target != null && !arrive)
    //                arrive = _bossPig.GoToTarget(_bossPig._target.position);

    //            if (arrive)
    //            {
    //                _bossPig.SetWayPointNum();

    //                if (_bossPig._childPigNum <= 0)
    //                {
    //                    SetState("Attack");
    //                }
    //                else
    //                {
    //                    SetState("Idle");
    //                }

    //            }

    //            yield return new WaitForFixedUpdate();
    //        }

    //        NextState(_state.ToString());

    //    }

    //    IEnumerator AttackState()
    //    {
    //        _bossPig.StartCoroutine(Attack());

    //        while (_state == Stat.Attack)
    //        {
    //            yield return null;
    //        }

    //        if (_state == Stat.Dead)
    //            _bossPig.StopAllCoroutines();

    //        NextState(_state.ToString());
    //    }

    //    IEnumerator Attack()
    //    {
    //        _camera.ShakeCamera(1.0f);
    //        yield return new WaitForSeconds(1.0f);

    //        _bossPig.isInvincible = false;
    //        _bossPig._isHide = false;
    //        _bossPig.m_collider.enabled = true;

    //        _bossPig.LookTarget(_bossPig._player.transform);

    //        _bossPig.OroraActive(true);
    //        _bossPig.animator.SetTrigger("Attack");

    //        _bossPig.SpawNormalPig();

    //        yield return new WaitForSeconds(3.0f);

    //        if (_state != Stat.Dead)
    //            SetState("Idle");

    //        yield return null;

    //    }

    //    public override void HitFunc()
    //    {
    //        if (_bossPig.currentHP <= _bossPig.maxHP * 0.7f)
    //        {
    //            SetState("Init");

    //            EnemyState statePattern = _bossPig._statePatternList[typeof(Pattern1)] as Pattern1;

    //            _bossPig._statePattern = statePattern;

    //            _bossPig.transform.localScale -= _bossPig._baseSize * 0.15f;

    //            _bossPig.StopAllCoroutines();

    //            statePattern.SetState("Init");

    //            statePattern.NextState(statePattern.CurrentState);
    //        }
    //    }

    //}

    //public class Pattern1 : BossPigState
    //{
    //    int _attackCount = 0;

    //    public Pattern1(Enemy enemy) : base(enemy)
    //    {

    //    }

    //    public override void StateLog()
    //    {
    //    }

    //    IEnumerator InitState()
    //    {
    //        SetState("Idle");
    //        _bossPig.OroraActive(false);
    //        _bossPig.SetOrora(1);
    //        yield return null;

    //        _camera = Global.shared<CameraController>();

    //        NextState(_state.ToString());
    //    }

    //    IEnumerator IdleState()
    //    {
    //        if (!_bossPig._isHide)
    //        {
    //            _camera.ShakeCamera(1.0f);
    //            _bossPig.isInvincible = true;
    //            _bossPig._isHide = true;
    //            _bossPig.m_collider.enabled = false;

    //            yield return new WaitForSeconds(0.5f);

    //            _bossPig.OroraActive(false);
    //            _bossPig.animator.SetTrigger("Hide");

    //            yield return new WaitForSeconds(0.5f);
    //        }

    //        yield return null;

    //        yield return IdleStay();
    //    }

    //    IEnumerator MoveState()
    //    {
    //        if (!_bossPig._isHide)
    //        {
    //            _camera.ShakeCamera(1.0f);
    //            _bossPig.isInvincible = true;
    //            _bossPig._isHide = true;
    //            _bossPig.m_collider.enabled = false;

    //            yield return new WaitForSeconds(0.5f);

    //            _bossPig.OroraActive(false);
    //            _bossPig.animator.SetTrigger("Hide");

    //            yield return new WaitForSeconds(0.5f);
    //        }

    //        yield return null;

    //        float patternDelay = 3;

    //        while (_state == Stat.Move)
    //        {
    //            if (_bossPig.GoToTarget(_bossPig._target.position))
    //            {
    //                _bossPig.SetWayPointNum();

    //                SetState("Idle");
    //            }
    //            else
    //            {
    //                if (patternDelay <= 0)
    //                {
    //                    SetState("Attack");
    //                }
    //                else
    //                    patternDelay -= Time.fixedDeltaTime;
    //            }
    //            yield return new WaitForFixedUpdate();
    //        }

    //        NextState(_state.ToString());
    //    }

    //    IEnumerator AttackState()
    //    {
    //        _bossPig.StartCoroutine(Attack());

    //        while (_state == Stat.Attack)
    //        {
    //            yield return null;
    //        }

    //        if (_state == Stat.Dead)
    //            _bossPig.StopAllCoroutines();

    //        NextState(_state.ToString());
    //    }

    //    IEnumerator Attack()
    //    {

    //        _camera.ShakeCamera(1.0f);
    //        _bossPig.isInvincible = true;
    //        _bossPig._isHide = true;
    //        _bossPig.m_collider.enabled = false;

    //        yield return new WaitForSeconds(0.5f);

    //        _bossPig.animator.SetTrigger("Hide2");

    //        yield return new WaitForSeconds(2.0f);

    //        _camera.ShakeCamera(1.0f);

    //        yield return new WaitForSeconds(0.5f);

    //        RaycastHit2D hit = Physics2D.Raycast(_bossPig._player.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

    //        _bossPig.transform.position = hit.point;

    //        if (_bossPig._WarringImg != null)
    //            _bossPig._WarringImg.SetActive(true);

    //        yield return new WaitForSeconds(0.5f);

    //        if (_bossPig._WarringImg != null)
    //            _bossPig._WarringImg.SetActive(false);

    //        _bossPig.isInvincible = false;
    //        _bossPig._isHide = false;
    //        _bossPig.m_collider.enabled = true;

    //        _bossPig.OroraActive(true);
    //        _bossPig.animator.SetTrigger("Attack");

    //        _attackCount++;

    //        _bossPig._numWayPoint = Random.Range(0, _bossPig._wayPoints.Length);

    //        if (_attackCount >= 4)
    //        {
    //            _attackCount = 0;
    //            _bossPig.RandomSpawnPig();
    //        }

    //        yield return new WaitForSeconds(1.0f);

    //        if(_state != Stat.Dead)
    //            SetState("Idle");

    //        yield return null;

    //    }

    //    public override void HitFunc()
    //    {
    //        if (_bossPig.currentHP <= _bossPig.maxHP * 0.4f)
    //        {
    //            SetState("Init");

    //            EnemyState statePattern = _bossPig._statePatternList[typeof(Pattern2)] as Pattern2;

    //            _bossPig._statePattern = statePattern;

    //            _bossPig.moveSpeed *= 6;
    //            _bossPig.transform.localScale -= _bossPig._baseSize * 0.15f;

    //            _bossPig.StopAllCoroutines();

    //            statePattern.SetState("Init");

    //            statePattern.NextState(statePattern.CurrentState);
    //        }
    //    }

    //}

    //public class Pattern2 : BossPigState
    //{
    //    public Pattern2(Enemy enemy) : base(enemy)
    //    {

    //    }

    //    public override void StateLog()
    //    {
    //    }

    //    IEnumerator InitState()
    //    {
    //        SetState("Idle");
    //        _bossPig.OroraActive(false);
    //        _bossPig.SetOrora(2);
    //        yield return null;

    //        _camera = Global.shared<CameraController>();
    //        NextState(_state.ToString());
    //    }

    //    IEnumerator IdleState()
    //    {
    //        if (_bossPig._isHide)
    //        {
    //            _camera.ShakeCamera(1.0f);

    //            yield return new WaitForSeconds(0.5f);

    //            _bossPig.isInvincible = false;
    //            _bossPig._isHide = false;
    //            _bossPig.m_collider.enabled = true;

    //            _bossPig.GetComponent<BoxCollider2D>().enabled = true;


    //            _bossPig.animator.SetTrigger("Rush");

    //            yield return new WaitForSeconds(0.5f);

    //        }
    //        _bossPig.OroraActive(true);
    //        yield return null;

    //        yield return IdleStay();
    //    }

    //    IEnumerator MoveState()
    //    {
    //        StateLog();
    //        _bossPig.animator.SetTrigger("Rush");

    //        yield return null;

    //        bool arrive = false;

    //        while (_state == Stat.Move)
    //        {
    //            if(!arrive)
    //                arrive = _bossPig.GoToTarget(_bossPig._target.position);

    //            if (arrive)
    //            {
    //                SetState("Idle");
    //                _bossPig.SetWayPointNum();
    //                _bossPig._currentMoveDleay = 1.5f;
    //                _bossPig.LookTarget(_bossPig._player.transform);
    //            }

    //            yield return new WaitForFixedUpdate();
    //        }

    //        NextState(_state.ToString());
    //    }
    //}

    class BossPig_InitState : InitState
    {
        BossPig _boss;

        public BossPig_InitState(BossPig boss) : base(boss)
        {
            _boss = boss;
        }

        protected override IEnumerator Enter()
        {
            _boss._isHide = false;
            _boss.isInvincible = true;
            _boss.m_collider.enabled = false;

            _boss._transform.localScale = _boss._baseSize;

            _boss.SetOrora(_boss._numPattern);
            _boss.OroraActive(true);
            BossHPBar.Display(_boss);

            Global.shared<SoundManager>().ChangeBGM("BossMode");

            yield return new WaitForSeconds(1.0f);
        }

        protected override IEnumerator Execute()
        {
            _boss.SetStatePattern<IdleState>();

            yield return null;
        }
    }

    class BossPig_IdleState : IdleState
    {
        BossPig _boss;

        public BossPig_IdleState(BossPig boss) : base(boss, null)
        {
            _boss = boss;
        }

        protected override IEnumerator Enter()
        {
            if (_boss._numPattern == 0)
            {
                if (!_boss._isHide)
                    yield return _boss.Hide();
            }
            else
            {
                _boss.animator.SetTrigger("Rush");
                _boss.OroraActive(true);
            }

            yield return null;
        }
    }

    class BossPig_MoveState : MoveState
    {
        BossPig _boss;

        public BossPig_MoveState(BossPig boss) : base(boss, null)
        {
            _boss = boss;
        }

        protected override IEnumerator Enter()
        {
            if (_boss._numPattern == 0)
            {
                if (!_boss._isHide)
                    yield return _boss.Hide();
            }
            else
            {
                _boss.animator.SetTrigger("Rush");
                _boss.OroraActive(true);
            }

            _target = _enemy._wayPoints[_enemy._numWayPoint];

            yield return null;
        }

        protected override IEnumerator Execute()
        {
            while (_enemy._statePattern is MoveState)
            {
                if (_enemy.GoToTarget(_target.position))
                {
                    if (_boss._numPattern == 0 && _boss._numChild <= 0)
                    {
                        _boss.SetStatePattern<AttackState>();
                    }
                    else
                    {
                        _enemy.SetStatePattern<IdleState>();
                        _enemy.SetWayPointNum();
                    }
                }

                yield return new WaitForFixedUpdate();
            }

            yield return null;
        }

    }

    class BossPig_Attack0State : AttackState
    {
        BossPig _boss;

        public BossPig_Attack0State(BossPig boss) : base(boss, null)
        {
            _boss = boss;
        }

        protected override IEnumerator Enter()
        {
            _boss._camera.ShakeCamera(1.0f);
            yield return new WaitForSeconds(1.0f);

            _boss._isHide = false;
            _boss.isInvincible = false;
            _boss.m_collider.enabled = true;

            _boss.LookTarget(_boss._player.transform);

            _boss.OroraActive(true);
            //_boss.RandomSpawnPig();
            _boss.SpawNormalPig();
            _boss.animator.SetTrigger("Attack");

            yield return null;
        }

        protected override IEnumerator Execute()
        {
            float delay = 3.0f;

            while (_enemy._statePattern is AttackState)
            {
                if (delay <= 0.0f)
                {
                    _boss.SetStatePattern<IdleState>();
                }
                else
                {
                    delay -= Time.deltaTime;
                }

                yield return null;
            }

            yield return null;
        }

    }

    class BossPig_Attack1State : AttackState
    {
        BossPig _boss;

        RaycastHit2D hit;

        public BossPig_Attack1State(BossPig boss) : base(boss, null)
        {
            _boss = boss;
        }

        protected override IEnumerator Enter()
        {
            _boss._camera.ShakeCamera(1.0f);

            yield return new WaitForSeconds(0.5f);

            _boss._isHide = true;
            _boss.isInvincible = true;
            _boss.m_collider.enabled = false;
            _boss.animator.SetTrigger("Hide2");

            yield return new WaitForSeconds(2.0f);
        }

        protected override IEnumerator Execute()
        {
            _boss._camera.ShakeCamera(1.0f);

            yield return new WaitForSeconds(0.5f);

            hit = Physics2D.Raycast(_boss._player.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
            _boss._transform.position = hit.point;

            if (_boss._warringImg != null)
                _boss._warringImg.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            if (_boss._warringImg != null)
                _boss._warringImg.SetActive(false);

            _boss._isHide = false;
            _boss.isInvincible = false;
            _boss.m_collider.enabled = true;

            _boss.OroraActive(true);
            _boss.animator.SetTrigger("Attack");

            _boss._numWayPoint = Random.Range(0, _boss._wayPoints.Length);

            yield return new WaitForSeconds(1.0f);

            _boss.SetStatePattern<IdleState>();

            yield return null;
        }

    }

    class BossPig_HitState : HitState
    {
        BossPig _boss;
                
        public BossPig_HitState(BossPig boss, float delay) : base(boss, 0, delay, null)
        {
            _boss = boss;
        }

        protected override IEnumerator Enter()
        {
            _boss._isHide = false;
            _boss.SoundPlay("Hit");
            _boss.OroraActive(true);
            _boss.isInvincible = false;
            _boss.m_collider.enabled = true;
            _boss.animator.SetTrigger("Hit");

            yield return null;
        }

        protected override IEnumerator Execute()
        {
            float delay = 0;
            while (_enemy._statePattern is HitState)
            {
                delay += Time.deltaTime;

                if (delay >= _delay)
                {
                    _enemy.SetStatePattern<IdleState>();
                }

                yield return null;
            }
        }

    }

    class BossPig_DeadState : NewDeadState
    {
        BossPig _boss;

        public BossPig_DeadState(BossPig boss) : base(boss)
        {
            _boss = boss;
        }

        protected override IEnumerator Enter()
        {
            _boss.ChildAllDead();

            BossHPBar.Conceal();
            _boss.OroraActive(true);

            _boss._isHide = true;
            _boss._numPattern = 0;
            _boss.isInvincible = true;
            _boss.m_collider.enabled = false;
            _boss.moveSpeed = _boss._baseMoveSpeed;

            Global.shared<SoundManager>().ChangeBGM("Stage-000");

            _boss._camera.ShakeCamera(1.0f);

            yield return new WaitForSeconds(1.0f);
        }
    }

}
