using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class Dialogue : MonoBehaviour, IPointerClickHandler
{

    #region 열거형

    public enum DialogueState
    {
        Start,
        Displaying,
        End,
    }

    #endregion

    #region 변수

    static Dialogue _instance;

    DialogueState _state;

    int _textIndex = 0;
    int _contentsIndex = 0;

    string _contents = "";

    bool _isDisplayingContents = false;

    DialogueData _currentData;

    public GameObject _dialogueUI;

    public DialogueData[] _dialogueData;

    public float _secondsBtweenCharacters = 0.15f;

    public Text _contentsText;

    public GameObject[] _illustrationList;

    #endregion

    #region 프로퍼티

    public static Dialogue Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<Dialogue>();

            return _instance;
        }
    }

    public static bool IsEnd
    {
        get
        {
            return (Instance._state == DialogueState.End);
        }
    }

    #endregion

    #region 메소드

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            DestroyImmediate(this);

        // Dialogue 폴더의 데이터들을 리스트에 삽입
        _dialogueData = Resources.LoadAll<DialogueData>("Prefab/Dialogue");

    }

    // 터치시 불리는 함수
    public void OnPointerClick(PointerEventData data)
    {
        if (_state == DialogueState.Displaying)
        {
            if (_isDisplayingContents)
            {
                SkipContents();
            }
            else
            {
                NextContents();
            }
        }
    }

    // 파라미터로 넘어온 ID랑 같은 Data를 찾아서 반환 하는 함수
    DialogueData GetData(DialogueID ID)
    {
        DialogueData getData = null;

        foreach (DialogueData data in _dialogueData)
        {
            if (data.GetID.Equals(ID))
            {
                getData = data;
                break;
            }
        }

        return getData;
    }

    // 대화 내용을 시작할때 불리는 함수, 내용을 표시할때 사용하는 변수 초기화 등
    void InitContents()
    {
        _textIndex = 0;
        _contentsText.text = "";
        _isDisplayingContents = true;
        _contents = _currentData.GetContentsData(_contentsIndex)._contents;
        // 현재 데이터 내용의 일러스트를 셋팅함
        SetIllustration(_currentData.GetContentsData(_contentsIndex)._illuType);
    }

    // 다음 내용 표시를 시작하게 해주는 함수
    void NextContents()
    {

        _contentsIndex++;

        // 데이터의 대화가 끝까지 진행 됬는지 검사, 끝까지 진행 됬으면 다이얼로그 종료.
        if (_contentsIndex >= _currentData.GetAllContentsData().Length)
        {
            // TODO : EndDialogue 추가
            EndDialogue();
            return;
        }

        // 다 끝나지 않았으면 다음 내용으로 진행 하기 위한 초기화
        InitContents();

    }

    // 다이얼로그를 종료 하고 초기화 하는 함수
    void EndDialogue()
    {
        _contentsIndex = 0;
        _isDisplayingContents = true;
        _state = DialogueState.End;
        StopAllCoroutines();
        EventManager.PostNotification(Event_Type.EndDialogue, this);
        _dialogueUI.SetActive(false);
    }

    void SetIllustration(IllustrationType _type)
    {
        // 활성화 되어 있는 일러스트를 비활성화 시킴
        foreach (GameObject illu in _illustrationList)
        {
            if (illu.activeSelf == true)
                illu.SetActive(false);
        }

        // 타입에 맞는 일러스트를 리스트에서 찾아 활성화 시킴, None은 아무것도 활성화 시키지 않는것 이므로 제외.
        if (_type != IllustrationType.None)
            _illustrationList[(int)_type].SetActive(true);
    }


    // 다이얼로그의 내용들을 표시하는 함수
    IEnumerator DisplayContents()
    {
        _state = DialogueState.Start;

        yield return null;

        _state = DialogueState.Displaying;

        while (_state == DialogueState.Displaying)
        {
            while (_isDisplayingContents)
            {
                _contentsText.text += _contents[_textIndex];
                _textIndex++;

                if (_textIndex >= _contents.Length)
                {
                    _isDisplayingContents = false;
                }

                yield return new WaitForSeconds(_secondsBtweenCharacters);
            }

            yield return null;
        }

    }

    // 다이얼로그 시작(표시)하는 함수
    public static void Display(DialogueID dataID)
    {
        DialogueData data = Instance.GetData(dataID);

        if (data == null)
        {
            Debug.Log("Data == null");
            return;
        }

        Instance._dialogueUI.SetActive(true);
        Instance._currentData = data;
        Instance.InitContents();
        Instance.StartCoroutine(Instance.DisplayContents());
    }

    // 내용 표시를 스킵하는 함수
    public void SkipContents()
    {
        _textIndex = 0;
        _contentsText.text = _contents;
        _isDisplayingContents = false;

    }

    // 내용 표시를 스킵하는 함수, 클래스 함수
    public static void SkipContents_Class()
    {
        Instance.SkipContents();
    }

    // 다이얼로그 내용들을 전부 스킵하고 종료하는 함수
    public void SkipDialogue()
    {
        EndDialogue();
    }

    // 다이얼로그 내용들을 전부 스킵하고 종료하는 함수, 클래스 함수
    public static void SkipDialogue_Class()
    {
        Instance.SkipDialogue();
    }

    #endregion
} 
