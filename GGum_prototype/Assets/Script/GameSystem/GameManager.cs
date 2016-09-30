using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public ScreenFade screen;

    GameObject playerObject;
    Player player;
    public GameObject boss;
    public GameObject squirrel;
    CameraController cameraController;

    public Dictionary<string, bool> flags = new Dictionary<string, bool>();
    public string[] flagKeys;

    public Transform[] cameraStopPoints;
    public GameObject[] stages;

    private int currentStageNumber;

    private Vector2 playerRespawnPos;

    bool zoom = true;
	// Use this for initialization
	void Start () {
        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<Player>();
        cameraController = Camera.main.GetComponent<CameraController>();
        for (int i = 0; i < flagKeys.Length; i++)
        {
            flags.Add(flagKeys[i], false);
        }

        currentStageNumber = 0;
        playerRespawnPos = new Vector2(-500, -30);
    }
	
	// Update is called once per frame
	void Update () {
	    if (player.CheckState("Dead"))
        {
            StartCoroutine(RestartGame());
        }

        if (boss.GetComponent<Character>()._statePattern._currentState == "Dead")
        {
            flags[flagKeys[2]] = true;
        }

        if (flags[flagKeys[0]] == true)
        {
            if (zoom)
            {
                zoom = false;
                StartCoroutine(EngageSquirrel());
            }
            
        }

        if (flags[flagKeys[1]] == true)
        {
            cameraController.CurrTarget = cameraStopPoints[1];
        }

        if (flags[flagKeys[2]] == true)
        {
            cameraController.CurrTarget = cameraStopPoints[0];
        }

        if (flags[flagKeys[3]] == true)
        {
            flags[flagKeys[3]] = false;
            StartCoroutine(EnterOtherStage(1));
        }
    }

    IEnumerator EngageSquirrel()
    {
        playerObject.GetComponent<Player>().isStop = true;
        cameraController.ZoomIn(squirrel.transform);
        yield return new WaitForSeconds(2.5f);
        playerObject.GetComponent<Player>().isStop = false;
        cameraController.ZoomOut();
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1.0f);
        screen.FadeIn();
        yield return new WaitForSeconds(screen.fadeTime);
        SceneManager.LoadScene("Scene1");
    }

    IEnumerator EnterOtherStage(int stageNumber)
    {
        playerObject.GetComponent<Player>().isStop = true;
        GameObject lastStage = stages[currentStageNumber];
        screen.FadeIn();
        yield return new WaitForSeconds(screen.fadeTime);
        currentStageNumber = stageNumber;
        playerObject.transform.position = playerRespawnPos;
        Camera.main.transform.position = new Vector3(-400, 0, Camera.main.transform.position.z);
        lastStage.transform.position = new Vector2(0, -1000);
        lastStage.SetActive(false);
        stages[stageNumber].transform.position = Vector2.zero;
        stages[stageNumber].SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        screen.FadeOut();
        playerObject.GetComponent<Player>().isStop = false;
    }
}
