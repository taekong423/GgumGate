using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    // 기본적인 이동 관련 클래스
    // 이동 함수들만 구현

    #region 변수

    protected Transform _transfom;
    protected Rigidbody2D _rigidbody;

    #endregion

    #region 메소드

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        _transfom = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // 기본 이동하는 함수
    protected void BaseMove(Vector2 direction, float scaleValue, float speed, Space space)
    {
        _transfom.Translate(direction * scaleValue * speed * Time.fixedDeltaTime, space);
    }

    protected void BasePhysicsXMove(float scaleValue, float speed)
    {
        _rigidbody.velocity = new Vector2(scaleValue * speed, _rigidbody.velocity.y);
    }

    protected void BasePhysicsYMove(float scaleValue, float speed)
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, scaleValue * speed);
    }

    #endregion

}
