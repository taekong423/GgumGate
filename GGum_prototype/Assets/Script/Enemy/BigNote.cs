using UnityEngine.UI;
using System.Collections;

public class BigNote : NewEnemy {

    public Image _hpBar;

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

}
