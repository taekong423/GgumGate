using UnityEngine;
using System;
using System.Collections;

public class Effect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }

    public void TurnOffEffect()
    {
        gameObject.SetActive(false);
    }

    public void TurnOnEffect()
    {
        gameObject.SetActive(true);
    }
}
