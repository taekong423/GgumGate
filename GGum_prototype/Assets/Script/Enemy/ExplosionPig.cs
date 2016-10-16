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
        StartCoroutine(Explosion());

    }

    public override void Dead()
    {
        if (!_isExplosion)
            return;
        GameObject obj = Instantiate(_explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        obj.GetComponent<Explosion>().pHitData = pHitData;
        SetStatePattern<InitState>();
        gameObject.SetActive(false);
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
                SetStatePattern<DeadState>();
                _isExplosion = true;
            }
            else
                delay -= Time.deltaTime;

            yield return null;
        }
    }

}
