using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sprite;
  //  private Animator anim;


    [Header("Jumpable ground")]
    public LayerMask groundLayer;

    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float crawlSpeed;
    public float lowrunSpeed;

    [Header("Jumping")]
    public float idleJumpForce;
    public float walkJumpForce;
    public float runJumpForce;

    private float moveX;

    public float coyoteTime;
    private float coyoteTimeCounter;

    public float fallGravity;
    public float defaultGravity;

    public enum MovementState { laydown=-1,idle=0, walk=1, crawl=2, run=3, lowrun=4, jump=5, fall=6, death=7}
    private MovementState mState;
    private MovementState lastState;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
      //  anim = GetComponent<Animator>();

        coyoteTimeCounter = coyoteTime;

    }

    // Update is called once per frame
    void Update() {
        if (mState == MovementState.death)
            return;

        // jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (IsGrounded()) {
            coyoteTimeCounter = coyoteTime;
            rb.gravityScale = defaultGravity;
        }
        else
            if (coyoteTimeCounter > 0)
            coyoteTimeCounter -= Time.deltaTime;

        // fallcounter & fall gravity
        if (rb.velocity.y < -0.1f && !IsGrounded()) { 
            rb.gravityScale = fallGravity;
        }

        DoAnimations();
    }

    private void FixedUpdate() {
        if (mState == MovementState.death)
            return;

        moveX = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift)) { //run-lowrun  
            if (Input.GetKey(KeyCode.S)) { // lowrun
                rb.velocity = new Vector2(moveX * lowrunSpeed, rb.velocity.y);
                mState = MovementState.lowrun;
            }
            else { // run
                rb.velocity = new Vector2(moveX * runSpeed, rb.velocity.y);
                mState = MovementState.run;
            }
        }
        else { //walk-crawl
            if (Input.GetKey(KeyCode.S)) { //crawl
                rb.velocity = new Vector2(moveX * crawlSpeed, rb.velocity.y);
                mState = MovementState.crawl;
            }
            else { // walk
                rb.velocity = new Vector2(moveX * walkSpeed, rb.velocity.y);
                mState = MovementState.walk;
            }
        }

        if(moveX == 0) { // idel-laydown
            if (Input.GetKey(KeyCode.S))
                mState = MovementState.laydown;
            else
                mState = MovementState.idle;
        }


        // jumping
        if(rb.velocity.y > 0.1f && !IsGrounded())
            mState = MovementState.jump;

        // falling
        if (rb.velocity.y < -0.1f && !IsGrounded())
            mState = MovementState.fall;
    }

    private void Jump() {
        if (mState == MovementState.laydown || mState == MovementState.crawl || mState == MovementState.lowrun)
            return;

        if(coyoteTimeCounter <= 0)
            return;

        if (IsGrounded() || coyoteTimeCounter > 0) {
            if (mState == MovementState.idle)
                rb.velocity = new Vector2(rb.velocity.x, idleJumpForce);
            if (mState == MovementState.walk)
                rb.velocity = new Vector2(rb.velocity.x, walkJumpForce);
            if (mState == MovementState.run)
                rb.velocity = new Vector2(rb.velocity.x, runJumpForce);
        }

        coyoteTimeCounter = 0;
    }
   

    private void DoAnimations() {

        // left-right
        switch (moveX) {
            case < 0: sprite.flipX = true; break;
            case > 0: sprite.flipX = false; break;
        }

       /* if(mState != lastState) {
            anim.SetInteger("State", (int)mState);
            lastState = mState;
        }*/
    }


    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }


    public void SetState(MovementState ms) {
        lastState = mState;
        mState = ms;
    }

    public MovementState GetState() {
        return mState;
    }

    public void SmoothStop() {
        StartCoroutine(SmoothStop(0.5f));
    }

    private IEnumerator SmoothStop(float duration) {
        float initialSpeed = rb.velocity.x;
        float elapsedTime = 0f;
        float smoothX;

        while (elapsedTime < duration) {
            smoothX = Mathf.SmoothStep(initialSpeed, 0, elapsedTime/duration);
            rb.velocity = new Vector2(smoothX, rb.velocity.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        smoothX = 0f;
        rb.velocity = new Vector2(smoothX, rb.velocity.y);
    }

}
