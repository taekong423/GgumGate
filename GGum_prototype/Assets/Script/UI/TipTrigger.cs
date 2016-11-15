using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipTrigger : MonoBehaviour {

    public List<string> TipList;

    [HideInInspector]
    public bool check;

    private Tip tip;


	// Use this for initialization
	void Start () {
        check = true;
        tip = GameObject.Find("Tip").GetComponent<Tip>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (check)
        {
            if (other.CompareTag("Player"))
            {
                check = false;
                tip.ShowTip(TipList);
            }
        }
    }
}
