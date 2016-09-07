using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

    bool jump;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        jump = GameController.This.ButtonDown(EButtonCode.Jump);

        

        if (Input.GetKeyDown(KeyCode.C))
            Shoot();
	}

    void FixedUpdate()
    {
        float x = GameController.This.ButtonAxis(EButtonCode.MoveX);

        Flip(x);

        Move(Axis.Horizontal, x);

        if (jump)
            Jump();
    }
}
