using UnityEngine.UI;
using System.Collections;

public class BigNote : NewEnemy {

    IEnemy _owner;

    public Image _hpBar;

    public float _fallSpeed;
    
    protected override void Init()
    {
        base.Init();
    }

    protected override void SetStateList()
    {
        base.SetStateList();
    }

    protected override void HitEvent()
    {
        _hpBar.fillAmount = (float)CurrentHP / (float)MaxHP;
    }

    public void Setting(IEnemy owner, float fallSpeed = 0)
    {
        _owner = owner;
        if (fallSpeed != 0)
            _fallSpeed = fallSpeed;
    }


    class CreateState : State
    {
        readonly BigNote _note;

        public CreateState(BigNote note, string id) : base(id)
        {
            _note = note;
        }

        public override void Enter()
        {
            _note._rigidbody.gravityScale = 0;
        }

    }

}
