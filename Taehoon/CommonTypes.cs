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

public struct HitInfo
{
    public string name;
    public int damage;
}
