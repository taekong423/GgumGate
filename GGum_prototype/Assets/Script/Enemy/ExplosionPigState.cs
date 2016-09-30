using UnityEngine;
using System.Collections;

public partial class ExplosionPig
{

    public class ExplosionState : NormalState
    {
        ExplosionPig _pig;

        bool _isExplosion = false;

        public ExplosionState(Enemy enemy) : base(enemy)
        {
            _pig = enemy as ExplosionPig;
        }

        protected override IEnumerator InitState()
        {
            yield return base.InitState();

            _pig.StartCoroutine(Explosion());
        }

        protected override IEnumerator DeadState()
        {
            _pig.isInvincible = true;
            _pig.GetComponent<BoxCollider2D>().enabled = false;

            if (!_isExplosion)
            {
                _pig.Dead();

                yield return null;

                _pig.animator.SetTrigger("Hit");

                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                GameObject obj = Instantiate(_pig._explosionPrefab, _pig.transform.position, Quaternion.identity) as GameObject;
                obj.GetComponent<Explosion>().pHitData = _pig.pHitData;
            }

            yield return new WaitForSeconds(1.0f);

            SetState("Init");

            _pig.gameObject.SetActive(false);
        }

        IEnumerator Explosion()
        {
            float delay = 4;

            while (_state != Stat.Dead)
            {
                _pig._countText.text = string.Format("{0:F0}", delay);

                if (delay <= 0)
                {
                    SetState("Dead");
                    _isExplosion = true;
                }
                else
                    delay -= Time.deltaTime;

                yield return null;
            }
        }

    }
	
}
