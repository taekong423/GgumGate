using UnityEngine;
using System.Collections;

public class Search_Move_Attack : Search_Chase_Attack {

    public Search_Move_Attack(AICharacter character) : base(character)
    {
    }

    public override void Operate()
    {
        if (AttackSearch())
        {
            _character.SetStatePattern<AttackState>();
        }
        else if(!ChaseSearch())
        {
            _character.SetStatePattern<MoveState>();
        }
    }

}
