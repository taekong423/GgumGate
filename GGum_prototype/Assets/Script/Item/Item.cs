using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour {

    public ItemType type;
    public ItemData itemData;
    public List<ItemEffect> effectList;

    private Player player;
    private GameObject effect;


    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }
    }
}
