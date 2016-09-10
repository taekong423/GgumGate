using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

    private float invincibleTime;
    private bool canShoot = true;
    private float hAxis;
    private float vAxis;
    private LayerMask layerMask;
    private float distToGround;

	// Use this for initialization
	void Start () {
        StartCoroutine(InitState());
	}
	
	// Update is called once per frame
	void Update () {
        onGround = GroundCheck();
        KeyInput();
	}

    void FixedUpdate()
    {
        
    }

    private void KeyInput()
    {
        hAxis = GameController.This.ButtonAxis(EButtonCode.MoveX);
        vAxis = GameController.This.ButtonAxis(EButtonCode.MoveY);

        //Move(Axis.Horizontal, hAxis);

        if (GameController.This.ButtonDown(EButtonCode.Jump) && onGround)
        {
            onGround = false;
            Jump();
        }

        if (GameController.This.ButtonPress(EButtonCode.Attack) && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    private void GetArrowKey()
    {
        hAxis = GameController.This.ButtonAxis(EButtonCode.MoveX);
        vAxis = GameController.This.ButtonAxis(EButtonCode.MoveY);
    }

    private bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.1f, layerMask);
        return (hit.collider != null);
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
        if (onGround)
            
        Attack(new HitInfo(gameObject, AttackDamage));
        yield return new WaitForSeconds(GetAttackSpeed(AttackSpeed));
        canShoot = true;
    }

    IEnumerator InitState()
    {
        distToGround = m_collider.bounds.extents.y;
        layerMask = LayerMask.NameToLayer("Ground");
        m_rigidbody = GetComponent<Rigidbody2D>();
        yield return null;

        state = State.Idle;
        yield return null;

        NextState();
    }

    IEnumerator IdleState()
    {
        
        yield return null;

        while (state == State.Idle)
        {
            GetArrowKey();
            if (hAxis != 0)
            {
                state = State.Move;
            }

            yield return null;
        }

        NextState();
    }

    IEnumerator MoveState()
    {
        Flip(hAxis);
        //mesh.animation.CrossFade("death");
        yield return null;

        while (state == State.Move)
        {
            GetArrowKey();
            if (hAxis == 0)
            {
                state = State.Idle;
            }
            else
            {
                Move(Axis.Horizontal, hAxis);
            }
            yield return null;
        }

        NextState();
    }
}
