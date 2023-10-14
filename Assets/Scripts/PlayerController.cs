using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections),typeof(Damageable))]
public class PlayerController : MonoBehaviour{

    public float walkSpeed = 4f;
    public float runSpeed = 9f;
    public float jumpImpulse = 12f;
    public float airWalkSpeed = 3f;
    public float airRunSpeed = 7f;
    
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public float CurrentMoveSpeed {
        get {
            if (CanMove) {
                if (isMoving && !touchingDirections.isOnWall) {
                    if (touchingDirections.isGrounded) {
                        if (IsRunning) {
                            return runSpeed;
                        }
                        else {
                            return walkSpeed;
                        }
                    }
                    else {
                        if (IsRunning) {
                            return airRunSpeed;
                        }
                        return airWalkSpeed;
                    }
                }
                else {
                    // idle speed is 0
                    return 0;
                }
            }else {
                // movement locked
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving { get { 
            return _isMoving;
        } private set { 
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        } 
    }

    [SerializeField]
    private bool _isRunning= false;
    public bool IsRunning { get {
            return _isRunning;
        } set { 
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    [SerializeField]
    public bool _isFacingRight = true;
    public bool IsFacingRight { get {
            return _isFacingRight;
        } private set { 
            if(_isFacingRight != value) {
                //Flip the local scale to make the player face the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        } 
    }

    public bool CanMove {
        get {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive {
        get {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }



    Rigidbody2D rb;
    Animator animator;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate() {

        if (!damageable.LockVelocity) {
            
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove (InputAction.CallbackContext context) {
        moveInput= context.ReadValue<Vector2>();

        if (IsAlive) {
            isMoving = moveInput != Vector2.zero;

            setFacingDirection(moveInput);
        }else {
            isMoving= false;
        }
    }

    private void setFacingDirection(Vector2 moveInput) {
        if(moveInput.x > 0 && !IsFacingRight) {
            //face the right
            IsFacingRight = true;
        }else if(moveInput.x < 0 && IsFacingRight) {
            //face the left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context) {
        if (context.started) {
            IsRunning = true;
        }else if (context.canceled) { 
            IsRunning=false;
        }
    }

    public void OnJump (InputAction.CallbackContext context) {
        // TODO Check if its alive after 
        if (context.started && touchingDirections.isGrounded && CanMove) {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if(context.started) {
            animator.SetTrigger(AnimationStrings.attackTrigger);  
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context) {
        if (context.started) {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnHit (int damage,Vector2 knockback) {

        rb.velocity = new Vector2(knockback.x,rb.velocity.y+knockback.y);
    }

}
