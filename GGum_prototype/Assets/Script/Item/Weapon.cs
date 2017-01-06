using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

    #region 변수

    [SerializeField]
    protected float _damage = 1;
    [SerializeField]
    protected float _delay = 1.0f;

    #endregion

    #region 프로퍼티

    public float Delay { get { return _delay; } }

    #endregion

    public abstract void Attack();

}
