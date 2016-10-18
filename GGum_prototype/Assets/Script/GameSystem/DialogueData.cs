using UnityEngine;
using System.Collections;

public enum DialogueButtonType
{
    Accept,
    Refuse,
    Exit,
}

[System.Serializable]
public class ContentData
{
    [System.Serializable]
    public class ButtonData
    {
        public DialogueButtonType _type;
        public string _nextID;
    }

    [TextArea(3, 5)]
    public string _content;
    public string _imgName;
    public ButtonData[] _buttonData;

}

public class DialogueData : MonoBehaviour {

    public string _ID;

    public ContentData[] _contents;


    public string GetContentText(int index)
    {
        if (_contents.Length <= 0)
            return "No Content";

        return _contents[index]._content;
    }

    public string GetIMGName(int index)
    {
        return _contents[index]._imgName;
    }
}
