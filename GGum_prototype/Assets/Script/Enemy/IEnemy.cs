
public interface IEnemy : ICharacter
{
    int CurrentHP { get; set; }
    int MaxHP { get; }

    string GetID { get; }
    
    void Active(bool active);

}
