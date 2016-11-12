using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour {

    bool _dialogueActive = false;

    bool _displaying = false;

    DialogueData _currentDialogueData;

    int _contentIndex = 0;

    IEnumerator _coroutine;

    Player _player;

    GameObject _currentImg;

    public bool _useDelay;

    public float _secondsBetweenCharacters = 0.15f;

    public GameObject _dialogueUI;

    public Text _dialogueText;

    public DialogueData[] _dialogueDatas;

    public bool DialogueActive { get { return _dialogueActive; } }

    public bool Displaying { get { return _displaying; } set { _displaying = value; } }


    public bool _isEnd = false;

    void Awake()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _dialogueDatas = Resources.LoadAll<DialogueData>("Prefab/Dialogue");
    }

        
    DialogueData GetDialogueData(string id)
    {
        DialogueData getdata = null;

        foreach (DialogueData data in _dialogueDatas)
        {
            if (data._ID == id)
                getdata = data;
        }

        return getdata;
    }

    void SetDIalogueText(int index)
    {
        if (index >= _currentDialogueData._contents.Length)
        {
            _contentIndex = 0;
            DialogueSetActive(false);
            _currentDialogueData.IsEnd = true;
            _currentDialogueData.CallEndEvent();
            _currentDialogueData = null;
            return;
        }

        if (_currentImg != null)
            _currentImg.SetActive(false);

        string imgName = _currentDialogueData.GetIMGName(index);

        GameObject img = _dialogueUI.transform.FindChild(imgName).gameObject;//.GetComponent<Image>();

        if (img != null)
        {
            img.SetActive(true);
            _currentImg = img;
        }

        _dialogueText.text = _currentDialogueData.GetContentText(index);

        _contentIndex++;
    }

    IEnumerator SetDIalogueText_Coroutine(int index)
    {
        yield return null;

        if (index >= _currentDialogueData._contents.Length)
        {
            _contentIndex = 0;
            DialogueSetActive(false);
            _currentDialogueData.IsEnd = true;
            _currentDialogueData.CallEndEvent();
            _currentDialogueData = null;
        }

        else
        {

            _displaying = true;

            int textIndex = 0;
            string text = _currentDialogueData.GetContentText(index);

            _dialogueText.text = "";

            if (_currentImg != null)
                _currentImg.SetActive(false);

            string imgName = _currentDialogueData.GetIMGName(index);

            GameObject img = _dialogueUI.transform.FindChild(imgName).gameObject;//.GetComponent<Image>();

            if (img != null)
            {
                img.SetActive(true);
                _currentImg = img;
            }

            while (textIndex < text.Length)
            {

                if (!_displaying)
                {
                    _dialogueText.text = text;
                    break;
                }

                _dialogueText.text += text[textIndex];

                textIndex++;

                /**/

                yield return new WaitForSeconds(_secondsBetweenCharacters);
            }
           
            _displaying = false;
            _contentIndex++;
        }

        yield return null;
    }

    public void DialogueSetActive(bool active)
    {
        _player.isStop = active;
        _dialogueActive = active;
        _dialogueUI.SetActive(active);
    }

    public void DisplayDialogue(string id)
    {
        DialogueSetActive(true);

        _currentDialogueData = GetDialogueData(id);

        if (!_useDelay)
        {
            SetDIalogueText(_contentIndex);
        }
        else
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = SetDIalogueText_Coroutine(_contentIndex);

            StartCoroutine(_coroutine);
        }
        
        //
    }

    public void NextContent()
    {
        if (_dialogueActive)
        {
            if (!_displaying)
            {
                if (!_useDelay)
                {
                    SetDIalogueText(_contentIndex);
                }
                else
                {
                    if (_coroutine != null)
                        StopCoroutine(_coroutine);

                    _coroutine = SetDIalogueText_Coroutine(_contentIndex);

                    StartCoroutine(_coroutine);
                }

            }
        }

    }

    public bool GetIsEnd(string id)
    {
        return GetDialogueData(id).IsEnd;
    }

}
