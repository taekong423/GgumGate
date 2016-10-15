using UnityEngine;
using System.Collections;

public class HpBar : MonoBehaviour {

    Player player;
    Animator hpAnimator;

    public float hpPercent;
    float lastHp;
	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        hpAnimator = GetComponentInChildren<Animator>();
        hpPercent = (player.currentHP / player.maxHP) * 100f;
        lastHp = player.currentHP;
    }
	
	// Update is called once per frame
	void Update () {
        if (IsHpChanged(player.currentHP))
        {
            hpPercent = ((float)player.currentHP / player.maxHP) * 100f;
            hpAnimator.SetFloat("Percent", hpPercent);
            lastHp = player.currentHP;
        }
	}

    bool IsHpChanged (int currHp)
    {
        if (currHp == lastHp)
            return false;
        else
            return true;
    }
}
