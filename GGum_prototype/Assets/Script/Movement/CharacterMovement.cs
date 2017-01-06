using UnityEngine;
using System.Collections;

public class CharacterMovement : Movement {

    #region 변수

    [Header("Walk and Run")]
    [SerializeField]
    Space _walkSpace; // 걷거나 달릴때 방향의 기준이 월드 좌표 기준인지 로컬 좌표 기준인지 결정하는 변수
    [SerializeField]
    float _walkSpeed = 10.0f; // 걸을 때 속도
    [SerializeField]
    float _runSpeed = 10.0f; // 달릴때 속도

    bool _isWalking = false; // 걷고 있으면 true, 아니면 false
    bool _isRunning = false; // 달리고 있으면 true, 아니면 false

    [Header("Jump")]
    [SerializeField]
    Space _jumpSpace; // 점프 방향의 기준이 월드인지 로컬인지 기준이 되는 변수
    [SerializeField]
    float _jumpForce = 100.0f; // 점프 파워
    [SerializeField]
    int _maxJumpNum = 1; // 점프 가능한 횟수

    bool _canJump = true;
    bool _isJumping = false;

    int _jumpCount = 0;

    [Header("GroundCheck")]
    [SerializeField]
    LayerMask _groundCheckLayer; // 체크할 레이어 설정
    [SerializeField]
    LayerMask _platformCheckLayer;
    [SerializeField]
    float _checkRadius = 1.0f; // 체크 범위
    [SerializeField]
    Vector3 _checkOffset; // 체크 중점 설정

    Collider2D _groundHit; // 충돌된 땅 콜라이더를 저장
    Collider2D _platformHit;

    bool _canCheckPlatform;
    bool _isFalling = false; // 공중에 있으면 true, 아니면 false


    [Header("Fly")]
    [SerializeField]
    float _flyGravity = 1.0f;

    float _originalGravity; // 시작 중력값 저장

    bool _isFlying = false; // 나는 중이면 true, 아니면 false

    [Header("Climb")]
    [SerializeField]
    LayerMask _climbLayer; // climb가능한 오브젝트 체크 레이어
    [SerializeField]
    Vector3 _climbCheckPoint; // 체크 중심점
    [SerializeField]
    Vector2 _climbCheckBoxSize; // 체크 범위(박스 형태)

    [SerializeField]
    float _snapSpeed = 5.0f; // 중앙에 고정하는 속도

    // climbing 이동속들
    [SerializeField]
    float _climbingUpSpeed = 3.0f;
    [SerializeField]
    float _climbingDownSpeed = 3.0f;
    [SerializeField]
    float _climbingLeftSpeed = 3.0f;
    [SerializeField]
    float _climbingRightSpeed = 3.0f;

    BoxCollider2D _climbObjectHit;
    ClimbObject _climbObject;

    bool _isClimbing = false;


    #endregion

    #region 프로퍼티

    public Vector2 GetVelocity { get { return _rigidbody.velocity; } }

    public bool IsWalking { get { return _isWalking; } }
    public bool IsRunning { get { return _isRunning; } }

    public bool CanCheckPlatform { get { return _canCheckPlatform; } set { _canCheckPlatform = value; } }
    public bool IsFalling { get { return _isFalling; } }
    public bool IsGround { get { return !_isFalling; } }

    public bool CanJump { get { return _canJump; } set { _canJump = value; } }
    public bool IsJumping { get { return _isJumping; } }

    public bool IsFlying { get { return _isFlying; } }

    public bool IsClimbing { get { return _isClimbing; } }
    public bool CanClimbing { get { return (_climbObjectHit != null) ? true : false; } }

    #endregion

    #region 메소드

    protected override void Init()
    {
        base.Init();
        _originalGravity = _rigidbody.gravityScale;
    }

    void FixedUpdate()
    {
        #region GroundCheck

        // 범위에 들어온 땅의 콜라이더를 저장함
        _groundHit = Physics2D.OverlapCircle(_transfom.position + _checkOffset, _checkRadius, _groundCheckLayer);

        _platformHit = Physics2D.OverlapCircle(_transfom.position + _checkOffset, _checkRadius, _platformCheckLayer);

        // 범위에 들어온 콜라이더가 있는지 검사 있으면 점프 초기화, isFalling = false, 아니면 isFalling = true
        if (_groundHit != null || (_platformHit != null && _canCheckPlatform))
        {
            OffJump();
            _isFalling = false;
        }
        else
        {
            _isFalling = true;
        }

        #endregion

        #region ClimbCheck

        _climbObjectHit = (BoxCollider2D)Physics2D.OverlapBox(_transfom.position + _climbCheckPoint, _climbCheckBoxSize, 0, _climbLayer);

        

        #endregion
    }
    #region Move

    public void Move(Vector2 direction, float scaleValue, bool isRun = false)
    {
        // 입력 값이 0이면 리턴
        if (scaleValue == 0.0f)
        {
            _isWalking = false;
            _isRunning = false;
            return;
        }

        // 이동을 자신 기준으로 할지 검사, 항상 right 방향으로 이동함(로컬 좌표 기준)
        if (_walkSpace == Space.Self)
        {
            // 입력 값이 음수면 180도 회전한다
            _transfom.localEulerAngles = new Vector3(0, (scaleValue > 0) ? 0 : 180, 0);
            scaleValue = 1.0f;
        }

        // 뛰는지 검사, 아니면 걷는 속도, 맞으면 뛰는 속도로 이동
        if (!isRun)
        {
            _isWalking = true;
            _isRunning = false;
            BaseMove(direction, scaleValue, _walkSpeed, _walkSpace);
        }
        else
        {
            _isWalking = false;
            _isRunning = true;
            BaseMove(direction, scaleValue, _runSpeed, _walkSpace);
        }
    }

    // 물리 이동
    public void PhysicsXMove(float scaleValue, bool isRun = false)
    {

        if (_walkSpace == Space.Self && scaleValue != 0)
        {
            _transfom.localEulerAngles = new Vector3(0, (scaleValue > 0) ? 0 : 180, 0);
        }

        if (!isRun)
        {
            BasePhysicsXMove(scaleValue, _walkSpeed);
        }
        else
        {
            BasePhysicsXMove(scaleValue, _runSpeed);
        }
    }

    #endregion

    #region Jump

    // 점프 초기화
    void OffJump()
    {
        if (!_isFalling || !_isJumping)
            return;

        Debug.Log("OffJump");
        _isJumping = false;
        _jumpCount = 0;
    }

    public void Jump(float force)
    {
        // 점프 중인데 점프 횟수가 최대 횟수일때 리턴
        if (_isJumping && _jumpCount >= _maxJumpNum || !_canJump)
            return;

        // Climb중이면 중단하고 점프함
        if (_isClimbing)
        {
            OffClimb();
        }

        _isJumping = true;

        _jumpCount++;

        // 점프 기준이 월드인지 검사
        if (_jumpSpace == Space.World)
        {
            _rigidbody.AddForce(Vector2.up * force);
        }
        else
        {
            _rigidbody.AddForce(transform.up * force);
        }

    }

    public void Jump()
    {
        // 점프 중인데 점프 횟수가 최대 횟수일때 리턴
        if (_isJumping && _jumpCount >= _maxJumpNum || !_canJump)
            return;

        // Climb중이면 중단하고 점프함
        if (_isClimbing)
        {
            OffClimb();
        }

        _isJumping = true;

        _jumpCount++;

        // 점프 기준이 월드인지 검사
        if (_jumpSpace == Space.World)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce);
        }
        else
        {
            _rigidbody.AddForce(transform.up * _jumpForce);
        }

    }

    #endregion

    #region Fly

    // 비행 시작 함수
    public void OnFly()
    {
        // 점프 중이 아니고 공중이 아닐 경우 리턴
        if (!_isJumping && !_isFalling)
            return;

        // 속도의 y값이 음수인지(아래로 떨어지는 중인지 체크), 현재 비행 상태인지 체크
        if (_rigidbody.velocity.y < -1f && !_isFlying)
        {
            _isFlying = true;
            _rigidbody.gravityScale = _flyGravity;
        }
    }

    // 비행 종료 함수
    public void OffFly()
    {
        if (!_isFlying)
            return;
        _isFlying = false;
        _rigidbody.gravityScale = _originalGravity;
    }

    #endregion

    #region Climb

    // Climb 시작 셋팅 함수
    public void OnClimb()
    {
        _isClimbing = true;
        _climbObject = _climbObjectHit.GetComponent<ClimbObject>();
        _rigidbody.gravityScale = 0;
        _jumpCount = 0;
    }

    // Climb 종료, Climb 시작전 상태로 초기화
    public void OffClimb()
    {
        _isClimbing = false;
        _climbObject = null;
        _rigidbody.gravityScale = _originalGravity;
    }

    // Climg 중일때 x축으로 이동하는 함수
    public void ClimbX(float scaleValue)
    {
        // Climb 중이 아니거나 ClimbObject를 벗어 났을 경우 리턴
        if (!_isClimbing || _climbObjectHit == null)
            return;

        // climb 중일때 x축 입력 값이 0 일 경우 x physic를 0으로 셋팅
        if (scaleValue == 0)
        {
            BasePhysicsXMove(0, 0);
            return;
        }

        // Climb중이면서 땅에 있는지 검사
        if (!IsGround)
        {
            // 입력 값이 왼쪽 방향인지, ClimbObject가 왼쪽이동을 허용하고 있는지 검사
            if (scaleValue < 0 && _climbObject.AllowLeft)
            {
                // 캐릭터의 거리가 x축으로 얼만큼 왼쪽편으로 떨어져 있는지 계산(왼쪽에 가까울수록 결과 값은 작음)
                float xOffset = Mathf.Abs(_transfom.position.x - (_climbObjectHit.transform.position.x - (_climbObjectHit.size.x / 2)));

                // ClimbObject가 왼쪽으로 이동하여 벗어나도록 허용하는지 검사하고 허용하지 않으면 계산 결과값이 1보다 작은지 검사
                if (!_climbObject.FallOnLeft && xOffset < 1f)
                {
                    // x축으로 더 이상 이동하지 않도록 물리 값을 0으로 설정
                    BasePhysicsXMove(0, 0);
                }
                else
                {
                    // x 축 왼쪽으로 이동
                    BasePhysicsXMove(scaleValue, _climbingLeftSpeed);
                }

            }
            else if (scaleValue > 0 && _climbObject.AllowRight)
            {
                // 캐릭터의 거리가 x축으로 얼만큼 오른쪽편으로 떨어져 있는지 계산(오른쪽에 가까울수록 결과 값은 작음)
                float xOffset = Mathf.Abs(_transfom.position.x - (_climbObjectHit.transform.position.x + (_climbObjectHit.size.x / 2)));

                // ClimbObject가 오른쪽으로 이동하여 벗어나도록 허용하는지 검사하고 허용하지 않으면 계산 결과값이 1보다 작은지 검사
                if (!_climbObject.FallOnRight && xOffset < 1f && !IsGround)
                {
                    // x축으로 더 이상 이동하지 않도록 물리 값을 0으로 설정
                    BasePhysicsXMove(0, 0);
                }
                else
                {
                    // x 축 오른쪽으로 이동
                    BasePhysicsXMove(scaleValue, _climbingRightSpeed);
                }

            }

        }
        else
        {
            // Climb 중이면서 땅 위에 있을때 x 축 입력이 들어오면 Climb을 종료함
            OffClimb();
        }

    }

    // Climb 중일때 y축으로 이동하는 함수
    public void ClimbY(float scaleValue)
    {
        // Climb 중이 아니거나 ClimbObject를 벗어 났을 경우 리턴
        if (!_isClimbing || _climbObjectHit == null)
            return;

        // climb 중일때 y축 입력 값이 0 일 경우 y physic를 0으로 셋팅
        if (scaleValue == 0)
        {
            BasePhysicsYMove(0, 0);
            return;
        }

        // 입력 값이 아래쪽 방향인지, ClimbObject가 아래쪽 이동을 허용하고 있는지 검사
        if (scaleValue < 0 && _climbObject.AllowDown)
        {
            // 캐릭터의 거리가 y축으로 얼만큼 아래편으로 떨어져 있는지 계산(아래에 가까울수록 결과 값은 작음)
            float yOffset = Mathf.Abs(_transfom.position.y - (_climbObjectHit.transform.position.y - ((_climbObjectHit.size.y / 2)) * _climbObjectHit.transform.localScale.y));

            // ClimbObject가 아래으로 이동하여 벗어나도록 허용하는지 검사하고 허용하지 않으면 계산 결과값이 1보다 작은지 검사
            if (!_climbObject.FallOnBottom && yOffset < 1f)
            {
                // y축으로 더 이상 이동하지 않도록 물리 값을 0으로 설정
                BasePhysicsYMove(0, 0);
            }
            else
            {
                // y 축 왼쪽으로 이동
                BasePhysicsYMove(scaleValue, _climbingDownSpeed);
            }
        }
        // 입력 값이 위쪽 방향인지, ClimbObject가 위쪽 이동을 허용하고 있는지 검사
        else if (scaleValue > 0 && _climbObject.AllowUp)
        {
            // 캐릭터의 거리가 y축으로 얼만큼 위쪽편으로 떨어져 있는지 계산(위에 가까울수록 결과 값은 작음)
            float yOffset = Mathf.Abs(_transfom.position.y - (_climbObjectHit.transform.position.y + ((_climbObjectHit.size.y / 2)) * _climbObjectHit.transform.localScale.y));

            // ClimbObject가 위로 이동하여 벗어나도록 허용하는지 검사하고 허용하지 않으면 계산 결과값이 1보다 작은지 검사
            if (!_climbObject.FallOnTop && yOffset < 1f)
            {
                // y축으로 더 이상 이동하지 않도록 물리 값을 0으로 설정
                BasePhysicsYMove(0, 0);
            }
            else
            {
                // y 축 왼쪽으로 이동
                BasePhysicsYMove(scaleValue, _climbingUpSpeed);
            }
        }

    }


    #endregion

    #endregion

}
