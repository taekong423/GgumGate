using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

    [System.Serializable]
    public struct EntrancePosition
    {
        public Vector2 enter;
        public Vector2 exit;
    }

    public EntrancePosition[] entrancePositions;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
