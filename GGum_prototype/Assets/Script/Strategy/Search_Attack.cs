using UnityEngine;
using System.Collections;
using System;

public class Search_Attack : Searchable {

    public Search_Attack(AICharacter chracter) : base(chracter)
    {

    }

    public override void Operate()
    {
        if (AttackSearch())
        {
            _character.SetStatePattern<AttackState>();
        }
    }

    protected bool AttackSearch()
    {
        return _character.Search(_character._player.transform, _character._attackRange);
    }
}
