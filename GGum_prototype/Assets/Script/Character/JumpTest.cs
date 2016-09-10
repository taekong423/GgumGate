using UnityEngine;
using System.Collections;

public class JumpTest : MonoBehaviour
{

    public float jumpForce = 0f;
    private Rigidbody2D rbody2D;
    public LayerMask layerMask;
    public bool grounded = false;
    public GameObject groundCheckObject;
    public float groundCheckDistance = 0f;
    public bool canMultipleJump = false;
    private int jumpCounter = 0;
    public int maxJumps = 0;

    // Use this for initialization 
    void Start () {
        rbody2D = GetComponent<Rigidbody2D>();
        jumpCounter = 1;
    }

    // Update is called once per frame 
    void Update() {
        RaycastHit2D hit01 = Physics2D.Raycast(groundCheckObject.transform.position, Vector2.down, groundCheckDistance, layerMask);
        grounded = (hit01.collider != null);

        if (canMultipleJump)
        {
            MultipleJump();
        }
        else
        {
            SingleJump();
        }
    }

    void SingleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rbody2D.AddForce(Vector2.up * jumpForce);
        }
    }

    void MultipleJump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (jumpCounter < maxJumps) {
                rbody2D.AddForce(Vector2.up * jumpForce); jumpCounter++;
            }
        }

        if (grounded) { jumpCounter = 1; }
    }
}