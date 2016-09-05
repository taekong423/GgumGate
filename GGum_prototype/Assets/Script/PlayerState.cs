using UnityEngine;
using System.Collections;

public class Playstates
{
    virtual public void Do()
    {

    }
}

class AttackState : Playstates
{
    public override void Do()
    {
        Debug.Log("attack");
    }
}

class IdleState : Playstates
{
    public override void Do()
    {
        Debug.Log("idle");
    }
}



