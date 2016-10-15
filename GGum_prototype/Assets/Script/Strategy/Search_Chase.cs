using UnityEngine;
using System.Collections;
using System;

public class Search_Chase : Searchable {

    public Search_Chase(AICharacter character) : base(character)
    {

    }

    public override void Operate()
    {
        Debug.Log("SearchChase");
        if (Search())
        {
            _character.SetStatePattern<ChaseState>();
        }
    }

    protected bool Search()
    {
        return _character.Search(_character._player.transform, _character._detectionRange);
    }
}
