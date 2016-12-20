using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour {

    public Camera Cm;

    public bool Check;
    
	// Use this for initialization
	void Start () {
	
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag=="Player")
        {
            if (Check)
            {
                //Cm.GetComponent<CameraController>().CameraMoveOn = true;
            }
            else
            {
                //Cm.GetComponent<CameraController>().CameraMoveOn = false;
            }
          
        }
    }
}
