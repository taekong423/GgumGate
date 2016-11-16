using UnityEngine;
using System.Collections;

public class DialogueCaller : MonoBehaviour {


    DialogueManager _dm;

    bool _isEnter;

    bool _autoPlay = false;

    public bool _isAuto = false;

    public string _dialogueID;

    
    void Awake()
    {
        _dm = GameObject.FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {

            _isEnter = true;  
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {

            _isEnter = false;
        }

    }

    void Update()
    {
        if (_isEnter)
        {

            if (_isAuto)
            {
                if (!_dm.DialogueActive)
                {
                    if (!_dm.Displaying && !_autoPlay)
                    {
                        _autoPlay = true;
                        _dm.DisplayDialogue(_dialogueID);
                    }
                }

            }
            else
            {

                if (GameController.ButtonPress(EButtonCode.Attack))
                {
                    if (!_dm.DialogueActive)
                    {
                        if (!_dm.Displaying)
                            _dm.DisplayDialogue(_dialogueID);
                    }

                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!_dm.Displaying)
                {
                    Debug.Log("true");
                    _dm.NextContent();
                    return;
                }
                if (_dm.Displaying && _dm._useDelay)
                {
                    Debug.Log("False");
                    _dm.Displaying = false;
                }

                
            }

            

        }

    }
}
