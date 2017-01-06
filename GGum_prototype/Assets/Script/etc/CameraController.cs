using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour {

    #region 변수

    static CameraController _instance;

    Transform _transform;
    BoxCollider _collider;
    Camera _camera;

    [Header("Follow")]
    [SerializeField]
    float _xMargin = 1.0f; // 카메라가 따라 가기 전에 플레이어가 이동할 수있는 x 축의 거리.
    [SerializeField]
    float _yMargin = 1.0f; // 카메라가 따라 가기 전에 플레이어가 이동할 수있는 y 축의 거리.
    [SerializeField]
    float _xSmooth = 1.0f; // 카메라가 x 축의 목표 이동을 따라잡는 속도
    [SerializeField]
    float _ySmooth = 1.0f; // 카메라가 y 축의 목표 이동을 따라잡는 속도
    [SerializeField]
    Transform _followTarget;

    bool _isFollowing = false;

    [Header("Shake")]
    [SerializeField]
    float _shakeRange = 5.0f;
    [SerializeField]
    float _shakeCycle = 0.05f;

    bool _isShaking = false;

    [Header("Focus")]
    [SerializeField]
    float _focusSpeed = 10.0f;
    [SerializeField]
    float _zoomOutSpeed = 30.0f;
    [SerializeField]
    Transform _focusTarget;

    bool _isFocusing = false;

    float _originalSize;

    [Header("Fade")]
    [SerializeField]
    Image _fadeScreen;

    bool _isFading = false;

    #endregion

    #region 프로퍼티

    public static CameraController Instance
    {
        get
        {
            //if

            return _instance;
        }
    }

    #endregion

    #region 메소드

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        _transform = transform;
        _camera = GetComponent<Camera>();
        _isFollowing = true;
        _originalSize = _camera.orthographicSize;
        _collider = GetComponent<BoxCollider>();

        SetColliderSize();

    }

    void FixedUpdate()
    {
        FollowTarget();
    }

    void SetColliderSize()
    {
        float height = _camera.orthographicSize * 2;
        float width = _camera.aspect * height;

        _collider.size = new Vector3(width, height, 100);
    }

    #region Follow

    bool CheckXMargin()
    {
        // x 축에서 카메라와 플레이어 사이의 거리가 x 여백보다 큰 경우 true를 반환합니다.
        return Mathf.Abs(_transform.position.x - _followTarget.position.x) > _xMargin;
    }

    bool CheckYMargin()
    {
        // x 축에서 카메라와 플레이어 사이의 거리가 x 여백보다 큰 경우 true를 반환합니다.
        return Mathf.Abs(_transform.position.y - _followTarget.position.y) > _yMargin;
    }

    // 타겟을 따라가는 함수
    void FollowTarget()
    {
        if (_followTarget == null || !_isFollowing)
            return;

        float targetX = _transform.position.x;
        float targetY = _transform.position.y;

        if (CheckXMargin())
            targetX = Mathf.Lerp(_transform.position.x, _followTarget.position.x, _xSmooth * Time.deltaTime);

        if (CheckYMargin())
            targetY = Mathf.Lerp(_transform.position.y, _followTarget.position.y, _ySmooth * Time.deltaTime);

        _transform.position = new Vector3(targetX, targetY, transform.position.z);

    }

    // Target Follow 기능을 활성화 또는 비활성화 하는 함수
    public static void SetFollowing(bool isFollowing)
    {
        Instance._isFollowing = isFollowing;
    }

    #endregion

    #region Shake

    // 카메라를 흔드는 코루틴 함수
    IEnumerator Shake(float shakeRange, float shakeCycle)
    {
        SetFollowing(false);

        Vector3 originalPos = _transform.position;

        float x;
        float y;

        while (_isShaking)
        {
            x = originalPos.x + Random.Range(-shakeRange, shakeRange);
            y = originalPos.y + Random.Range(-shakeRange, shakeRange);

            _transform.position = new Vector3(x, y, originalPos.z);

            yield return new WaitForSeconds(shakeCycle);
        }

        SetFollowing(true);

    }

    // 카메라 쉐이크를 활성화 시키고 몇 초후 비활성화 시키는 코루틴 함수
    IEnumerator ShakeForSeconds(float shakeTime)
    {
        _isShaking = true;

        yield return new WaitForSeconds(shakeTime);

        _isShaking = false;
    }


    // 카메라를 시간에 따라 흔드는 함수
    public static void ShakeCamera(float shakeTime, float shakeRange = 5.0f, float shakeCycle = 0.05f)
    {
        if (!Instance._isShaking)
        {
            Instance.StartCoroutine(Instance.ShakeForSeconds(shakeTime));
            Instance.StartCoroutine(Instance.Shake(shakeRange, shakeCycle));
        }
    }

    // 카메라 흔들기를 활성화 또는 비활성화 하는 함수
    public static void ShakeCamera(bool isShaking, float shakeRange = 5.0f, float shakeCycle = 0.05f)
    {
        if (Instance._isShaking)
        {
            Instance._isShaking = isShaking;
        }
        else
        {
            Instance._isShaking = isShaking;
            Instance.StartCoroutine(Instance.Shake(shakeRange, shakeCycle));
        }
    }

    #endregion

    #region Focus

    // 타겟을 포커싱하는 함수
    IEnumerator Focus(Transform target, float waitTime, float zoomSize, bool focusAfterArrival)
    {
        _isFocusing = true;

        SetFollowing(false);

        // 타겟 지점까지 이동이 완료되면 true, 아니면 false
        bool endMove = false;
        // zoomIn이 완료되면 true, 아니면 false
        bool endZoomIn = false;

        // 카메라가 이동할 지점
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, _transform.position.z);

        // 카메라와 타겟 포인트와의 거리(zoom을 할때 거리에 비례하여 zoomInOut을 위한 값)
        float distance = Vector2.Distance(targetPos, _transform.position);

        while (_isFocusing)
        {
            // 타겟까지 이동
            if(!endMove)
                _transform.position = Vector3.MoveTowards(_transform.position, targetPos, _focusSpeed * Time.fixedDeltaTime);

            // 도착후 zoomIn을 할것인지 바로 zoomIn을 할것인지 검사
            // FocusAfterArrival이 true면 도착후 zoomIn, false면 바로 zoomIn
            if ((focusAfterArrival && endMove) || (!focusAfterArrival && !endMove))
            {
                // zoomIn(카메라 확대)
                if (!endZoomIn)
                {
                    _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, zoomSize, ((50 * _focusSpeed) / distance) * Time.fixedDeltaTime);
                    SetColliderSize();
                }
                // 목표한 zoomSize까지 확대했으면 zoomIn을 끝냄
                if (_camera.orthographicSize.Equals(zoomSize))
                {
                    endZoomIn = true;
                }

            }
            
            // 타겟까지 이동이 완료되면 이동 끝냄
            if (_transform.position.Equals(targetPos))
            {
                endMove = true;
            }

            // 이동과 확대가 끝나면 루프를 종료 시킨다.
            if (endMove && endZoomIn)
            {
                _isFocusing = false;
            }

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(waitTime);

        SetFollowing(true);
        StartCoroutine(ZoomOut());
    }

    // 확대된 화면을 다시 축소 시키는 코루틴 함수
    IEnumerator ZoomOut()
    {
        while (!_isFocusing)
        {
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _originalSize, _zoomOutSpeed * Time.fixedDeltaTime);
            SetColliderSize();
            if (_camera.orthographicSize.Equals(_originalSize))
                break;

            yield return null;
        }
    }

    // 타겟 포커스를 시작하는 함수
    public static void FocusTarget(Transform target, float waitTime = 1.0f, float zoomSize = 50.0f, bool focusAfterArrival = false)
    {
        if (!Instance._isFocusing)
        {
            Instance.StartCoroutine(Instance.Focus(target, waitTime, zoomSize, focusAfterArrival));
        }
        
    }

    #endregion

    #region Fade

    // 화면을 페이드하는 코루틴
    IEnumerator FadeScreen(float fadeTime, bool isFadeIn)
    {
        _isFading = true;

        float alpha = (isFadeIn) ? 0 : 1;
        float currentAlpha = (isFadeIn) ? 1 : 0;

        while (_isFading)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, alpha, (1/fadeTime) * Time.deltaTime);

            _fadeScreen.color = new Color(0, 0, 0, currentAlpha);

            if (currentAlpha == alpha)
            {
                _isFading = false;
            }

            yield return null;
        }
        
    }

    // 일정 시간안에 페이드 하는 코루틴 호출 함수
    public static void Fade(float fadeTime, bool isFadeIn = true)
    {
        if(!Instance._isFading && Instance._fadeScreen != null)
        {
            Instance.StartCoroutine(Instance.FadeScreen(fadeTime, isFadeIn));
        }

    }



    #endregion

    #endregion

}
