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
    public GameObject attacker;
    public int damage;

    public HitInfo(GameObject attacker = null, int damage = 0)
    {
        this.attacker = attacker;
        this.damage = damage;
    }
}
