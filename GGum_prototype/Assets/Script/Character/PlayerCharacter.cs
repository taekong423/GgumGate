using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float x = GameController.This.ButtonAxis(EButtonCode.MoveX);

        MoveHorizontal(x);
	}
}
