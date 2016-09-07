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

    protected bool onGround;

    public Rigidbody2D m_rigidbody;
    public BoxCollider2D m_collider;
    public Transform attackBox;
    public GameObject effect;
    public GameObject bullet;
    public GameObject container;
    public State state;

    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    public int CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public int MaxShield { get { return maxShield; } set { maxShield = value; } }
    public int CurrentShield { get { return currentShield; } set { currentShield = value; } }
    public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
    public float AttackRange { get { return attackRange; } set { attackRange = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }



    public void Move(Axis axis, float keyValue)
    {
        if (axis == Axis.Horizontal)
        {
            transform.Translate(Vector2.right * keyValue * moveSpeed * Time.fixedDeltaTime);
        }
        else if (axis == Axis.Vertical)
        {
            transform.Translate(Vector2.up * keyValue * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Flip(float dir)
    {
        if (dir > 0)
            container.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir < 0)
            container.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void Jump()
    {
        if (m_rigidbody != null)
        {
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else
            Debug.Log("Null Rigidbody...");
    }

    public void Shoot()
    {
        Instantiate(bullet, attackBox.position, attackBox.rotation);
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
