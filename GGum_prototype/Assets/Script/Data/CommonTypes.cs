using UnityEngine;
using System.Collections;

public enum State
{
    Idle,
    Move,
    Attack,
    Hit,
    Dead,
}

public enum Axis
{
    Horizontal,
    Vertical
}

public enum MoveType
{
    Once,
    PingPong,
    Random,
}

public struct HitInfo
{
    public string name;
    public int damage;
}
