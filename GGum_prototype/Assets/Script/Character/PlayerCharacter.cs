using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{
    enum PlayerState
    {
        Any,
        Jump,
        Shoot,
        Ladder,
        Swim,
    }

    public int maxJumps;

    private State lastState;
    private PlayerState playerState;
    private int jumpCounter;
    private bool canShoot;
    private bool canClimb;
    private float hAxis;
    private float vAxis;
    private LayerMask layerMask;
    private float rayLength;
    private Vector3 rayPosCenter;
    private Vector3 rayPosRight;
    private Vector3 rayPosLeft;
    private GameObject ladder;

    private bool currGroundCheck;
    private bool lastGroundCheck;

    float rayPosX;
    float rayPosY;

    CameraController cameraController;

    // Use this for initialization
    void Start()
    {
        //InitCharacter();
        StartCoroutine(InitState());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            cameraController.ShakeCamera(true);
        }

        if (Input.GetKeyDown("2"))
        {
            cameraController.ShakeCamera(false);
        }

        if (Input.GetKeyDown("3"))
        {
            cameraController.ShakeCamera(2.0f);
        }

        KeyInput();
    }

    void FixedUpdate()
    {
        
    }

    protected override IEnumerator InitState()
    {
        InitCharacter();
        yield return null;

        state = State.Idle;
        yield return null;

        NextState();
    }

    protected override IEnumerator IdleState()
    {
        animator.SetTrigger("Idle");
        yield return null;

        while (state == State.Idle)
        {
            AxisInput();
            yield return new WaitForFixedUpdate();   
        }

        NextState();
    }

    protected override IEnumerator MoveState()
    {
        animator.SetTrigger("Run");
        yield return null;

        while (state == State.Move)
        {
            AxisInput();
            yield return new WaitForFixedUpdate();
        }

        NextState();
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
        currGroundCheck = false;
        lastGroundCheck = false;
        m_collider = GetComponent<CircleCollider2D>();
        layerMask = LayerMask.GetMask("Ground");
        rayLength = 2.5f;
        rayPosX = 2.1f;
        rayPosY = 10.0f;
        invincibleTime = 1.0f;

        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void KeyInput()
    {
        currGroundCheck = GroundCheck();
        
        if (currGroundCheck != lastGroundCheck)
        {
            if (currGroundCheck)
            {
                if (playerState != PlayerState.Any)
                {
                    SetAnimationBack();
                    playerState = PlayerState.Any;
                }
                m_rigidbody.gravityScale = 10;
            }
            else
            {
                playerState = PlayerState.Jump;

                if (!canClimb)
                    m_rigidbody.gravityScale = 50;
            }
        }

        lastGroundCheck = currGroundCheck;

        // Input Jump Key
        if (GameController.This.ButtonDown(EButtonCode.Jump))
        {
            if (currGroundCheck)
            {
                jumpCounter = 0;
                m_rigidbody.gravityScale = 10;
            }

            if (canClimb)
            {
                jumpCounter = 0;
                canClimb = false;
            }

            if (jumpCounter < maxJumps)
            {
                playerState = PlayerState.Jump;
                animator.SetTrigger("Jump");
                m_rigidbody.velocity = Vector2.zero;
                Jump();
                m_rigidbody.gravityScale = 50;
                jumpCounter++;
            }
        }

        // Input Attack Key
        if (GameController.This.ButtonPress(EButtonCode.Attack) && canShoot && !canClimb)
        {
            StartCoroutine(Shoot());
        }
    }

    private void AxisInput()
    {
        hAxis = GameController.This.ButtonAxis(EButtonCode.MoveX);
        vAxis = GameController.This.ButtonAxis(EButtonCode.MoveY);

        Flip(hAxis);

        if (canClimb)
        {
            if (vAxis != 0)
            {
                InitLadder();
                Move(Axis.Vertical, vAxis);
            }
            else if (vAxis == 0 && hAxis != 0)
            {
                state = State.Move;
                animator.SetTrigger("Run");
                Move(Axis.Horizontal, hAxis);
            }
        }
        else
        {
            if (hAxis == 0)
            {
                state = State.Idle;
            }
            else
            {
                state = State.Move;
                Move(Axis.Horizontal, hAxis);
            }
        }
    }

    private void InitLadder()
    {
        playerState = PlayerState.Ladder;
        animator.SetTrigger("Ladder");
        m_rigidbody.gravityScale = 0;
        m_rigidbody.velocity = Vector2.zero;
        gameObject.transform.position = new Vector3(ladder.transform.position.x, gameObject.transform.position.y, 0);
        ladder.GetComponent<LadderCollisionTrigger>().IgnorePlatform(true);
    }

    private bool GroundCheck()
    {
        rayPosCenter = transform.position + new Vector3(0, -rayPosY, 0);
        rayPosRight = rayPosCenter + new Vector3(rayPosX, 0, 0);
        rayPosLeft = rayPosCenter + new Vector3(-rayPosX, 0, 0);

        if (IsGround(rayPosLeft, rayLength) || IsGround(rayPosRight, rayLength))
        {
            return true;
        }
        else
        {
            return false;
        }
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

        if (playerState == PlayerState.Jump)
        {
            animator.SetBool("JumpAttack", true);
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetBool("JumpAttack", false);
            animator.SetBool("CriticalAttack", false);
            animator.SetTrigger("Attack");
        }

        Attack(new HitData(gameObject, AttackDamage));

        yield return new WaitForSeconds(GetAttackSpeed(AttackSpeed));

        SetAnimationBack();

        canShoot = true;
    }

    private void SetAnimationBack()
    {
        if (state == State.Idle)
            animator.SetTrigger("Idle");
        else if (state == State.Move)
            animator.SetTrigger("Run");
    }

    private bool CheckColliderByLayer(string layerName, Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerName))
            return true;
        else
            return false;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (CheckColliderByLayer("Ladder", other))
        {
            if (!canClimb)
            {
                Debug.Log("OnLadder");
                canClimb = true;
                ladder = other.gameObject;
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
                m_rigidbody.gravityScale = 50;
                jumpCounter = 0;
            }
        }
    }
}