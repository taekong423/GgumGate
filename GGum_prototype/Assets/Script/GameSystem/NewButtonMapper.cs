using UnityEngine;
using System.Collections;

public class NewButtonMapper : MonoBehaviour {

    bool _isDown;

	// Use this for initialization
	void Start () {
	
	}

    void OnDestroy()
    {
        Debug.Log("MapperDestroy");
    }

    public bool IsDown()
    {
        bool isDown = _isDown;

        if (_isDown)
            _isDown = false;

         return isDown;

    }


}
