using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour {

    #region 변수

    // 필요 파편 갯수
    [SerializeField]
    int _numFragment = 1;

    [SerializeField]
    Transform _startPoint;

    #endregion

    #region 메소드

    public int GetFragment()
    {
        return _numFragment;
    }

    public Transform GetStartPoint()
    {
        return _startPoint;
    }

    #endregion
}
