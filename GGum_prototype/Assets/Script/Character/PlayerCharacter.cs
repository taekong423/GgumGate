using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{

    public int maxJumps;

    private int jumpCounter;
    private float invincibleTime;
    private bool canShoot;
    private bool canClimb;
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
        InitCharacter();
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

        if (canClimb && vAxis != 0)
        {
            m_rigidbody.isKinematic = true;
            m_rigidbody.gravityScale = 0;
            m_rigidbody.velocity = Vector2.zero;
            Move(Axis.Vertical, vAxis);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(rayPosCenter, rayPosCenter - new Vector3(0, y + rayLength, 0));
        Gizmos.DrawLine(rayPosRight, rayPosRight - new Vector3(0, rayPosY + rayLength, 0));
        Gizmos.DrawLine(rayPosLeft, rayPosLeft - new Vector3(0, rayPosY + rayLength, 0));
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();

        canClimb = false;
        canShoot = true;
        onGround = false;
        m_collider = GetComponent<BoxCollider2D>();
        layerMask = LayerMask.GetMask("Ground");
        rayLength = 1.0f;
        rayPosX = 2.1f;
        rayPosY = 11.5f;
    }

    private int GetRawAxis(float value)
    {
        if (value > 0)
        {
            return 1;
        }
        else if (value < 0)
            return -1;
        else
            return 0;
    }

    private void KeyInput()
    {
        if (GameController.This.ButtonDown(EButtonCode.Jump))
        {
            if (onGround)
            {
                jumpCounter = 0;
                m_rigidbody.gravityScale = 10;
            }

            if (canClimb)
            {
                jumpCounter = 0;
                canClimb = false;
                m_rigidbody.isKinematic = false;
            }

            if (jumpCounter < maxJumps)
            {
                m_rigidbody.velocity = Vector2.zero;
                Jump();
                m_rigidbody.gravityScale = 50;
                jumpCounter++;
            }
        }

        if (GameController.This.ButtonPress(EButtonCode.Attack) && canShoot && !canClimb)
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
            if (!canClimb)
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

    protected override void Attack(HitData hitInfo)
    {
        CreateBullet(hitInfo);
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        Attack(new HitData(gameObject, AttackDamage));

        yield return new WaitForSeconds(GetAttackSpeed(AttackSpeed));
        canShoot = true;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (m_collider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            if (!canClimb)
            {
                Debug.Log("OnLadder");
                canClimb = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            if (canClimb)
            {
                Debug.Log("OnLadder");
                canClimb = false;
                m_rigidbody.isKinematic = false;
                m_rigidbody.gravityScale = 50;
                jumpCounter = 0;
            }
        }
    }
}