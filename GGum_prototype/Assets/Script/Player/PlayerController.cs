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
    }

    #endregion

}
