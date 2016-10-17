using UnityEngine;
using System.Collections;

public class DialogueCaller : MonoBehaviour {


    DialogueManager _dm;

    bool _isEnter;

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
                    if(!_dm.Displaying)
                        _dm.DisplayDialogue(_dialogueID);
                }
                else
                {
                    _dm.NextContent();
                }

            }
            else
            {

                if (GameController.ButtonDown(EButtonCode.Attack))
                {
                    if (!_dm.DialogueActive)
                    {
                        if (!_dm.Displaying)
                            _dm.DisplayDialogue(_dialogueID);
                    }
                    else
                    {
                       
                    }

                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                _dm.NextContent();

                if (_dm.Displaying && _dm._useDelay)
                {
                    _dm.Displaying = false;
                }
            }

            

        }

    }
}
