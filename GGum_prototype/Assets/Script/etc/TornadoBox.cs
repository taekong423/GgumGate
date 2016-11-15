using UnityEngine;
using System.Collections;

public class TornadoBox : MonoBehaviour {

    bool _isIn = false;
    bool _isThrow = false;
    bool _isThrowing = false;

    float _dist;

    Transform _transform;
    Transform _player;

    Vector3 _pos;

    float _seta = 0;

    public float _speed = 20.0f;
    public float _power = 20.0f;
    public float _throwRange = 20.0f;
    public float _height = 95.0f;
    public float _addForce = 200.0f;

    void Awake()
    {
        _transform = transform;
        _pos = _transform.position + new Vector3(0, _height, 0);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            _isIn = true;
            _player = coll.transform;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            _isIn = false;
            _isThrow = false;
            _isThrowing = false;
            
        }
    }

    void FixedUpdate()
    {
        if (_isIn)
        {
            
            if (_isThrow)
            {
                if (!_isThrowing)
                {
                    //_player.Translate(Vector3.up * _speed * Time.fixedDeltaTime);
                    _player.position = Vector2.Lerp(_player.position, new Vector3(_pos.x + Mathf.Sin(_seta) * 200, _pos.y+1, _player.position.z), _speed * Time.fixedDeltaTime);
                    
                    if (_player.position.y >= _pos.y)
                    {
                        _isThrowing = true;
                        Throw((Mathf.Sin(_seta) >= 0) ? 1 : -1);
                    }

                    _seta += Time.fixedDeltaTime * 10;
                    if (_seta >= 6.28f)
                        _seta = 0;
                }

            }
            else
            {
                _dist = Vector3.Distance(_transform.position, _player.position);

                _player.position = Vector2.Lerp(_player.position, _transform.position, 50/_dist * _power * Time.fixedDeltaTime);
                if (_dist < _throwRange)
                {
                    _isThrow = true;
                    _dist = 0;
                    _player.GetComponent<Player>().isStop = true;
                    _player.GetComponent<Rigidbody2D>().gravityScale = 0;
                }

            }

        }
    }

    void Throw(float dirX)
    {
        Debug.Log("Throw");

        _player.GetComponent<Player>().isStop = false;
        _player.GetComponent<Rigidbody2D>().gravityScale = 50;

        Vector2 dir = new Vector2(dirX * 20, 10);
        _player.GetComponent<Player>().m_rigidbody.AddForce(dir * _addForce, ForceMode2D.Force);
    }

}
