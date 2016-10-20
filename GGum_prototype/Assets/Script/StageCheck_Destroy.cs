using UnityEngine;
using System.Collections;

public class StageCheck_Destroy : MonoBehaviour {

    GameManager _gm;

    public string _stageName;

    void Awake()
    {
        _gm = GameObject.FindObjectOfType<GameManager>();
    }

	// Update is called once per frame
	void Update () {

        if (!_gm.stages[_gm.currentStageNumber].name.Equals(_stageName))
        {
            Destroy(gameObject);
        }

	}
}
