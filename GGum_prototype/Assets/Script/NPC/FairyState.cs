using UnityEngine;
using System.Collections;

public class FairyState : StatePattern {

    public enum FState
    {
        Init,
        Idle,
        Follow,
        Attack,
        Dead,
    }

    protected Fairy fairy;
    protected FState fState;


    public FairyState(Fairy fairy) : base(fairy)
    {
        this.fairy = fairy;
    }

    public override void SetState(string value)
    {
        SetState<FState>(ref fState, value);
    }

    public override void StartState()
    {
        NextState("Init");
    }

    IEnumerator InitState()
    {
        Debug.Log("FairyInit");
        SetState("Idle");
        fairy.targetPos = fairy.player.fairyPoint;
        yield return null;

        NextState(fState.ToString());
    }

    IEnumerator IdleState()
    {
        //---> Add Animation
        yield return null;

        while (fState == FState.Idle)
        {
            fairy.Follow();
            yield return null;
        }

        NextState(fState.ToString());
    }

    IEnumerator FollowState()
    {
        //---> Add Animation
        yield return null;

        while (fState == FState.Follow)
        {

            yield return null;
        }

        NextState(fState.ToString());
    }
}
