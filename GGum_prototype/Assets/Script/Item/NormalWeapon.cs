using UnityEngine;
using System.Collections;
using System;

public class NormalWeapon : Weapon {

    #region 변수

    [SerializeField]
    GameObject _bullet;
    [SerializeField]
    Transform _muzzle;

    #endregion

    #region 메소드

    void CreateBullet()
    {
        if (_bullet == null)
            return;

        GameObject bullet = (GameObject)Instantiate(_bullet, _muzzle.position, _muzzle.rotation);

        bullet.GetComponent<Bullet>().pHitData = new New.HitData(gameObject, _damage);

    }

    public override void Attack()
    {
        CreateBullet();
    }

    #endregion

}
