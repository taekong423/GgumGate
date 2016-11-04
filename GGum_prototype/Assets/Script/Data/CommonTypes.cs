using UnityEngine;
using System.Collections;


public enum Axis
{
    Horizontal,
    Vertical
}

public enum MoveType
{
    Once,
    PingPong,
    Random,
}

public enum ItemType
{
    Quest,
    Equipable,
    Consumable,
}

public enum EButtonCode
{
    None,
    MoveX,
    MoveY,
    Attack,
    Jump,
}

public enum EItemEffect
{
    Hp,
    MaxHp,
    Shield,
    AttackDamage,
    AttackSpeed,
    MoveSpeed,
}

public enum SpeechType
{
    Pet,
    Player,
}

[System.Serializable]
public struct SpeechData
{
    public SpeechType type;
    public string text;
}

public struct HitData
{
    public GameObject attacker;
    public int damage;

    public HitData(GameObject attacker = null, int damage = 0)
    {
        this.attacker = attacker;
        this.damage = damage;
    }
}

[System.Serializable]
public struct ItemData
{
    public int id;
    public string name;

    public ItemData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}

[System.Serializable]
public struct ItemEffect
{
    public EItemEffect effect;
    public float value;
}

[System.Serializable]
public struct CameraClamp
{
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
}