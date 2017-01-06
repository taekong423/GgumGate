using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    #region 변수

    [SerializeField]
    Player _player;

    float _xAxis = 0;
    float _yAxis = 0;

    #endregion


    #region 메소드

    void Update()
    {
        InputPlayer();
    }

    void InputPlayer()
    {
        _xAxis = Input.GetAxisRaw("Horizontal");
        _yAxis = Input.GetAxisRaw("Vertical");

        _player.Move(_xAxis, _yAxis);
        
        if (Input.GetKey(KeyCode.C))
        {
            _player.OnAttack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Jump();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _player.OnFly();
        }
        else
        {
            _player.OffFly();
        }


    }

    #endregion

}
