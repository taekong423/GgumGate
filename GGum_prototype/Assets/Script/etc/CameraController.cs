using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    #region 변수

    Transform _transform;

    [Header("Follow")]
    public float _xMargin = 1.0f; // 카메라가 따라 가기 전에 플레이어가 이동할 수있는 x 축의 거리.
    public float _yMargin = 1.0f; // 카메라가 따라 가기 전에 플레이어가 이동할 수있는 y 축의 거리.
    public float _xSmooth = 1.0f; // 카메라가 x 축의 목표 이동을 따라잡는 속도
    public float _ySmooth = 1.0f; // 카메라가 y 축의 목표 이동을 따라잡는 속도

    public Transform _followtarget;

    bool _isFollowing = false;

    [Header("Shake")]
    public float _shakeRange = 5.0f;
    public float _shakeCycle = 0.05f;

    bool _isShaking = false;

    bool _isFocusing = false;

    #endregion

    #region 메소드

    void Awake()
    {
        _transform = transform;
        _isFollowing = true;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //ShakeCamera(3.0f, _shakeRange);
        }

        FollowTarget();
    }

    #region Follow

    bool CheckXMargin()
    {
        // x 축에서 카메라와 플레이어 사이의 거리가 x 여백보다 큰 경우 true를 반환합니다.
        return Mathf.Abs(_transform.position.x - _followtarget.position.x) > _xMargin;
    }

    bool CheckYMargin()
    {
        // x 축에서 카메라와 플레이어 사이의 거리가 x 여백보다 큰 경우 true를 반환합니다.
        return Mathf.Abs(_transform.position.y - _followtarget.position.y) > _yMargin;
    }

    // 타겟을 따라가는 함수
    void FollowTarget()
    {
        if (_followtarget == null || !_isFollowing)
            return;

        float targetX = _transform.position.x;
        float targetY = _transform.position.y;

        if (CheckXMargin())
            targetX = Mathf.Lerp(_transform.position.x, _followtarget.position.x, _xSmooth * Time.deltaTime);

        if (CheckYMargin())
            targetY = Mathf.Lerp(_transform.position.y, _followtarget.position.y, _ySmooth * Time.deltaTime);

        _transform.position = new Vector3(targetX, targetY, transform.position.z);

    }

    // Target Follow 기능을 활성화 또는 비활성화 하는 함수
    public void SetFollowing(bool isFollowing)
    {
        _isFollowing = isFollowing;
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
    public void ShakeCamera(float shakeTime, float shakeRange = 5.0f, float shakeCycle = 0.05f)
    {
        if (!_isShaking)
        {
            StartCoroutine(ShakeForSeconds(shakeTime));
            StartCoroutine(Shake(shakeRange, shakeCycle));
        }
    }

    // 카메라 흔들기를 활성화 또는 비활성화 하는 함수
    public void ShakeCamera(bool isShaking, float shakeRange = 5.0f, float shakeCycle = 0.05f)
    {
        if (_isShaking)
        {
            _isShaking = isShaking;
        }
        else
        {
            _isShaking = isShaking;
            StartCoroutine(Shake(shakeRange, shakeCycle));
        }
    }

    #endregion

    #region Focus

    IEnumerator Focus(Transform target, bool focusAfterArrival)
    {
        _isFocusing = true;

        while (_isFocusing)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target, );

            yield return new WaitForFixedUpdate();
        }
    }

    // 타겟 포커스를 시작하는 함수
    public void FocusTarget(Transform target, bool focusAfterArrival = false)
    {

    }

    #endregion




    #endregion

}
