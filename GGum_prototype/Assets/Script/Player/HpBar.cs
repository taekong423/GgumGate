using UnityEngine;
using System.Collections;

public class HpBar : MonoBehaviour {

    [System.Serializable]
    public struct Clamp
    {
        public float min;
        public float max;
    }

    public RectTransform hpObject;
    [Range(0, 1)]
    public float hp;
    public Clamp yRange;
    public float yPos;

    private float lastHp;
    private Player player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //hp = ((float)player.currentHP / player.maxHP);
        //lastHp = player.currentHP;
    }
	
	// Update is called once per frame
	void Update () {
        //if (IsHpChanged(player.currentHP))
        {
            //hp = ((float)player.currentHP / player.maxHP);
            float y = yRange.max * hp;
            hpObject.anchoredPosition = new Vector2(0, y);
            //lastHp = player.currentHP;
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
