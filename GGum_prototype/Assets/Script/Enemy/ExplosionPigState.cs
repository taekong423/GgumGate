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

        public override void StartState()
        {
            NextState("Init");
        }

        protected override IEnumerator InitState()
        {

            yield return base.InitState();

            //_pig.StartCoroutine(Explosion());
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

                SetState("Init");

                _pig.gameObject.SetActive(false);
            }
            else
            {
                GameObject obj = Instantiate(_pig._explosionPrefab, _pig.transform.position, Quaternion.identity) as GameObject;
                obj.GetComponent<Explosion>().pHitData = _pig.pHitData;
                SetState("Init");
                _pig.gameObject.SetActive(false);
            }

            
        }

        

    }
	
}
