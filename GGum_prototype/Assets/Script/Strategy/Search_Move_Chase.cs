using UnityEngine;
using System.Collections;

public class Search_Move_Chase : Search_Chase_Attack {

    public Search_Move_Chase(AICharacter character) : base(character)
    {
    }

    public override void Operate()
    {
        if (!AttackSearch() && ChaseSearch())
        {
            _character.SetStatePattern<ChaseState>();
        }
        else if (!AttackSearch() && !ChaseSearch())
        {
            _character.SetStatePattern<MoveState>();
        }
    }


}
