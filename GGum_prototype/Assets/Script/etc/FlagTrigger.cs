using UnityEngine;
using System.Collections;

public class FlagTrigger : MonoBehaviour {

    public string flagName;
    bool triggerOn;
    GameManager gm;

	// Use this for initialization
	void Start () {
        triggerOn = true;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.tag != "Entrance")
            {
                if (triggerOn)
                {
                    gm.flags[flagName] = true;
                    triggerOn = false;
                }
            }
            else
                gm.flags[flagName] = true;

        }
    }
}
