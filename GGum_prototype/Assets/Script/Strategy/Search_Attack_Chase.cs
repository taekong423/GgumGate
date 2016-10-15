using UnityEngine;
using System.Collections;

public class Search_Attack_Chase : Search_Chase_Attack
{

    public Search_Attack_Chase(AICharacter character) : base(character)
    {
    }

    public override void Operate()
    {

        if (!AttackSearch() && Search())
        {
            _character.SetStatePattern<ChaseState>();
        }
        else if(!AttackSearch() && !Search())
        {
            _character.SetStatePattern<MoveState>();
        }
    }

}
