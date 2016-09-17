using UnityEngine;
using System.Collections;

public class NormalPig : Enemy {

    void OnEnable()
    {
        Sprinkle();
        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Sprinkle()
    {
        float power = Random.Range(8000, 16000);

        Vector2 dir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(0.5f, 1.0f));

        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_rigidbody.AddForce(dir * power, ForceMode2D.Force);
        

        //m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        Debug.Log("Sprinkle : " + dir.ToString());

    }
}
