using UnityEngine;
using System.Collections;
using System;

public class Search_Chase_Attack : Search_Chase {

    public Search_Chase_Attack(AICharacter character) : base(character)
    {
    }

    public override void Operate()
    {
        if (AttackSearch())
        {
            _character.SetStatePattern<AttackState>();
        }
        else if (Search())
        {
            _character.SetStatePattern<ChaseState>();
        }
        else
        {
            _character.SetStatePattern<MoveState>();
        }
    }

    protected bool AttackSearch()
    {
        return _character.Search(_character._player.transform, _character._attackRange);
    }
}
