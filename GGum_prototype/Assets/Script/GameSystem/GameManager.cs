using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public ScreenFade screen;
    GameObject player;

    void Awake ()
    {
        
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
	    if (player.GetComponent<PlayerCharacter>().CurrentState == State.Dead)
        {
            StartCoroutine(RestartGame());
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
