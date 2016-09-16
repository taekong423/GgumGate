using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossPig : Enemy {

    CameraController _camera;

    int _patternNum = 0;

    int _childPigNum = 0;


    List<GameObject> _childPigs;
    public GameObject[] _childPigPrefabs;

    
    protected override void InitCharacter()
    {
        base.InitCharacter();
        _camera = Global.shared<CameraController>();
        _player = GameObject.FindObjectOfType<PlayerCharacter>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Test1111");
        }
    }

    protected override IEnumerator InitState()
    {
        yield return new WaitForSeconds(1.0f);

        _camera.ShakeCamera(true);

        yield return new WaitForSeconds(1.0f);

        _camera.ShakeCamera(false);

        yield return null;

        animator.SetTrigger("Hide");
        Physics2D.IgnoreCollision(_player.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>(), true);

        state = State.Idle;

        yield return null;

        NextState();
        StartCoroutine(Search());

    }

    protected override IEnumerator IdleState()
    {
        while (state == State.Idle)
        {
            switch (_patternNum)
            {
                case 0:

                    state = State.Move;
                    break;

                default:
                    break;
            }

            yield return null;
        }

        yield return null;

        NextState();
    }

    protected override IEnumerator MoveState()
    {
        while (state == State.Move)
        {
            switch (_patternNum)
            {
                case 0:

                    if (_childPigNum <= 1)
                    {
                        if (GoToTarget(_target.position))
                        {
                            state = State.Attack;
                        }
                    }
                    break;

                default:
                    break;
            }

            yield return null;
        }

        yield return null;


        NextState();
    }

    protected override IEnumerator AttackState()
    {
        while (state == State.Attack)
        {
            yield return null;
        }

        yield return null;
    }

    protected override IEnumerator HitState()
    {
        while (state == State.Hit)
        {
            yield return null;
        }

        yield return null;
    }

    protected override IEnumerator DeadState()
    {
        while (state == State.Dead)
        {
            yield return null;
        }

        yield return null;
    }

    protected override IEnumerator Search()
    {
        while (state != State.Dead)
        {
            yield return null;
        }

        yield return null;
    }

    void SpawnPig(GameObject prefab, int spawnNum = 4)
    {
        
    }

    void RandomSpawnPig()
    {

    }
}
