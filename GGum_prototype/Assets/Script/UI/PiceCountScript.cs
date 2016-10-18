using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PiceCountScript : MonoBehaviour {

    Inventory Inven;
    public Text text;
   
	// Use this for initialization
	void Start () {

        Inven = GameObject.FindWithTag("Player").GetComponent<Inventory>();

	}
	

	// Update is called once per frame
	void LateUpdate () {

        text.text = Inven.questItems[new ItemData(1, "Fragment")].ToString();

    }
}
