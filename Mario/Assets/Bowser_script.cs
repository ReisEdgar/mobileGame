using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//-------------DESCRIPTION-------------
// Bowser is boss which is located in the 4th level of each world. He is defeated either by throwing 5 fire balls (in this case he will perform a flip and turn into 
// one of other enemies of the game) or touch an Axe which is located behind him, in this case Bridge under Bowser will disappear and Bowser will fall into lava.
public class Bowser_script : EnemyAbstract_script
{
    const int Delay = 24; // Amout of frames during which Bowser will throw hammers
    const float MovementDelay = 0.2f;
    float randomValue;
    float randomNumber;
    float movementDurationTimer; // Indicates how long horizontal movement will last
    float movementDealy; // Delay between movements
    bool jump;
    bool executed; // Prevents some part of a code to be run more than once per certain time
    bool grounded;
    FireSpit_script fireScript;
    float timer;
    bool activated; // Indicates that Mario is close enough for Bowser to start spitting fire
    GameObject Bridge; // Brdige Bowser is standing on
    bool freezeMovementExecuted;// Prevents some part of a code to be run more than once per certain time
    public static int spriteDirection { get; private set; }
    Animator animator;
    public AudioClip BowserDeadAudio;
    AudioSource audioSource;
    public static bool BowserFellDown { get; set; }
    public static bool BowserDead { get; private set; }
    bool bowserDeadExecuted; // Prevents some part of a code to be run more than once per certain time
    bool bowserFellExecuted;// Prevents some part of a code to be run more than once per certain time
    public bool Hammer; // Indicates that Bowser throws hammers
    float throwTimer; // Delay between throwing sets of hammers. It is choosen randomlly.
    int delay; // Timer for making a delay between throwing single hammers
    Hammer_script script;
    int lives; // Bowser might be killed by throwing 5 fire balls at him.
    bool killedWithFireBalls;

    public string world;
    // Use this for initialization
    void Start()
    {
        lives = 5;
        delay = Delay;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        movementDirection = -1;
        fireScript = GetComponentInChildren<FireSpit_script>();
        Bridge = GameObject.Find("Bridge");
        script = GetComponentInChildren<Hammer_script>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -2 && !killedWithFireBalls) // Prevents level clear music from playing, before Mario hasn't touched an axe.
        {

            BowserFellDown = true;
            Destroy(gameObject);
        }
        if (Hammer)
            HammerThrow();

        NormalizeLocalScale();



        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;

        if (!activated && Mathf.Abs(Camera_script.cameraPositionX - transform.position.x) < 20)
            activated = true;

        if (!GlobalFreezMovement)
            Movement();

        if (activated && !freezeMovement && !GlobalFreezMovement)
            FireSpit();


        if (Bridge_script.activate)
        {
            Damaged = true;
            freezeMovement = true;
            if (killedWithFireBalls) // If bowser was killed with fire balls, he will be already under camera rendering point when MArio will touch an axe
            {
                BowserFellDown = true;
                Destroy(gameObject);

            }
        }

        if (freezeMovement && !freezeMovementExecuted) // When Bridge is activated, Bowser freezes in his current position
        {
            EnemyRigidbody.velocity = Vector2.zero;
            EnemyRigidbody.gravityScale = 0;
            freezeMovementExecuted = true;
            animator.SetBool("Falling", true);
            BowserDead = true;
        }

        if (Bridge == null) // Once Bridge completely disappeared
        {
            animator.SetBool("Falling", false); // Animation is played only while Bridge is compressing and Bwser id frozen mid-air
            if (!bowserDeadExecuted)
            {
                audioSource.clip = BowserDeadAudio;
                audioSource.Play();
                bowserDeadExecuted = true;
            }
            EnemyRigidbody.gravityScale = 1; // Faling down into fire
        }

    }

    protected override void Movement()
    {
        if (activated && !freezeMovement) // Delay between each movement
        {
            if (movementDealy >= 0)
                movementDealy -= Time.deltaTime;
            else
            {
                if (movementDurationTimer >= 0 && EnemyRigidbody.velocity.y == 0)
                {
                    MovementSelection(); // MEthod selects random movement once and then performs it.

                    movementDurationTimer -= Time.deltaTime;

                    if (movementDurationTimer < 0) // After horizontal movement was finished
                    {
                        randomValue = UnityEngine.Random.value;
                        if (randomValue <= 0.3) // Chosing between another horinotal movement or jump
                            SetRandomMovementTimer();
                        else
                            jump = true;

                        movementDealy = MovementDelay; // Short delay between movements
                        executed = false;
                    }

                }
                else if (jump)
                    Jump();

            }
        }
    }


    // Random selection of horizontal movement direction
    void MovementSelection()
    {

        if (!executed)
        {
            randomValue = UnityEngine.Random.value;
            executed = true;
        }

        if (randomValue <= 0.5)
            HorizontalMovement(movementDirection);
        else if (randomValue <= 0.9)
            HorizontalMovement(movementDirection * -1);
        else
            movementDirection = movementDirection * -1; // Skipping the movement



    }

    // Choosing random jump direction and horizontal velocity
    void Jump()
    {
        randomValue = UnityEngine.Random.value;
        if (randomValue <= 0.25)
            JumpUp(1 * movementDirection);
        else if (randomValue <= 0.55)
            JumpUp(0.5f * movementDirection);
        else if (randomValue <= 0.75)
            JumpUp(0.5f * movementDirection * -1);
        else if (randomValue <= 0.9)
            JumpUp(movementDirection * -1);
        else if (randomValue <= 1)
            movementDirection = movementDirection * -1;

        jump = false;
        SetRandomMovementTimer(); // After each jump Bowser always performs horizontal movement

    }

    // Fire spitting with random delays
    void FireSpit()
    {
        if (timer <= 0)
        {
            randomNumber = UnityEngine.Random.value;
            if (randomNumber <= 0.4)
                timer = 3f;
            else if (randomNumber <= 0.8)
                timer = 2.5f;
            else
                timer = 1.6f;
            audioSource.Play();
            fireScript.Fire = true;
        }
        else
            timer -= Time.deltaTime;

    }

    void NormalizeLocalScale()
    {

        if (Mario_script.PositionX > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            spriteDirection = -1;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            spriteDirection = 1;
        }

    }

    void HorizontalMovement(int direction)
    {
        EnemyRigidbody.velocity = new Vector2(direction, 0);

    }

    void JumpUp(float direction)
    {
        EnemyRigidbody.velocity = new Vector2(0.5f * direction, 5);
    }

    // Since 6 world, Bowser throws hammers in addition to spitting fire
    void HammerThrow()
    {
        if (throwTimer <= 0)
        {
            float randomThrowValue = UnityEngine.Random.value;
            if (delay == 24)   // Bowser throws few hammers in a row
                script.ThrowHammer();
            else if (delay == 20)
                script.ThrowHammer();
            else if (delay == 16)
                script.ThrowHammer();
            else if (delay == 12)
                script.ThrowHammer();
            else if (delay == 8)
                script.ThrowHammer();
            else if (delay == 4)
            {
                script.ThrowHammer();
                delay = 0;
            }

            delay--;

            if (delay <= 0)  // Random delay before another throw
            {
                if (randomThrowValue <= 0.2)
                    throwTimer = 0.8f;
                else if (randomThrowValue <= 0.5)
                    throwTimer = 1.9f;
                else if (randomThrowValue <= 0.75)
                    throwTimer = 2.4f;
                else
                    throwTimer = 2.9f;
                delay = Delay;
            }
        }
        else
            throwTimer -= Time.deltaTime;
    }

    // Selecting random amount of time for horizontal movement duration
    void SetRandomMovementTimer()
    {
        randomValue = UnityEngine.Random.value;
        if (randomValue <= 0.25f)
            movementDurationTimer = 0.1f;
        else if (randomValue <= 0.5f)
            movementDurationTimer = 0.2f;
        else if (randomValue <= 0.75f)
            movementDurationTimer = 0.3f;
        else
            movementDurationTimer = 0.4f;
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Movement bounds") // PRevents Bowser from moving to far in a single direction
            movementDirection = movementDirection * -1;
    }

    protected override void DisableColliders()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<EdgeCollider2D>().enabled = false;

    }

    protected override void Stomped()
    {
    }

    // Bowser can be killed by throwing 5 fire balls into him, depending on a level, he will flip and turn into certain enemy before falling into fire.
    protected override void TouchedWithFireBall()
    {
        lives--;
        
   if(lives <= 0)
        {
            Damaged = true;
         world = SceneManager.GetActiveScene().name.Substring(1, 1);

            if (world == "1")
                animator.SetBool("goomba", true);
            else if (world == "2")
                animator.SetBool("koopa", true);
            else if (world == "3")
                animator.SetBool("buzz", true);
            else if (world == "4")
                animator.SetBool("spinny", true);
            else if (world == "5")
                animator.SetBool("lakitu", true);
            else if (world == "6")
                animator.SetBool("bloober", true);
            else if (world == "7")
                animator.SetBool("hammerBro", true);
            else if (world == "8") // Final Bowser can't be killed with fire balls
                return;
            killedWithFireBalls = true;
            freezeMovement = true;
            freezeMovementExecuted = true;
            audioSource.clip = BowserDeadAudio;
            audioSource.Play();
            BowserDead = true;
            DisableColliders();
            transform.position = new Vector2(transform.position.x, transform.position.y + 3);
            GetComponent<SpriteRenderer>().flipY = true;

        }
   }

    protected override void TouchedByInvincibleMario()
    {
    }

    void OnLevelWasLoaded()
    {
        BowserDead = false;
        BowserFellDown = false;
    }
}
