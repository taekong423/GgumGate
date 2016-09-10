using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

    private float invincibleTime;
    private bool canShoot = true;

	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (GameController.This.ButtonDown(EButtonCode.Jump))
            Jump();


        if (GameController.This.ButtonPress(EButtonCode.Attack) && canShoot)
        {
            StartCoroutine(Shoot());
        }
	}

    void FixedUpdate()
    {
        float x = GameController.This.ButtonAxis(EButtonCode.MoveX);

        Flip(x);

        Move(Axis.Horizontal, x);
    }

    private float GetAttackSpeed(float attackSpeed)
    {
        if (attackSpeed <= 0)
            return 1.0f;
        else
            return 1.0f / attackSpeed;
    }

    protected override void Attack(HitInfo hitInfo)
    {
        CreateBullet(hitInfo);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        Attack(new HitInfo(gameObject, AttackDamage));
        yield return new WaitForSeconds(GetAttackSpeed(AttackSpeed));
        canShoot = true;
    }
}
