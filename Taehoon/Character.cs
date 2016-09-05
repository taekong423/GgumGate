using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    [SerializeField]
    private int maxHP;
    private int currentHP;
    [SerializeField]
    private int maxShield;
    private int currentShield;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;


    public Transform attackPosition;
    public GameObject effect;
    public GameObject hitBox;
    public GameObject bullet;
    public State state;

    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    public int CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public int MaxShield { get { return maxShield; } set { maxShield = value; } }
    public int CurrentShield { get { return currentShield; } set { currentShield = value; } }
    public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
    public float AttackRange { get { return attackRange; } set { attackRange = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }



    public void MoveHorizontal(float axis)
    {
        transform.Translate(Vector2.right * axis * moveSpeed * Time.fixedDeltaTime);
    }

    public void MoveVertical(float axis)
    {
        transform.Translate(Vector2.up * axis * moveSpeed * Time.fixedDeltaTime);
    }


    public void OnHit(HitInfo hitInfo)
    {
        if (state != State.Dead)
        {
            int damage = hitInfo.damage;

            CalcDamage(ref currentShield, ref damage);

            if (damage > 0)
            {
                CalcDamage(ref currentHP, ref damage);
            }

            if (currentHP <= 0)
            {
                // When Dead
                state = State.Dead;
            }
        }
    }

    private void CalcDamage(ref int point, ref int damage)
    {
        if (point > 0)
        {
            if (point >= damage)
            {
                point -= damage;
                damage = 0;
            } 
            else
            {
                damage -= point;
                point = 0;
            }
        }
    }

    
}
