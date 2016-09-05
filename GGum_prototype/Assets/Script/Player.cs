using UnityEngine;
using System.Collections;

public enum PlayerState
{ Ledle,Ground}

public class Player : MonoBehaviour {

    private Rigidbody2D myRigidbody;

    private Animator myAnimator;
    public PlayerState Pst;
    public Transform bulletPos;
    private bool attack;
    public bool facingRight;
    [SerializeField]
    private float MoveSpeed = 0;
    [SerializeField]
    private Vector2 JumpVector;
   // private bool Jump;
    [SerializeField]
    private bool isGrounded;
    public Transform grounder;
    [SerializeField]
    private float radiuss;
    [SerializeField]
    private LayerMask Ground;
    public float gravityMultiplier = 3.5f;

    public Rigidbody2D rocket;              
    public float speed = 20f;

    private bool Jump;
    public bool isJump;
    public bool ladder;
    public bool JumpAttack;

    public GameObject Bullet;
    Playstates _state;
    // Use this for initialization
    void Start () {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        Jump = false;
        ladder = false;
    }
	
	// Update is called once per frame
	void Update () {
        IsgroundCheck();
    }

    void FixedUpdate(){
        handleLayers();
        if (!ladder)
        {
            float horizontal = Input.GetAxis("Horizontal");
            HandleMovement(horizontal);
            OnGravite();
            Flip(horizontal);
            
            HandleAttack();
            RestValues();
            InputHandle();
        }
        else
        {
            float vertical = Input.GetAxis("Vertical");
            LadderMovement(vertical);
            
           
        }
    }

    private void HandleMovement(float horizontal)
    {
      
        myRigidbody.velocity = new Vector2(horizontal * MoveSpeed, myRigidbody.velocity.y);
    
        if (Jump&&isGrounded)
        {
            myAnimator.SetTrigger("Jump");
            myAnimator.SetBool("Lend", true);
            myRigidbody.AddForce(JumpVector, ForceMode2D.Force);

        }
         
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

    }
    private void LadderMovement(float vertical)
    {
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, vertical*MoveSpeed);
    }

    private void InputHandle()
    {
        if(Input.GetKey (KeyCode.B))
        {
           Jump = true;

        }
        if(Input.GetKey(KeyCode.C))
        {
           
            if(isGrounded)
            { 
                attack = true;
            }
            else
            {
                JumpAttack = true;
            }
        }
    }


    private void HandleAttack(){
        if (attack&&isGrounded)
        {

            myAnimator.SetTrigger("Attack");
            myRigidbody.velocity = Vector2.zero;
           
        }

        if (!isGrounded && JumpAttack)
        {
            myAnimator.SetBool("JumpAttack", true);
        }
        if (!JumpAttack)
        {
            myAnimator.SetBool("JumpAttack", false);
        }
        /*  if (!isGrounded && attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
          {
              myAnimator.SetTrigger("attack");
              myRigidbody.velocity = Vector2.zero;
          }*/
    }

    private void RestValues()
    {
        attack = false;
        // Jump = false;
        JumpAttack = false;
    }

    void AttackFinsh()
    {
        if (!attack) { myAnimator.ResetTrigger("Attack"); }
       

        if (facingRight){ // ... instantiate the rocket facing right and set it's velocity to the right. 
            Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(speed, 0);
        }
        else{// Otherwise instantiate the rocket facing left and set it's velocity to the left.
            Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(-speed, 0);
        }
    }


    private void IsgroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(grounder.transform.position, radiuss, Ground);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(grounder.transform.position, radiuss);
    }

    void OnGravite()
    {
        if (!isGrounded) //basic gravity, only applied when we are not on the ground
        {
            myAnimator.ResetTrigger("Jump");
            myAnimator.SetBool("Lend",false);
            Jump = false;
            GetComponent<Rigidbody2D>().AddForce(Physics2D.gravity * gravityMultiplier,ForceMode2D.Impulse);

        }
    }

    void SendAnimMessage(string message)
    {
        SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }

    private void handleLayers()
    {
        if (ladder)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }


    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScal = transform.localScale;

            theScal.x *= -1;

            transform.localScale = theScal;
        }
    }

 
}
