using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public ScreenFade screen;

    GameObject player;
    public GameObject boss;
    CameraController cameraController;


    public Dictionary<string, bool> flags = new Dictionary<string, bool>();
    public string[] flagKeys;

    public Transform[] cameraStopPoints;

    void Awake ()
    {
        
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        cameraController = Camera.main.GetComponent<CameraController>();
        for (int i = 0; i < flagKeys.Length; i++)
        {
            flags.Add(flagKeys[i], false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (player.GetComponent<PlayerCharacter>().CurrentState == State.Dead)
        {
            StartCoroutine(RestartGame());
        }

        if (boss.GetComponent<Character>().CurrentState == State.Dead)
        {
            flags[flagKeys[2]] = true;
        }

        if (flags[flagKeys[1]] == true)
        {
            cameraController.CurrTarget = cameraStopPoints[1];
        }

        if (flags[flagKeys[2]] == true)
        {
            cameraController.CurrTarget = cameraStopPoints[0];
        }
	}

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1.0f);
        screen.FadeIn();
        yield return new WaitForSeconds(screen.fadeTime);
        SceneManager.LoadScene("Scene1");
    }
}
