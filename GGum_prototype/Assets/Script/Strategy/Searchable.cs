using UnityEngine;

public abstract class Searchable {

    protected AICharacter _character;

    public Searchable(AICharacter character = null)
    {
        _character = character;
    }

    public abstract void Operate();

}
