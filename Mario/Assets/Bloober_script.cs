using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//------------------------------
 // Enemy which appears in SEA levels. Chasses Mario for a long time, but after some time, stops. 
 // Might be killed only with fireball or touched by Invincible Mario. 
 // 
//------------------------------

public class Bloober_script : EnemyAbstract_script {
     const float MovementTimer = 0.6f;
     const float DelayTimer = 0.7f;
    const float MovementDownTime = 0.3f;

    bool moveDown; // Allows MoveDown method to be called
    bool executed; // Prevents part of a code to be run more than once per certain time period
     bool movingUpRight; // Allows to perform diagonal movement right
     bool movingUpLeft; // Alows to perform diagonal movement left
     bool moveAround; // Allows Bloober to perform few diagonal movements up in a row, without moving down
     bool moveAroundExecuted; // Prevents part of a code to be run more than once per certain time period
    bool moveUpExecuted; // Prevents part of a code to be run more than once per certain time period
    int moveAroundDirection; // Direction of diagonal movement when performing random movements
    int GroundLayer; // Numeric value of Ground layer
    int EnemyLayer; // Numeric value of EnemyLayer
    BoxCollider2D boxCollider; 
     float delayTimer; // Delay between diagonal movements up
     float movementTimer; // Indicates how long diagonal movement up will last
     int moveAroundCount; // Indicates amount of random diagonal movements up Bloober will do before moving down, while Mario doesn't move
    float startPointX; // After moving 85units to the right from it's appearence point, Bloober stops chasing Mario and just moves around
    bool stopChasing; // Indicates that Bloober stoped chasing Mario
    Animator animator;
    float movementDownTimer; // Before Moving diagonally up, Bloober moves down for a short period of time
	// Use this for initialization
	  void Start () {
        scooreScript = GetComponentInChildren<Scoore_script>();
        animator = GetComponent<Animator>();
        startPointX = transform.position.x;
        GroundLayer = 8;
        EnemyLayer = 15;
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        moveDown = true;
        boxCollider = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Damaged)
        {
            freezeMovement = true;
            animator.SetBool("swim2", true);
            animator.SetBool("swim1", false);

        }

        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;
        if (!freezeMovement && transform.position.x - Mario_script.PositionX < 15 && !GlobalFreezMovement)
        Movement();

   
       
    }

    // If Mario doesn't move and Bloober is next to him, Bloober performs random movements
    // He will move up "moveAroundCount" times and then move down
    void MoveAround()
    {
        if (!executed)
        {
            if (Mario_script.PositionX > transform.position.x) // Choosing MoveUp direction
            {
                float randomValue = UnityEngine.Random.value; 
                if(randomValue < 0.3)    // Sometimes he might move up in a same direction more than once
                    moveAroundDirection = -1;
                moveAroundDirection = 1;
            }
            else
            {
                float randomValue = UnityEngine.Random.value;
                if (randomValue < 0.3)
                    moveAroundDirection = 1;
                moveAroundDirection = -1;
            }
                executed = true;
        }

        DiagonalMovementUp(moveAroundDirection);

        if (movementTimer <= 0) // if movement performed
        {
            moveAroundCount--;
            executed = false;  // Allows to choose moveAroundDirection again
            movementTimer = MovementTimer;
            EnemyRigidbody.velocity = Vector2.zero;
        }


        if (moveAroundCount == 0 )
        {
            movementTimer = 0;
            moveAround = false;
            executed = false;
            moveAroundExecuted = false;
            moveDown = true; // After performing required amount of random movements Bloober always move down
        }
        
    }

    void DiagonalMovementUp(int direction)
    {
        if (delayTimer <= 0 && !moveUpExecuted) // Delay between movements
        {

                movementDownTimer = MovementDownTime;
                if (direction > 0)
                    movingUpRight = true;
                else
                    movingUpLeft = true;
                delayTimer = DelayTimer;
                movementTimer = MovementTimer;
                moveUpExecuted = true;
                animator.SetBool("Swim1", false);
                animator.SetBool("Swim2", true);
            
        }
        else
        { 
            delayTimer -= Time.deltaTime;

        if (movementDownTimer > 0) // Short movement down
        {
                EnemyRigidbody.velocity = new Vector2(0, -1.5f);
            movementDownTimer -= Time.deltaTime;
        }
        else //  Diagonal movement up
        {
                animator.SetBool("Swim1", true);
                   animator.SetBool("Swim2", false);

                if (movementTimer > 0.52)
                    EnemyRigidbody.velocity = new Vector2(1 * direction, 1);
                else if (movementTimer > 0.48)
                    EnemyRigidbody.velocity = new Vector2(2 * direction, 2);
                else if (movementTimer > 0.4)
                    EnemyRigidbody.velocity = new Vector2(3 * direction, 3);
                else if (movementTimer > 0)
                    EnemyRigidbody.velocity = new Vector2(4 * direction, 4);
                else
                {
                    if (!moveAround) // When Bloober moves around he performs few diagonal movements before moving down
                        moveDown = true;
                    EnemyRigidbody.velocity = new Vector2(0, 0);

                    moveUpExecuted = false;

                    movingUpLeft = false;
                    movingUpRight = false;
                }
        }
    }
    }

    // When Mario is located lower than Bloober, Bloober moves straight down.
    void MovementDown()
    {
        if (!Mario_script.InAir)  
        {
            if ((Mario_script.PositionY >= transform.position.y - 2)) // If Mario is standing on something, Bloober won't move down lower than Mario's position Y + 2
            {
                moveDown = false;
                EnemyRigidbody.velocity = Vector2.zero;
            }
            else
            {
                EnemyRigidbody.velocity = new Vector2(0, -2);
                moveDown = true;
            }
        }
        else
        { 
            if ((transform.position.y > 0)||(Mario_script.PositionY >= transform.position.y + 4)) // If Mario swims, Bloober will move down
            {                                                                                          //  until his Y position won't reach Mario's Y position -4
                moveDown = false;                                                                      // or won't reach the ground
                EnemyRigidbody.velocity = Vector2.zero;
            }
            else
            { 
                
                EnemyRigidbody.velocity = new Vector2(0, -2);
                moveDown = true;
            }
        }
     
        }

    protected override void Movement()
    {

        if (transform.position.y > 8.7f) // Prevents Bloober from moving above water
        {
            EnemyRigidbody.velocity = new Vector2(EnemyRigidbody.velocity.x, 0);
            transform.position = new Vector2(transform.position.x, 8.7f);
        }
        if(stopChasing || transform.position.x > startPointX + 85) // After moving certain distances to the right from startPointX, 
        {                                                          //Bloober stops chasing Mario and moves around        
            stopChasing = true;
            moveAround = true;
            moveAroundCount = 1;
            movementTimer = MovementTimer;
            MoveAround();
        }

        if (Mario_script.moving)
            moveAround = false;

        if (!Mario_script.moving && Math.Abs(Mario_script.PositionX - transform.position.x) < 2 && !moveDown && !Mario_script.InAir) // Start of moving around
        {
            if (!moveAroundExecuted)
            {
                movementTimer = MovementTimer;
                moveAround = true;
                float randomValue = UnityEngine.Random.value;

                moveAroundExecuted = true;
                if (randomValue <= 0.45)
                    moveAroundCount = 1;
                else if (randomValue <= 0.8)
                    moveAroundCount = 2;
                else if (randomValue <= 1)
                    moveAroundCount = 3;
            }
            MoveAround();
        }
       else if (moveDown) // Each time after performing random amount of random movements up, Bloober moves down.
        {
            MovementDown();
        }

        else if (Mario_script.PositionX <= transform.position.x - 1 && !movingUpRight)
        {
            DiagonalMovementUp(-1);
        }
        else if (Mario_script.PositionX >= transform.position.x + 1 && !movingUpLeft)
        {
            if (Mario_script.PositionX - 10 >= transform.position.x)
                DiagonalMovementUp(2); // Bloober will chase MArio faster
            else
                DiagonalMovementUp(1);
        }
       // else 



        if (movementTimer <= 0)
        {
            movingUpLeft = false;
            movingUpRight = false;
        }
        //-----------------
        movementTimer -= Time.deltaTime;
        //-----------------    }
    }

    protected override void DisableColliders()
    {
        boxCollider.enabled = false;
    }

    protected override void Stomped()
    {
    }

    void CustomFlip()
    {
        Damaged = true;
        EnemyRigidbody.velocity = Vector2.zero;
        animator.SetBool("swim2", true);
        animator.SetBool("swim1", false);
        transform.localScale = new Vector2(1, -1);
        EnemyRigidbody.gravityScale = 0.2f;
    }

    protected override void TouchedWithFireBall()
    {
        if (!Damaged)
        {
            SetScoore("200");
            CustomFlip();
        }
    }

    protected override void TouchedByInvincibleMario()
    {
        TouchedWithFireBall();
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}


 