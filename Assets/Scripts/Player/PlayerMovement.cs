using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; ////How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Layers")]
    [SerializeField] private LayerMask upGroundLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps; //How many extra times you can jump, like double or triple jumps
    private int jumpCounter; //Keep track of how many extra jumps we have at the moment

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    //private float wallJumpCooldown;
    private float horizontalInput;


    // Start is called before the first frame update
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //If the char moves right
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        //If the char moves left, invert the sprite
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }

        //If the player is on the wall
        if (onWall())
        {
            //Make the player not slide off the wall
            //body.gravityScale = 0;
            //body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                jumpCounter = extraJumps; //Reset jump counter to extra jump value
            }
            else
            {
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
            }
        }

    }

    private void Jump()
    {
        //If coyote counter is 0 or less and not on the wall and don't have any extra jumps don't do anything
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
        {
            //WallJump();
        }
        else
        {
            if (isGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
            else
            {
                //If not on the ground and coyote counter bigger than 0 do a coyote jump
                if (coyoteCounter > 0)
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                }
                else
                {
                    if (jumpCounter > 0)
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }

                //Reset to avoid double coyote jumps
                coyoteCounter = 0;
            }
        }
    }

    private void WallJump()
    {
        body.AddForce
            (
                new Vector2
                (
                    -Mathf.Sign(transform.localScale.x) * wallJumpX,
                    wallJumpY
                )
            );
        //wallJumpCooldown = 0;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHitGround = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D raycastHitUpGround = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, upGroundLayer);

        return raycastHitGround.collider != null || raycastHitUpGround.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit =
            Physics2D.BoxCast
            (
                boxCollider.bounds.center,
                boxCollider.bounds.size,
                0,
                new Vector2(transform.localScale.x, 0),
                0.1f,
                wallLayer
            );
        return raycastHit.collider != null;
        
    }
}
