using UnityEngine;
using System.Collections;
using System;

public class Search_Chase_Attack : Searchable {

    public Search_Chase_Attack(AICharacter character) : base(character)
    {
    }

    public override void Operate()
    {
        if (AttackSearch())
        {
            _character.SetStatePattern<AttackState>();
        }
        else if (ChaseSearch())
        {
            _character.SetStatePattern<ChaseState>();
        }
    }

    protected bool ChaseSearch()
    {
        return _character.Search(_character._player.transform, _character._detectionRange);
    }

    protected bool AttackSearch()
    {
        return _character.Search(_character._player.transform, _character._attackRange);
    }
}
