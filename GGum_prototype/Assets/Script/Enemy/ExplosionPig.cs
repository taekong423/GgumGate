using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public partial class ExplosionPig : NormalPig {

    public Text _countText;
    public GameObject _explosionPrefab;

    protected override void InitCharacter()
    {
        base.InitCharacter();

        _statePatternList.Add(typeof(ExplosionState), new ExplosionState(this));

        SetStatePattern<ExplosionState>();
    }

}
