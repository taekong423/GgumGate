using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {

    public GameObject[] items;

    private GameObject effect;
    private Character character;

    private bool check;

	// Use this for initialization
	void Start () {
        character = GetComponent<Character>();
        check = true;
        
	}
	
	// Update is called once per frame
	void Update () {
	    if (character._statePattern._currentState == "Dead" && check)
        {
            check = false;
            DropItem();
        }
	}

    void DropItem()
    {
        int itemNum = Random.Range(0, items.Length);
        GameObject obj = (GameObject)Instantiate(items[itemNum], transform.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.2f, 0.2f), 1.0f) * 8000f);
    }
}
