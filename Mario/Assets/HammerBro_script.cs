using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------DESCRIPTION---------------
// Hammer Bros are enemies, which throw infinite amount of hammers, which kill Mario on touch. They jump between two levels of Bricks and make set of horizontal movements
// after each jump. If Mario hasn't defeated or moved past them in 20seconds, they start chasing him.

public class HammerBro_script : EnemyAbstract_script {
    const float Level1Height = 4.3f; 
    const float Level2Height = 8.2f;
    const int noLevelHeight = 5;
    const int ChaseTimerValue = 20;
    const int GroundLayer = 8;
    const int BricksLayer = 10;
    const int QuestionBlockLayer = 11;
    public int spriteDirection { get; set; }
     float delay; // Random delay before making a jump, after changing horizontal movement direction 3 times
    Hammer_script script; // Script of a Hammer which Hammer Bro throws
    public bool jumpDown; // Indicates that character is jumping down
    BoxCollider2D boxCollider;
    public bool jumpUp; // Indicates that character is jumping up
    public int movementCount;  // Amount of times character changed his horizontal movement direction on a same level.
    bool jumpDownExecuted; // Prevents some part of a code to be run more than once per certain time
    bool jumpUpExecuted; // Prevents some part of a code to be run more than once per certain time
    public bool movingHorizontally; // Indicates that character is moving horizntally
     bool setDelay; 
    float throwTimer;
    public bool toLevel0; // /
    public bool toLevel1;// |  Specifies to which level Hammer Bro is jumping
   public bool toLevel2;//   \
    float chaseTimer;
    public bool oneLeveled; // Hammer bro will be jumping on;y on a single level
    public float PositionX { get; set; }
    public float PositionY { get; set; }
   Transform scoore;
    Animator animator;
    public bool noLevel; // Hammer bro will jump up and down on a groud
    public bool singleLevel; // Hammer bro will jump only on a single level
    Transform groundCheck; // Child object located under game object

    // Use this for initialization
    void Start()
    {
        groundCheck = transform.Find("Ground check");
        scooreScript = GetComponentInChildren<Scoore_script>();
        scoore = transform.Find("Scoore");
        movingHorizontally = true;
        setDelay = true;
        boxCollider = GetComponent<BoxCollider2D>();
        script = GetComponentInChildren<Hammer_script>();
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        movementDirection = 1;
        chaseTimer = ChaseTimerValue;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;

        Animations();

        if(flip && !Damaged) // Flip was performed because Mario hit block under enemy
        {
            SetScoore("1000");
            Damaged = true;
        }

        if (flip)
        {
            Flip(transform);
        }
        

        if (chaseTimer < 0)
            movementDirection = spriteDirection;

        if (!freezeMovement && !GlobalFreezMovement && transform.position.x - Camera_script.cameraPositionX < 15)
            Movement();

        PositionX = transform.position.x;  
        PositionY = transform.position.y;

        NormalizeScooreLocalScale();

        HammerThrow();
    }

    // Depending on condicitions method starts jump up or down
    void JumpingUpAndDown()
    {
        if (!movingHorizontally && chaseTimer > 0)
        {
            if (!jumpDown && transform.position.y < 0.2) // If character stands on a ground
            {
                toLevel1 = true;
                jumpUp = true;
            }
            // If character stands on a level1, and not performing any movements, he can perform either jump up or down
            else if (!jumpUp && !jumpDown && transform.position.y < Level1Height)
            {
                float randomValue = UnityEngine.Random.value; //Generated random value defines if character will jump up or down

                if (singleLevel)
                    randomValue = 0;

                if (randomValue <= 0.6)           
                {
                    toLevel0 = true;
                    jumpDown = true;
                }
                else
                {
                    jumpUp = true;
                    toLevel2 = true;
                }

            }
            // If character stands on a level2, he jumps down
            else if (!jumpUp && !jumpDown)
            {
                toLevel1 = true;
                jumpDown = true;
            }
            
        }
    }

    // Method makes character jump up
    void JumpUp()
    {
        EnemyRigidbody.velocity = new Vector2(0, 9);   

        if (!jumpUpExecuted)
            boxCollider.isTrigger = true;

        if((toLevel1 && transform.position.y > Level1Height) ||(toLevel2 && transform.position.y > Level2Height) || (noLevel && transform.position.y > noLevelHeight))
        {
            boxCollider.isTrigger = false;            
            setDelay = true;       
            movingHorizontally = true;  
            jumpUp = false;    
            toLevel1 = false;
            toLevel2 = false;
            jumpUpExecuted = false;
        }
    }

    //Method makes character jump on level down. Method is called once per jump 
    void JumpDown()
    {
        if (!jumpDownExecuted)
        {
            boxCollider.isTrigger = true;
            EnemyRigidbody.velocity = new Vector2(0, 5); //Chareacter makes small jump up before falling down
            jumpDownExecuted = true;
        }
        if((toLevel0 && transform.position.y < 0.5) || (toLevel1 && transform.position.y < 4.7))// When jump is finished
        {            boxCollider.isTrigger = false;

            setDelay = true;           
            movingHorizontally = true;  
            jumpDown = false;              
            toLevel0 = false;
            toLevel1 = false;
            jumpDownExecuted = false;
        }

    }

    // Method allows character to move horizontally between game objects "Movement bounds"
    void HorizontalMovement()
    {
        if (setDelay)  
        {
            float randomDelay = UnityEngine.Random.value; // Character will change movement direction 3 times.
            if (randomDelay <= 0.25)          // After that, depending on a randomDelay value, he will jump up or down
                delay = 0.2f;                 // after random period of time set here.
            else if (randomDelay <= 0.5f)
                delay = 0.4f;
            else if (randomDelay <= 0.75f)
                delay = 0.6f;
            else
                delay = 0.8f;
            setDelay = false;
        }

        EnemyRigidbody.velocity = new Vector2(2.2f * movementDirection, EnemyRigidbody.velocity.y); // Horizontal movement velocity

        if (movementCount >= 3)   // After changing direction three times, count down starts
            delay -= Time.deltaTime;

        if (movementCount == 3 && delay <= 0) 
        {
            EnemyRigidbody.velocity = Vector2.zero;
            jumpDownExecuted = false;     
            movingHorizontally = false;  
            movementCount = 0;
        }

    }

    void Animations()
    {
        animator.SetBool("Throw", script.startTimer);
    }

    void HammerThrow()
    {
        if(!script.startTimer)
        if (throwTimer <= 0)
        {
          float  randomThrowValue = UnityEngine.Random.value;
            script.ThrowHammer();
            if (randomThrowValue <= 0.2)
                throwTimer = 0.5f;
            else if (randomThrowValue <= 0.5)
                throwTimer = 1.5f;
            else if (randomThrowValue <= 0.75)
                throwTimer = 1.9f;
            else
                throwTimer = 2.4f;
        }
        else
            throwTimer -= Time.deltaTime;
    }

  // Changes local scale x of game object and child
    void NormalizeScooreLocalScale()
    {
        if (Mario_script.PositionX < transform.position.x - 1)
        {
            spriteDirection = -1;
            transform.localScale = new Vector2(1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(1, 1);
        }
        else if (Mario_script.PositionX > transform.position.x + 1)
        {
            spriteDirection = 1;
            transform.localScale = new Vector2(-1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(-1, 1);
        }
    }



    protected override void Movement()
    {
        chaseTimer -= Time.deltaTime;
        CheckIfStuck();
        // Method checks which jump (up or down) character need to perform 
        JumpingUpAndDown();

        if (jumpUp)
            JumpUp();
        if (jumpDown)
            JumpDown();

        // Allows character to move horizontally between game objects "Movement bounds". This method is called after each jump.
        if (movingHorizontally)
            HorizontalMovement();
    }

    protected override void DisableColliders()
    {
        boxCollider.enabled = false;
    }

    protected override void Stomped()
    {
        Mario_script.shortJumpBool = true;
        Damaged = true;

        SetScoore("5000");

        flipDirection = 1;
        flip = true;
    }

    protected override void TouchedWithFireBall()
    {
        SetScoore("1000");
        Damaged = true;
        flip = true;
    }

    protected override void TouchedByInvincibleMario()
    {
        TouchedWithFireBall();
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Enemy") // Hammer Bros ignore collisions when they are jumping
        {
            if (jumpDown || jumpUp)
            {
                Physics2D.IgnoreCollision(boxCollider, collision.collider, true);
            }
            else // Or just change movement direction, if they are moving horizontally
            {
                movementCount++;
                movementDirection = movementDirection * -1;
            }
                    }

        // If Brick is destroyed edge collider covering surface won't let enemy fall down. In this case collision with it is ignored
        if (collision.gameObject.tag == "EdgeCollider")
        {
            if (!(Physics2D.OverlapPoint(groundCheck.position, GroundLayer) || Physics2D.OverlapPoint(groundCheck.position, BricksLayer) || Physics2D.OverlapPoint(groundCheck.position, QuestionBlockLayer)))
            {
                Physics2D.IgnoreCollision(boxCollider, collision.collider);

            }
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ChangeDirection" && chaseTimer > 0)    // While making horizontal movements character moves between two gameobjects called "Movement bounds".
        {
            movementCount++;

            movementDirection = movementDirection * -1;
        }
    } 

}
