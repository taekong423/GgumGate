using UnityEngine;
using System.Collections;


[System.Serializable]
public class ContentsData
{
    [TextArea(3, 5)]
    public string _contents;
    public IllustrationType _illuType;
}

public class DialogueData : MonoBehaviour
{


    #region 변수

    [SerializeField]
    DialogueID _ID;
    [SerializeField]
    ContentsData[] _contents;

    #endregion

    #region 프로퍼티


    public DialogueID GetID
    {
        get
        {
            return _ID;
        }
    }

    #endregion

    #region 메소드

    // index에 해당하는 대화 내용 데이터를 반환함
    public ContentsData GetContentsData(int index)
    {
        if (_contents.Length == 0)
            return null;
        else
            return _contents[index];
    }

    // 모든 대화 내용 데이터를 반환함
    public ContentsData[] GetAllContentsData()
    {
        return _contents;
    }

    #endregion

} 
