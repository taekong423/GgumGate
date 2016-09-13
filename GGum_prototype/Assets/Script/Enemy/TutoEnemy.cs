using UnityEngine;
using System.Collections;

public class TutoEnemy : Enemy {

    protected bool _isCinematic = false;

    public bool IsCinematic { get { return _isCinematic; } set { _isCinematic = false; } }

}
