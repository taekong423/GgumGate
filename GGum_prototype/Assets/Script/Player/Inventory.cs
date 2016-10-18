using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    private Player player;

    public Dictionary<ItemData, int> questItems;
    public Dictionary<ItemData, List<ItemEffect>> equipableItems;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
        questItems = new Dictionary<ItemData, int>();
        equipableItems = new Dictionary<ItemData, List<ItemEffect>>();
	}
	
	// Update is called once per frame
	void Update () {
        //ApplyEquipable();
	}

    void ApplyEquipable()
    {
        int maxHpItem = 0;
        int attackDamageItem = 0;
        float attackSpeedItem = 0;
        float moveSpeedItem = 0;

        foreach (List<ItemEffect> list in equipableItems.Values)
        {
            foreach (ItemEffect ie in list)
            {
                switch (ie.effect)
                {
                    case EItemEffect.Hp:
                        break;
                    case EItemEffect.MaxHp:
                        maxHpItem += (int)ie.value;
                        break;
                    case EItemEffect.Shield:
                        break;
                    case EItemEffect.AttackDamage:
                        attackDamageItem += (int)ie.value;
                        break;
                    case EItemEffect.AttackSpeed:
                        attackSpeedItem += ie.value;
                        break;
                    case EItemEffect.MoveSpeed:
                        moveSpeedItem += ie.value;
                        break;
                }
            }
        }
    }

    void ApplyConsumable(List<ItemEffect> list)
    {
        foreach(ItemEffect ie in list)
        {
            switch (ie.effect)
            {
                case EItemEffect.Hp:
                    player.currentHP += (int)ie.value;
                    break;
                case EItemEffect.MaxHp:
                    player.maxHP += (int)ie.value;
                    break;
                case EItemEffect.Shield:
                    player.shield += (int)ie.value;
                    break;
                case EItemEffect.AttackDamage:
                    player.attackDamage += (int)ie.value;
                    break;
                case EItemEffect.AttackSpeed:
                    player.attackSpeed += ie.value;
                    break;
                case EItemEffect.MoveSpeed:
                    player.moveSpeed += ie.value;
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item.type == ItemType.Quest)
            {
                if (questItems.ContainsKey(item.itemData))
                {
                    questItems[item.itemData]++;
                }
                else
                {
                    questItems.Add(item.itemData, 1);
                }

                Destroy(other.gameObject);
            }
            else if (item.type == ItemType.Equipable)
            {
                if (!equipableItems.ContainsKey(item.itemData))
                {
                    equipableItems.Add(item.itemData, item.effectList);
                    Destroy(other.gameObject);
                }
            }
            else if (item.type == ItemType.Consumable)
            {
                ApplyConsumable(item.effectList);
                Destroy(other.gameObject);
            }
        }
    }
}
