using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public partial class ExplosionPig : NormalPig {

    bool _isExplosion = false;

    public Text _countText;
    public GameObject _explosionPrefab;

    void OnEnable()
    {
        if (_statePattern != null)
        {
            _statePattern.StartState();
        }

        if (_isSet)
            Sprinkle();

        StartCoroutine(Explosion());

    }

    public override void Dead()
    {
        if (_isExplosion)
        {
            _boss.NumChild--;
            GameObject obj = Instantiate(_explosionPrefab, transform.position, Quaternion.identity) as GameObject;
            //obj.GetComponent<Explosion>().pHitData = pHitData;
            SetStatePattern<InitState>();
            gameObject.SetActive(false);
        }
        else
        {
            base.Dead();
        }
    }

    //protected override void InitCharacter()
    //{
    //    base.InitCharacter();



    //    //_statePatternList.Add(typeof(ExplosionState), new ExplosionState(this));

    //    //SetStatePattern<ExplosionState>();
    //}

    IEnumerator Explosion()
    {
        float delay = 4;

        while (!(_statePattern is DeadState))
        {
            _countText.text = string.Format("{0:F0}", delay);

            if (delay <= 0)
            {
                _isExplosion = true;
                SetStatePattern<DeadState>();
            }
            else
                delay -= Time.deltaTime;

            yield return null;
        }
    }

}
