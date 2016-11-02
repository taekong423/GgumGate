using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    public Transform animatedPlayerModel; //Animated model that will have all the animations in it
    bool mPlayerDead = false;
    bool mIdle = false;

    void Start()
    {
    
            animatedPlayerModel.GetComponent<Animation>()["Player@Idle"].speed = 0;

    }

    void Update()
    {
       /* //recalculate walking speed
        float walkingSpeed = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) * 0.075f;
        animatedPlayerModel.GetComponent<Animation>()["Player@Run"].speed = walkingSpeed;

        //switch to idle animation if needed
        if (walkingSpeed == 0 && animatedPlayerModel.GetComponent<Animation>()["Player@Run"].enabled)
        {
            animatedPlayerModel.GetComponent<Animation>().Play("Player@Idle");
            mIdle = true;
        }

        if (walkingSpeed > 0.01f && mIdle)
        {
            mIdle = false;
            animatedPlayerModel.GetComponent<Animation>().CrossFade("Player@Run");
        }*/
    }

    void PlayAnim(string animName)
    {
        if (!mPlayerDead)
        {
            animatedPlayerModel.GetComponent<Animation>().Play(animName);
            animatedPlayerModel.transform.localPosition = Vector3.zero; //reset any position change made by on wall anim
        }
    }

   /* void GoLeft()
    {
        Vector3 localScale = animatedPlayerModel.transform.localScale;
        localScale.z = -Mathf.Abs(localScale.z);
        animatedPlayerModel.transform.localScale = localScale;
    }*/

    /*void GoRight()
    {
        Vector3 localScale = animatedPlayerModel.transform.localScale;
        localScale.z = Mathf.Abs(localScale.z);
        animatedPlayerModel.transform.localScale = localScale;
    }*/

    public void PlayerDied()
    {
        PlayAnim("Player@Die");
        mPlayerDead = true;
    }

    public void PlayerLives()
    {
        //GoRight();
        mPlayerDead = false;
        PlayAnim("Player@Run");
    }





    //MESSAGES CALLED BY PlatformerPhysics.cs:
    void StartedJump()
    {
        PlayAnim("Player@Jump");
    }

    void StartedHightJump()
    {
        PlayAnim("Player@Jumps");
    }

    void StartedAttack()
    {
        PlayAnim("Player@Attack");
    }

   /* void StoppedCrouching()
    {
        PlayAnim("slideout");

        if (GetComponent<Player>().IsOnWall())
            LandedOnWall();
        else
            animatedPlayerModel.GetComponent<Animation>().CrossFade("walk", 2.0f);
    }

   
  /*  void ReleasedWall()
    {
        print("released");
        if (!animatedPlayerModel.GetComponent<Animation>()["jump"].enabled && !GetComponent<PlatformerPhysics>().IsCrouching())
            PlayAnim("walk");
    }

    void StartedSprinting()
    {
        //print("Start Sprint");
    }

    void StoppedSprinting()
    {
        //print("Stop Sprint");
    }*/
}
