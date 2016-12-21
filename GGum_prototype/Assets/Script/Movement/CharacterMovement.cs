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

    bool _isFalling = false; // 공중에 있으면 true, 아니면 false

    int _jumpCount = 0;

    #endregion

    #region 프로퍼티

    public bool IsWalking { get { return _isWalking; } private set { _isWalking = value; _isRunning = !value; } }
    public bool IsRunning { get { return _isRunning; } private set { _isRunning = value; _isWalking = !value; } }

    public bool IsFalling { get { return _isFalling; } private set { _isFalling = value; } }
    public bool IsGround { get { return !_isFalling; } }

    #endregion

    #region 메소드

    void Awake()
    {
        _transfom = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction, float scaleValue, bool isRun = false)
    {
        if (scaleValue == 0.0f)
            return;

        if (_walkSpace == Space.Self)
        {
            _transfom.localEulerAngles = new Vector3(0, (scaleValue > 0) ? 0 : 180, 0);
            scaleValue = 1.0f;
        }

        if (!isRun)
        {
            BaseMove(direction, scaleValue, _walkSpeed, _walkSpace);
        }
        else
        {
            BaseMove(direction, scaleValue, _runSpeed, _walkSpace);
        }
    }

    public void Jump()
    {

    }

    #endregion

}
