using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{

    public int maxJumps;

    private int jumpCounter;
    private float invincibleTime;
    private bool canShoot = true;
    private float hAxis;
    private float vAxis;
    private LayerMask layerMask;
    private float rayLength;
    private Vector3 rayPosCenter;
    private Vector3 rayPosRight;
    private Vector3 rayPosLeft;

    float rayPosX;
    float rayPosY;

    // Use this for initialization
    void Start()
    {
        CurrentHP = MaxHP;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
        layerMask = LayerMask.GetMask("Ground");
        rayLength = 1.0f;
        rayPosX = 2.1f;
        rayPosY = 11.5f;
    }

    // Update is called once per frame
    void Update()
    {
        onGround = GroundCheck();
        KeyInput();
    }

    void FixedUpdate()
    {
        hAxis = GameController.This.ButtonAxis(EButtonCode.MoveX);
        vAxis = GameController.This.ButtonAxis(EButtonCode.MoveY);

        Flip(hAxis);
        Move(Axis.Horizontal, hAxis);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(rayPosCenter, rayPosCenter - new Vector3(0, y + rayLength, 0));
        Gizmos.DrawLine(rayPosRight, rayPosRight - new Vector3(0, rayPosY + rayLength, 0));
        Gizmos.DrawLine(rayPosLeft, rayPosLeft - new Vector3(0, rayPosY + rayLength, 0));
    }

    private void KeyInput()
    {
        if (GameController.This.ButtonDown(EButtonCode.Jump))
        {
            if (onGround)
            {
                jumpCounter = 0;
                m_rigidbody.gravityScale = 20;
            }

            if (jumpCounter < maxJumps)
            {
                Jump();
                m_rigidbody.gravityScale = 50;
                jumpCounter++;
            }
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
        rayPosCenter = transform.position;
        rayPosRight = rayPosCenter + new Vector3(rayPosX, 0, 0);
        rayPosLeft = rayPosCenter + new Vector3(-rayPosX, 0, 0);
        if (IsGround(rayPosLeft) || IsGround(rayPosRight))
        {
            m_rigidbody.gravityScale = 10;
            return true;
        }
        else
        {
            m_rigidbody.gravityScale = 50;
            return false;
        }
    }

    private bool IsGround(Vector3 originPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(originPos, Vector2.down, rayPosY + rayLength, layerMask);
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
        Attack(new HitInfo(gameObject, AttackDamage));

        yield return new WaitForSeconds(GetAttackSpeed(AttackSpeed));
        canShoot = true;
    }
}