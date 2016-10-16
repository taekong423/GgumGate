using UnityEngine;
using System.Collections;
using System;

public class Search_Chase : Searchable {

    public Search_Chase(AICharacter character) : base(character)
    {

    }

    public override void Operate()
    {
        if (ChaseSearch())
        {
            _character.SetStatePattern<ChaseState>();
        }
    }

    protected bool ChaseSearch()
    {
        return _character.Search(_character._player.transform, _character._detectionRange);
    }
}
