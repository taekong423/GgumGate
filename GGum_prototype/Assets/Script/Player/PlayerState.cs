﻿using UnityEngine;
using System.Collections;

public partial class Player
{
    public class PlayerState : StatePattern
    {
        public enum State
        {
            Init,
            Idle,
            Move,
            Swim,
            Hit,
            Dead,
        }

        Player player;

        State state;

        public PlayerState(Character character) : base(character)
        {
            player = character as Player;
        }

        public override void SetState(string value)
        {
            SetState<State>(ref state, value);
        }

        public override void StartState()
        {
            NextState("Init");
        }

        IEnumerator InitState()
        {
            //InitCharacter();
            yield return null;

            state = State.Idle;
            yield return null;

            NextState(state.ToString());
        }

        IEnumerator IdleState()
        {
            if (!player.isStop)
                player.SetTrigger("Idle");
            yield return null;

            while (state == State.Idle)
            {
                player.AxisInput();
                yield return new WaitForFixedUpdate();
            }

            NextState(state.ToString());
        }

        IEnumerator MoveState()
        {
            if (!player.isStop)
                player.animator.SetTrigger("Run");
            yield return null;

            while (state == State.Move)
            {
                player.AxisInput();
                yield return new WaitForFixedUpdate();
            }

            NextState(state.ToString());
        }

        IEnumerator DeadState()
        {
            player.SetTrigger("Die");
            yield return null;

            while (state == State.Dead)
            {
                yield return null;
            }

            NextState(state.ToString());
        }
    }
}