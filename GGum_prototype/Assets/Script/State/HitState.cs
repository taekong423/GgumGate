using UnityEngine;
using System.Collections;

public class HitState : EnemyState {

    protected float _keepTime;
    protected float _delay;

    public HitState(Enemy enemy, float keepTime, float delay, Searchable searchable) : base(enemy, searchable)
    {
        CurrentState = "Hit";
        _keepTime = keepTime;
        _delay = delay;
    }

    protected override IEnumerator Enter()
    {
        _enemy.SoundPlay("Hit");
        _enemy.animator.SetTrigger("Hit");
        _enemy._isHitEffectDelay = true;
        _enemy.StartCoroutine(Keep());

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

    protected override IEnumerator Exit()
    {
        _enemy._statePattern.StartState();

        yield return null;
    }

    IEnumerator Keep()
    {
        yield return new WaitForSeconds(_keepTime);

        _enemy._isHitEffectDelay = false;
    }
}


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