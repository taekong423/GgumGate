using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float x = GameController.This.ButtonAxis(EButtonCode.MoveX);
        if (x > 0)
            container.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (x < 0)
            container.transform.rotation = Quaternion.Euler(0, 180, 0);

        Move(Axis.Horizontal, x);


        //bool jump = GameController.This.ButtonDown(EButtonCode.Jump);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.C))
            Shoot();
	}
}
