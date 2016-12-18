using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public Transform[] _trans;

    private void Awake()
    {
        _trans = transform.GetComponentsInChildren<Transform>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
