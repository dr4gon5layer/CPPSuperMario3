using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer marioSprite;

    public int jumpForce;
    public float speed;
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    public bool isShooting;

    public int score = 0;
    public int lives = 3;

    bool coroutineRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        marioSprite = GetComponent<SpriteRenderer>();

        if (speed <= 0)
        {
            speed = 5.0f;
        }

        if (jumpForce <= 0)
        {
            jumpForce = 345;
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;
        }

        if (!groundCheck)
        {
            Debug.Log("Groundcheck does not exist, please assign a ground check object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Checks to make sure the character is on the ground before swapping back to idle animation.
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        //Code to jump.
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.E))
        {
            if (isGrounded)
            {
                anim.SetTrigger("DoubleJump");
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * (jumpForce + 150));
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce);
            }
        }

        Vector2 moveDirection = new Vector2(horizontalInput * speed, rb.velocity.y);
        rb.velocity = moveDirection;

        anim.SetFloat("speed", Mathf.Abs(horizontalInput));
        anim.SetBool("isGrounded", isGrounded);

        if (marioSprite.flipX && horizontalInput > 0 || !marioSprite.flipX && horizontalInput < 0)
        {
            marioSprite.flipX = !marioSprite.flipX;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("isShooting", true);
        }
    }

    public void StartJChange()
    {
        if (!coroutineRunning)
        {
            StartCoroutine(JumpFChange());
        }
        else
        {
            StopCoroutine(JumpFChange());
            StartCoroutine(JumpFChange());
        }
    }

    IEnumerator JumpFChange()
    {
        coroutineRunning = true;
        jumpForce = 600;
        yield return new WaitForSeconds(10.0f);
        jumpForce = 345;
        coroutineRunning = false;
    }
}
