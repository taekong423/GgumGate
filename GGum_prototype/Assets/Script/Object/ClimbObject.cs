using UnityEngine;
using System.Collections;

public class ClimbObject : MonoBehaviour {

    #region 변수

    [Header("General")]
    [SerializeField]
    bool _snapToMiddle = true;

    [Header("Allow Movement")]
    [SerializeField]
    bool _allowUp = true;
    [SerializeField]
    bool _allowDown = true;
    [SerializeField]
    bool _allowLeft = true;
    [SerializeField]
    bool _allowRight = true;

    [Header("Falling on the ladder")]
    [SerializeField]
    bool _fallOnTop = true;
    [SerializeField]
    bool _fallOnBottom = true;
    [SerializeField]
    bool _fallOnLeft = true;
    [SerializeField]
    bool _fallOnRight = true;

    #endregion

    #region 프로퍼티

    public bool SnapToMiddle { get { return _snapToMiddle; } }

    public bool AllowUp { get { return _allowUp; } }
    public bool AllowDown { get { return _allowDown; } }
    public bool AllowLeft { get { return _allowLeft; } }
    public bool AllowRight { get { return _allowRight; } }

    public bool FallOnTop{ get { return _fallOnTop; } }
    public bool FallOnBottom { get { return _fallOnBottom; } }
    public bool FallOnLeft { get { return _fallOnLeft; } }
    public bool FallOnRight { get { return _fallOnRight; } }

    #endregion

}
