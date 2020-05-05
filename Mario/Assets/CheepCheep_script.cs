using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//--------------------------
/*
  CHARACTER DESCRIPTION
  
    Cheep Cheep appears in SEA level slowly swiming in one direction 
    or jumping up, trying to kill Mario in some OVERWORLD levels.
    Might be killed with fire ball or by touching Mario in an Invincible form

 */
//-------------------------
public class CheepCheep_script : EnemyAbstract_script {
    const float JumpTimer = 1.5f; // Amount of time jump up will last
    float jumpTimer; //Jump up time timer
    public bool underWater; // Indicates, that CheepCheep will swim in one direction
    bool executed; // Prevents some part of a code to be run more than once per certain time
    float randomNumber; // Used for choosing random delay before jump and random choice of horizontal velocity
    int horizontalVelocity; // Horizontal velocity of CheepCheep's jump up
    public bool gray; // Changes CheepCheep color to gray
    int direction; // Jump up direction
     bool jumpDelaySet;// Prevents some part of a code to be run mre than once per certain time 
     float jumpDelay; // After Mario gets close to CheepCheep(who is bellow camera's rendering field), he will jump before a randomly choosen delay
    Animator animator;
    Transform scoore;
  
    // Use this for initialization
    void Start () {
        scooreScript = GetComponentInChildren<Scoore_script>();
        scoore = transform.GetChild(0);
        animator = GetComponent<Animator>();
        animator.SetBool("gray", gray);
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        jumpTimer = JumpTimer;
        jumpDelay = 0;         // Each time CheepCheep instantiates his clone, jump delay...
        jumpDelaySet = false; // ... and timerExecuted are set to false, so that clone doesn't jump immediately with the same trajectory
	}

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -9)   // Destroys game object after a jump
            Destroy(gameObject);
        if(!freezeMovement && ! GlobalFreezMovement && transform.position.x - Camera_script.cameraPositionX < 15)
        Movement();

        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;

    }

    // If this method is not called, after changing localscale of gameobject, deparented scoore won't appear near CheepCheep
    void NormalizeLocalScale()
    {
        if (direction == 1)
        {
            transform.localScale = new Vector2(1, 1);
                scoore.localScale = new Vector2(1, 1);
        }
        else 
        {
            transform.localScale = new Vector2(-1, 1);
                scoore.localScale = new Vector2(-1, 1);
        }
    }

    protected override void Movement()
    {
        if (underWater)
            EnemyRigidbody.velocity = new Vector2(-1, 0);

        else
        {
            if (!jumpDelaySet && Camera_script.cameraPositionX > transform.position.x - 17) // Prevents distant and yet invisible CheepCheeps from jump 
            {

                randomNumber = UnityEngine.Random.value;
                if (randomNumber <= 0.2)      // Random delay before jump, so that they don't jump all at once and don't create a new clone on each frame
                    jumpDelay = 2;
                else if (randomNumber <= 0.4)
                    jumpDelay = 3;
                else if (randomNumber <= 0.6)
                    jumpDelay = 4;
                else if (randomNumber <= 0.8)
                    jumpDelay = 5;
                else
                    jumpDelay = 6;

                jumpDelaySet = true;

                if (Mario_script.PositionX < transform.position.x)
                    direction = -1;
                else
                    direction = 1;

                NormalizeLocalScale();

            }
            else if (jumpDelaySet)
            {
                if (jumpDelay < 0)
                {
                    if (!executed)
                    {
                        Instantiate(gameObject);  // Clone of a game object is created before each jump, for continuous generation of jumping CheepCheeps

                        randomNumber = UnityEngine.Random.value;
                        if (randomNumber <= 0.2)            // Setting of a random horizontal velocity
                            horizontalVelocity = 1;
                        else if (randomNumber <= 0.4)
                            horizontalVelocity = 2;
                        else if (randomNumber <= 0.6)
                            horizontalVelocity = 3;
                        else if (randomNumber <= 0.8)
                            horizontalVelocity = 4;
                        else
                            horizontalVelocity = 5;
                        executed = true;
                    }

                    if (jumpTimer >= 1.4)      // Control of a jump velocity
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 15);
                    else if (jumpTimer >= 1.1)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 12);
                    else if (jumpTimer >= 0.7)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 10);
                    else if (jumpTimer >= 0.55)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 7);
                    else if (jumpTimer >= 0.5)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 5);
                    else if (jumpTimer >= 0.45)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 4);
                    else if (jumpTimer >= 0.3)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 3);
                    else if (jumpTimer >= 0.15)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 2);
                    else if (jumpTimer >= 0)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, 1);
                    else if (jumpTimer >= -0.15)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -1);
                    else if (jumpTimer >= -0.3)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -2);
                    else if (jumpTimer >= -0.4)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -3);
                    else if (jumpTimer >= -0.45)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -4);
                    else if (jumpTimer >= -0.5)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -5);
                    else if (jumpTimer >= -0.55)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -7);
                    else if (jumpTimer >= -0.7)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -10);
                    else if (jumpTimer >= -1)
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -12);
                    else
                        EnemyRigidbody.velocity = new Vector2(horizontalVelocity * direction, -15);

                    jumpTimer -= Time.deltaTime;

                }

                else
                    jumpDelay -= Time.deltaTime;
            }
        }
    }

    protected override void DisableColliders()
    {
       
    }

    protected override void Stomped()
    {
    }
    //Cheep cheep doesn't use standart Flip() method
    void CustomFlip()
    {
        Damaged = true;
        EnemyRigidbody.velocity = Vector2.zero;
        animator.enabled = false;
        freezeMovement = true;
        transform.localScale = new Vector2(1, -1);

        if (SceneManager.GetActiveScene().name.Contains("SEA"))
            EnemyRigidbody.gravityScale = 0.2f;
        else
            EnemyRigidbody.gravityScale = 1;
    }

    protected override void TouchedWithFireBall()
    {
        if (!Damaged)
        {
            Damaged = true;
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
