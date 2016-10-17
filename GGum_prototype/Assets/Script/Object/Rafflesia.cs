using UnityEngine;
using System.Collections;

public class Rafflesia : MonoBehaviour {

    public float firstForce;
    public float secondForce;
    public Vector2 pushDirection;
    public bool cliff;
    GameManager gm;
    Inventory inventory;
	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            Rigidbody2D m_rigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            if (gm.flags["DefeatBossPig"] == true)
            {
                m_rigidbody.velocity = Vector2.zero;
                if (cliff)
                    m_rigidbody.AddForce(Vector2.up * secondForce * (inventory.questItems[new ItemData(1, "Fragment")]/3.0f), ForceMode2D.Impulse);
                else
                    m_rigidbody.AddForce(Vector2.up * secondForce, ForceMode2D.Impulse);
            }
            else
            {
                m_rigidbody.velocity = Vector2.zero;
                m_rigidbody.AddForce(pushDirection * firstForce, ForceMode2D.Impulse);
            }
            
        }
    }
}
