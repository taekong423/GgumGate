using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    Player player;

    public GameData gameData;
    public ScreenFade screen;
    public GameObject boss;
    public GameObject squirrel;
    CameraController cameraController;

    public Dictionary<string, bool> flags = new Dictionary<string, bool>();
    public string[] flagKeys;

    public Transform[] cameraStopPoints;
    public GameObject[] stages;

    public int currentStageNumber;

    private Vector2 playerRespawnPos;

    bool zoom = true;

    int touchCount;
    bool isTouching;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        cameraController = Camera.main.GetComponent<CameraController>();
        for (int i = 0; i < flagKeys.Length; i++)
        {
            flags.Add(flagKeys[i], false);
        }

        currentStageNumber = 0;
        playerRespawnPos = new Vector2(1000, 0);
        touchCount = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isTouching)
                {
                    StartCoroutine(TouchCheck(0.5f));
                }

                touchCount++;
                if (touchCount == 1) Application.Quit();
            }
        }

        if (boss.activeInHierarchy)
        {
            if (boss.GetComponent<Character>()._statePattern.CurrentState == "Dead")
            {
                flags[flagKeys[2]] = true;
            }
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

        if (flags[flagKeys[4]] == true)
        {
            flags[flagKeys[4]] = false;
            StartCoroutine(BackToLastStage());
        }
    }

    IEnumerator TouchCheck(float time)
    {
        isTouching = true;

        yield return new WaitForSeconds(time);
        touchCount = 0;
        isTouching = false;
    }

    IEnumerator EngageSquirrel()
    {
        player.isStop = true;
        cameraController.ZoomIn(squirrel.transform);
        yield return new WaitForSeconds(2.5f);
        player.isStop = false;
        cameraController.ZoomOut();
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1.0f);
        screen.FadeIn();
        yield return new WaitForSeconds(screen.fadeTime);
        SceneManager.LoadScene("InGame");
    }

    IEnumerator EnterOtherStage(int stageNumber)
    {
        player.isStop = true;
        GameObject lastStage = stages[currentStageNumber];
        screen.FadeIn();
        yield return new WaitForSeconds(screen.fadeTime);
        currentStageNumber = stageNumber;
        player.transform.position = playerRespawnPos;
        Camera.main.transform.position = new Vector3(1080, 0, Camera.main.transform.position.z);
        lastStage.transform.position = new Vector2(1000, -1000);
        lastStage.SetActive(false);
        stages[stageNumber].transform.position = new Vector2(1000, 0);
        stages[stageNumber].SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        screen.FadeOut();
        player.isStop = false;
    }

    IEnumerator BackToLastStage()
    {
        player.isStop = true;
        GameObject lastStage = stages[currentStageNumber];
        screen.FadeIn();
        yield return new WaitForSeconds(screen.fadeTime);
        currentStageNumber--;
        player.transform.position = gameData.entrancePositions[currentStageNumber].exit;
        Camera.main.transform.position = new Vector3(gameData.entrancePositions[currentStageNumber].exit.x, gameData.entrancePositions[currentStageNumber].exit.y, Camera.main.transform.position.z);
        lastStage.transform.position = new Vector2(1000, -1000);
        lastStage.SetActive(false);
        stages[currentStageNumber].transform.position = new Vector2(1000, 0);
        stages[currentStageNumber].SetActive(true);

        yield return new WaitForSeconds(0.5f);
        screen.FadeOut();
        player.isStop = false;
    }
}
