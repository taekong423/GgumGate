using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    #region 변수

    static GameManager _instance;

    Transform _startPoint;

    [SerializeField]
    StageID _startStage;

    Stage _currentStage;

    int _currentFragment = 0;

    [SerializeField]
    List<Stage> _stageList;

    #endregion

    #region 프로퍼티

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    #endregion

    #region 메소드

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    // 페이드인하면서 시작할 스테이지를 활성화하고 플레이어를 시작 위치로 이동
    void Start()
    {
        CameraController.Fade(2, true);

        // 리스트에서 시작 스테이지를 가져와 현재스테이지에 저장하고 활성화
        _currentStage = _stageList[(int)_startStage];
        _currentStage.gameObject.SetActive(true);

        // 플레이어 트랜스폼 정보를 글로벌에서 가져옴
        Transform playerTransfom = Global.shared<Player>().transform.parent;
        
        // 현재 스테이지에 스타트 지점이 있으면 플레이어를 스타트 지점으로 이동
        if(_currentStage.GetStartPoint() != null)
            playerTransfom.position = _currentStage.GetStartPoint().position;

    }

    // 화면 페이드하면서 스테이지를 바꾸고 플레이어 위치를 스테이지 시작지점으로 이동
    IEnumerator Change(StageID stageID, Transform stagePoint)
    {
        CameraController.Fade(1, false);

        yield return new WaitForSeconds(1.5f);

        // 현재 스테이지가 있으면 비활성화
        if (_currentStage != null)
            _currentStage.gameObject.SetActive(false);

        // 매개변수로 들어온 다음 스테이지ID에 맞는 스테이지를 리스트에서 찾아 현재스테이지에 저장 후 활성화
        _currentStage = _stageList[(int)stageID];
        _currentStage.gameObject.SetActive(true);

        // 플레이어 트랜스폼 정보를 글로벌에서 가져옴
        Transform playerTransfom = Global.shared<Player>().transform.parent;

        // 매개변수로 넘어온 포인트가 널이고 스테이지 시작 지점은 존재할때 스테이지 시작지점으로 플레이어 위치 이동
        if (stagePoint == null && _currentStage.GetStartPoint() != null)
        {
            playerTransfom.position = _currentStage.GetStartPoint().position;
        }
        // 매개변수로 넘어온 포인트가 존재할때 포인트로 플레이어 이동
        else if(stagePoint != null)
        {
            playerTransfom.position = stagePoint.position;
        }

        // 가지고 있는 파편 초기화
        _currentFragment = 0;

        CameraController.Fade(1, true);
    }

    // 스테이지를 바꾸는 코루틴 호출 함수
    public static void ChangeStage(StageID stageID, Transform stagePoint = null)
    {
        Instance.StartCoroutine(Instance.Change(stageID, stagePoint));
    }

    #endregion

    #region old
    //   Player player;

    //   public GameData gameData;
    //   public ScreenFade screen;
    //   public GameObject[] boss;
    //   public GameObject squirrel;
    //   CameraController cameraController;

    //   public Dictionary<string, bool> flags = new Dictionary<string, bool>();
    //   public string[] flagKeys;

    //   public Transform[] cameraStopPoints;
    //   public GameObject[] stages;

    //   public int currentStageNumber;

    //   private Vector2 playerRespawnPos;

    //   bool zoom = true;

    //   int touchCount;
    //   bool isTouching;

    //// Use this for initialization
    //void Start () {
    //       player = GameObject.FindWithTag("Player").GetComponent<Player>();
    //       gameData = GameObject.Find("GameData").GetComponent<GameData>();
    //       cameraController = Camera.main.GetComponent<CameraController>();
    //       for (int i = 0; i < flagKeys.Length; i++)
    //       {
    //           flags.Add(flagKeys[i], false);
    //       }

    //       currentStageNumber = 0;
    //       playerRespawnPos = new Vector2(1000, 0);
    //       touchCount = 0;
    //   }

    //// Update is called once per frame
    //void Update () {
    //       /*
    //       if (Application.platform == RuntimePlatform.Android)
    //       {
    //           if (Input.GetKeyDown(KeyCode.Escape))
    //           {
    //               if (!isTouching)
    //               {
    //                   StartCoroutine(TouchCheck(0.5f));
    //               }

    //               touchCount++;
    //               if (touchCount == 1) Application.Quit();
    //           }
    //       }
    //       */
    //       if (boss[0].activeInHierarchy)
    //       {
    //           if (boss[0].GetComponent<ICharacter>().CurrentState == "Dead")
    //           {
    //               flags[flagKeys[2]] = true;
    //           }
    //       }

    //       if (boss[1].activeInHierarchy)
    //       {
    //           if (boss[1].GetComponent<ICharacter>().CurrentState == "Dead")
    //           {
    //               flags[flagKeys[6]] = true;
    //           }
    //       }

    //       if (flags[flagKeys[0]] == true)
    //       {
    //           if (zoom)
    //           {
    //               zoom = false;
    //               StartCoroutine(EngageSquirrel());
    //           }
    //       }

    //       if (flags[flagKeys[1]] == true)
    //       {
    //           //cameraController.CurrTarget = cameraStopPoints[1];
    //       }

    //       if (flags[flagKeys[2]] == true)
    //       {
    //           //cameraController.CurrTarget = cameraStopPoints[0];
    //       }

    //       if (flags[flagKeys[3]] == true)
    //       {
    //           flags[flagKeys[3]] = false;
    //           StartCoroutine(EnterOtherStage(1));
    //       }

    //       if (flags[flagKeys[4]] == true)
    //       {
    //           flags[flagKeys[4]] = false;
    //           StartCoroutine(BackToLastStage());
    //       }

    //       if (flags[flagKeys[5]] == true)
    //       {
    //           flags[flagKeys[5]] = false;
    //           //cameraController.cameraClamp.xMin = 3170.0f;
    //           //cameraController.cameraClamp.xMax = 3515.0f;
    //       }

    //       if (flags[flagKeys[6]] == true)
    //       {
    //           flags[flagKeys[6]] = false;
    //           //cameraController.cameraClamp = gameData.cameraClamp[1];
    //       }
    //   }

    //   IEnumerator TouchCheck(float time)
    //   {
    //       isTouching = true;

    //       yield return new WaitForSeconds(time);
    //       touchCount = 0;
    //       isTouching = false;
    //   }

    //   IEnumerator EngageSquirrel()
    //   {
    //       //player.isStop = true;
    //       //cameraController.ZoomIn(squirrel.transform);
    //       yield return new WaitForSeconds(2.5f);
    //       //player.isStop = false;
    //       //cameraController.ZoomOut();
    //   }

    //   IEnumerator RestartGame()
    //   {
    //       yield return new WaitForSeconds(1.0f);
    //       screen.FadeIn();
    //       yield return new WaitForSeconds(screen.fadeTime);
    //       SceneManager.LoadScene("InGame");
    //   }

    //   IEnumerator EnterOtherStage(int stageNumber)
    //   {
    //       //player.isStop = true;
    //       GameObject lastStage = stages[currentStageNumber];
    //       screen.FadeIn();
    //       yield return new WaitForSeconds(screen.fadeTime);
    //       currentStageNumber = stageNumber;
    //       player.transform.position = playerRespawnPos;
    //       Camera.main.transform.position = new Vector3(1080, 0, Camera.main.transform.position.z);
    //       //cameraController.cameraClamp = gameData.cameraClamp[currentStageNumber];
    //       lastStage.transform.position = new Vector2(1000, -1000);
    //       lastStage.SetActive(false);
    //       stages[stageNumber].transform.position = new Vector2(1000, 0);
    //       stages[stageNumber].SetActive(true);

    //       Global.shared<SoundManager>().ChangeBGM(stages[stageNumber].name);

    //       yield return new WaitForSeconds(0.5f);
    //       screen.FadeOut();
    //       //player.isStop = false;
    //   }

    //   IEnumerator BackToLastStage()
    //   {
    //       //player.isStop = true;
    //       GameObject lastStage = stages[currentStageNumber];
    //       screen.FadeIn();
    //       yield return new WaitForSeconds(screen.fadeTime);
    //       currentStageNumber--;
    //       player.transform.position = gameData.entrancePositions[currentStageNumber].exit;
    //       Camera.main.transform.position = new Vector3(gameData.entrancePositions[currentStageNumber].exit.x, gameData.entrancePositions[currentStageNumber].exit.y, Camera.main.transform.position.z);
    //       //cameraController.cameraClamp = gameData.cameraClamp[currentStageNumber];
    //       lastStage.transform.position = new Vector2(1000, -1000);
    //       lastStage.SetActive(false);
    //       stages[currentStageNumber].transform.position = new Vector2(1000, 0);
    //       stages[currentStageNumber].SetActive(true);

    //       Global.shared<SoundManager>().ChangeBGM(stages[currentStageNumber].name);

    //       yield return new WaitForSeconds(0.5f);
    //       screen.FadeOut();
    //       //player.isStop = false;
    //   } 
    #endregion
}
