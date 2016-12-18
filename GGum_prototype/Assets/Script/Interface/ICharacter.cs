
namespace New
{
    public interface ICharacter
    {

        float CurrentHP { get; set; }
        float MaxHP { get; set; }

        //float Current

        bool IsDead { get; set; }

        HitResult OnHit(HitData hitdata);
    }
} 
