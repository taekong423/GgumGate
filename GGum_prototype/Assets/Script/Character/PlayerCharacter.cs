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
    private CircleCollider2D cc;
    private GameObject ladder;

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


        if (canClimb && vAxis != 0)
        {
            m_rigidbody.isKinematic = true;
            //m_rigidbody.gravityScale = 0;
            m_rigidbody.velocity = Vector2.zero;
            gameObject.transform.position = new Vector3(ladder.transform.position.x, gameObject.transform.position.y, 0);
            Move(Axis.Vertical, vAxis);
        }
        else
        {
            Move(Axis.Horizontal, hAxis);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(rayPosCenter, rayPosCenter - new Vector3(0, y + rayLength, 0));
        Gizmos.DrawLine(rayPosRight, rayPosRight - new Vector3(0, rayLength, 0));
        Gizmos.DrawLine(rayPosLeft, rayPosLeft - new Vector3(0, rayLength, 0));
    }

    protected override void InitCharacter()
    {
        base.InitCharacter();

        canClimb = false;
        canShoot = true;
        onGround = false;
        m_collider = GetComponent<BoxCollider2D>();
        cc = GetComponent<CircleCollider2D>();
        layerMask = LayerMask.GetMask("Ground");
        rayLength = 2.5f;
        rayPosX = 2.1f;
        rayPosY = 10.0f;
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
        rayPosCenter = transform.position + new Vector3(0, -rayPosY, 0);
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
        RaycastHit2D hit = Physics2D.Raycast(originPos, Vector2.down, rayLength, layerMask);
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

    private bool CheckColliderByLayer(string layerName, Collider2D other, out GameObject otherObject)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerName))
        {
            otherObject = other.gameObject;
            return true;
        }
        else
        {
            otherObject = null;
            return false;
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (m_collider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            
        }
        if (CheckColliderByLayer("Ladder", other, out ladder))
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
                Debug.Log("OffLadder");
                canClimb = false;
                m_rigidbody.isKinematic = false;
                m_rigidbody.gravityScale = 50;
                jumpCounter = 0;
            }
        }
    }
}