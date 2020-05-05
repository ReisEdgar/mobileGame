using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  -----------DESCRIPTION--------------

/* 
 Koopa is an enemy who can appear in various forms:

 KoopaTroopa - moves horizontally, changes movement direction after hitting something, if jumped on - he hides in a shell which can be kicked and used as a projectile
 which will flip and kill other enemies (after a kick moving shell might damage Mario too).

 KoopaParaTroopa - jumping Koopa,  changes movement direction after hitting something, ignores collisions with Bricks and Question Blocks,
 if jumped on - turns into a normal KopaTroopa.

 RedKoopaTroopa - KoopaTroopa which moves between invisible movement bounds (never falls of the edge).

 RedKoopaParaTroopa - flies up an down between array of transofrms which change enemy's speed and movement direction. if jumped on - turns into
 normal KoopaTroopa and falls down.
     
     */


public class KoopaTroopa_Script : EnemyAbstract_script
{
    const float BackToLifeLenght = 1.85f; // Amout of time Koopa's BackToLife animation will last
    const float BackToLifeTime = 7.8f; // Amout of time Koopa has to spent in a shell unkicked in order to come back to life
    const float VerticalSpeed = 1.5f;
    const int GroundLayer = 8;
    const int BricksLayer = 10;
    const int QuestionBlockLayer = 11;
    BoxCollider2D boxCollider;
    Animator animator;
    bool shellSlide; // Bool is set to TRUE after Mario kicks Koopa hidden in his shell and allows shell to slide and kills all enemies on it's way. 
    float backToLifeTimer; // If Koopa hidden in the shell is not kicked after some time - he comes out of the shell and start walking again.
    public bool ParaTroopa;         // 
    public bool RedKoopaTroopa;     // These three bools turn KoopaTroopa into, KoopaParaTroopa, RedKoopaTroopa or RedKoopaParaTroopa. They must be set to TRUE in a scene view.
    bool inAShell; // Indicates that Koopa have been jumped on and has hidden in his shell.
    bool inShellExecuted; // Allows some part of the code to be run only once per certain time
    public int shellSlideCount; // Counts how much times Koopa's sliding shell have changed direction. Integer is used to allow Mario kick Koopa in shell and not get damaged                  
                         // after a contact with it. Ater shell changed direction at least once, it can damage Mario.

    float backToLifeLenght; //Lenght of an animation backToLife
    public EdgeCollider2D edgeCollider;
    AudioSource audioSource;
    public AudioClip kick;
    Scoore_script scooreScript2;
    public static int shellSlideFlips { get; set; }
    public bool BuzzBeatle; // Selects different set of animations and makes enemies immune to fire balls

    GameObject collidedWith; // Prevents OnCollisionEnter to be called twice during single collision
    Transform scoore; //  / Koopa has two scoores, because scoores is shown during stimping and kicking
    Transform scoore2;//  \ 

    bool executedScoore;
    SpriteRenderer spriteRenderer;
    public float timer; // Timer for ParaTroopa jump
    Vector2 fullBoxColliderSize = new Vector2(0.8421053f, 1.210526f); // Used to resize box collider when Koopa comes back to life after being stomped
    bool slowDown;// Indicates that Red koopa paratroopa needs to slow down before changing movement direction
    public EdgeCollider2D childCollider; // Edge collider of ParaTroopa, used to find out when he is grounded/
    Transform groundCheck; // Child object located under game obejct
    Vector2[] oldColliderPoints;
    Vector2[] newColliderPoints;

    // Use this for initialization
    // int movementDirection;
    void Start()
    {

        oldColliderPoints = new Vector2[4];
        newColliderPoints = new Vector2[4];

        oldColliderPoints[0] = new Vector2(0.47f, -0.4f);
        oldColliderPoints[1] = new Vector2(0.47f, 0.65f);
        oldColliderPoints[2] = new Vector2(-0.47f, 0.65f);
        oldColliderPoints[3] = new Vector2(-0.47f, -0.4f);

        newColliderPoints[0] = new Vector2(0.47f, -0.2f);
        newColliderPoints[1] = new Vector2(0.47f, 0.4f);
        newColliderPoints[2] = new Vector2(-0.47f, 0.4f);
        newColliderPoints[3] = new Vector2(-0.47f, -0.2f);        
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        SetParaTroopaTimer();
        movementDirection = -1;
        shellSlideCount = 0;
        scoore = transform.GetChild(1);
        scoore2 = transform.GetChild(2);
        audioSource = GetComponent<AudioSource>();
        backToLifeLenght = BackToLifeLenght;
        groundCheck = transform.Find("Ground check");
        edgeCollider = GetComponent<EdgeCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        shellSlide = false;
        backToLifeTimer = BackToLifeTime;

        if (ParaTroopa && !RedKoopaTroopa)
        {
            childCollider = groundCheck.GetComponent<EdgeCollider2D>();
            SetParaTroopaTimer();
        }
        scooreScript = transform.GetChild(1).GetComponent<Scoore_script>();
        scooreScript.parent = transform;
        scooreScript2 = transform.GetChild(2).GetComponent<Scoore_script>();
        scooreScript2.parent = transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2[] temp = spriteRenderer.sprite.vertices;
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (scene.Contains("OVERWORLD"))
            animator.SetBool("OverWorld", true);
        else if (scene.Contains("Castle"))
                animator.SetBool("Castle", true);

        
            EnemyRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    // Update is called once per frame
    void Update()
    {


        if (flip && !Damaged) // Flip was performed because Mario hit block under enemy
        {
            SetScoore("200");
            Damaged = true;
        }

        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;

        if (!freezeMovement && !GlobalFreezMovement)
            Movement();

        if (flip)
            Flip(transform);

        NormalizeScooreLocalScale();

        if (shellSlide && !executedScoore)
        {
            if (Mario_script.InAir)
                scooreScript2.SetScoore("500");
            else
                scooreScript2.SetScoore("400");

            executedScoore = true;
        }

        Animations();

        BackToLife();

    }

    // If Mario stomped Koopa but didin't kicked it for certain time, he will move out of shell and start walking normally again
    void BackToLife()
    {
        if (inAShell && !(shellSlide)) // Cheking how much time Koopa is hiding in a shell and not being kicked.
            backToLifeTimer -= Time.deltaTime;


        if (backToLifeTimer <= 0) // If certain amount of time have passed, Koopa goes out of the shell and acts the same as he did before he was stomped.
        {
            animator.SetBool("BackToLife", true);

            if (backToLifeLenght <= 0)

            {
                gameObject.tag = "Enemy";
                EnemyRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
                if (!BuzzBeatle)
                    boxCollider.size = fullBoxColliderSize;
                movementDirection = 1;
                backToLifeTimer = BackToLifeTime;
                if(!BuzzBeatle)
                edgeCollider.points = oldColliderPoints;

                Damaged = false;
                freezeMovement = false;
                shellSlide = false;
                inAShell = false;
                inShellExecuted = false;
                backToLifeLenght = BackToLifeLenght;
                EnemyRigidbody.gravityScale = 1;


            }
            else
                backToLifeLenght -= Time.deltaTime;
        }
    }

    // Sets timer for ParaTroopa's jump
    public void SetParaTroopaTimer()
    {
        timer = 0.3f;
    }

    // Changes local scale x of gameobject and it's chlids
    void NormalizeScooreLocalScale()
    {
        if (EnemyRigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(1, 1);
            if (scooreScript2.renderTimer <= 0)
                scoore2.localScale = new Vector2(1, 1);

        }
        else if (EnemyRigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(-1, 1);
            if (scooreScript2.renderTimer <= 0)
                scoore2.localScale = new Vector2(-1, 1);

        }
    }

    public new void CheckIfStuck()
    {
        if (EnemyRigidbody.velocity.x == 0 && EnemyRigidbody.velocity.y == 0)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0)
            {
                stuckTimer = ParaTroopa ? stuckTimer * 2 : stuckTimer;
                if (boxColliderTouch)
                {
                    boxColliderTouch = false;
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
                }
                else
                {
                    movementDirection = movementDirection * -1;
                }
            }
        }
        else
        {
            stuckTimer = stuckTimerConst;
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (RedKoopaTroopa && collision.gameObject.tag == "ChangeDirection" && !shellSlide)
        {
            if (!ParaTroopa)  // Normal red koopa troopa will change movement direction
                movementDirection = movementDirection * -1;
            else
                slowDown = true; // Red koopa para troopa will slow down before changing movement direction

        }
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {

        base.OnCollisionEnter2D(collision);
        if (ParaTroopa)
        {
            if (collision.gameObject.layer == BricksLayer || collision.gameObject.layer == QuestionBlockLayer)
                Physics2D.IgnoreCollision(boxCollider, collision.collider);
            if (collision.otherCollider.Equals(childCollider))
                SetParaTroopaTimer(); // Setting timer for another jump
        }
        if (collision.gameObject.tag == "ShellSlide")
            flipedByKoopa();

        if (!shellSlide)
        {
            if (collision.gameObject.tag != "Player" && collision.otherCollider is EdgeCollider2D)
            {
                  if (collision.otherCollider.Equals(edgeCollider))
                movementDirection = movementDirection * -1;

            }
        }
        else
        {
            if(collision.otherCollider is EdgeCollider2D)
            {
                if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "EdgeCollider")
                {
                    collidedWith = collision.gameObject;
                    shellSlideCount++;
                    movementDirection = movementDirection * -1;
                }
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            if (inAShell && !shellSlide)
            {
                gameObject.tag = "ShellSlide"; // Allows other gameobject recognize Koopa in a shell without getting it's script
                EnemyRigidbody.mass = 10000; // Allows Koopa to move through other enemies smoothly
                EnemyRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                if (collision.gameObject.GetComponent<Transform>().position.x > transform.position.x)
                    movementDirection = -1;
                else
                    movementDirection = 1;

                freezeMovement = false;
                shellSlide = true;
                audioSource.clip = kick;
                audioSource.Play();
            }
        }
        if (collision.gameObject.tag == "ShellSlide")
            if (collision.transform.position.x > transform.position.x)
                StartFlip(-1);
            else
                StartFlip(1);

        // If Brick is destroyed edge collider covering surface won't let enemy fall down. In this case collision with it is ignored
        if (collision.gameObject.tag == "EdgeCollider")
        {
            if (!(Physics2D.OverlapPoint(groundCheck.position, GroundLayer) || Physics2D.OverlapPoint(groundCheck.position, BricksLayer) || Physics2D.OverlapPoint(groundCheck.position, QuestionBlockLayer)))
            {
                Physics2D.IgnoreCollision(boxCollider, collision.collider);
                Physics2D.IgnoreCollision(edgeCollider, collision.collider);

            }

        }
    }

    //Each flip in a row will give Mario more pints
    void flipedByKoopa()
    {
        switch (shellSlideFlips)
        {
            case 0:
                {
                    SetScoore("400");
                    break;
                }
            case 1:
                {
                    SetScoore("800");
                    break;
                }
            case 2:
                {
                    SetScoore("1000");
                    break;
                }
            case 3:
                {
                    SetScoore("2000");
                    break;
                }
            case 4:
                {
                    SetScoore("4000");
                    break;
                }
            case 5:
                {
                    SetScoore("8000");
                    break;
                }
            case 6:
                {
                    SetScoore("1UP");
                    Mario_script.AddLive();
                    Mario_script.play1Up = true;
                    break;
                }
            default:
                {
                    SetScoore("8000");
                    break;
                }

        };
        animator.SetBool("Dead", true);
        shellSlideFlips++;
        flip = true;
    }

    void Animations()
    {
        if (!flip)
        {
            animator.SetBool("BuzzBeetle", BuzzBeatle);
            animator.SetInteger("MovementDirection", movementDirection);
            animator.SetBool("Dead", inAShell);
            animator.SetBool("RedKoopa", RedKoopaTroopa);
            animator.SetBool("ParaTroopa", ParaTroopa);
        }
    }

    void RedKoopaParaTroopaMovement()
    {
        if (!slowDown)
            EnemyRigidbody.velocity = new Vector2(0, VerticalSpeed * movementDirection); // Movement at full speed

        if (slowDown)  // Slows down, changes direction and then speeds up gameobject
        {
            if (movementDirection == 1)
            {

                EnemyRigidbody.velocity = new Vector2(0, EnemyRigidbody.velocity.y - 0.03f); // Koopa's velocity decreases until it's module reaches VerticalSpeed value
                if (Mathf.Abs(EnemyRigidbody.velocity.y) >= VerticalSpeed)
                {
                    slowDown = false;
                    movementDirection = -1;
                }
            }
            else
            {
                EnemyRigidbody.velocity = new Vector2(0, EnemyRigidbody.velocity.y + 0.03f);
                if (Mathf.Abs(EnemyRigidbody.velocity.y) >= VerticalSpeed)
                {
                    slowDown = false;
                    movementDirection = 1;
                }
            }
        }
    }

    protected override void Movement()
    {
        if (Mathf.Abs(Camera_script.cameraPositionX - transform.position.x) < 15)
        {
            CheckIfStuck();

            if (ParaTroopa)
            {
                if (RedKoopaTroopa)
                    RedKoopaParaTroopaMovement();
                else // Jumping up and down
                {
                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                        if (timer > 0.17f)
                            EnemyRigidbody.velocity = new Vector2(3 * movementDirection, 10);
                        else if (timer > 0.13f)
                            EnemyRigidbody.velocity = new Vector2(3 * movementDirection, 8);
                        else if (timer > 0.11f)
                            EnemyRigidbody.velocity = new Vector2(3 * movementDirection, 7);
                        else if (timer > 0.1f)
                            EnemyRigidbody.velocity = new Vector2(3 * movementDirection, 6);
                        else if (timer > 0.07f)
                            EnemyRigidbody.velocity = new Vector2(3 * movementDirection, 4);
                        else if (timer > 0.04f)
                            EnemyRigidbody.velocity = new Vector2(3 * movementDirection, 3);
                        else if (timer > 0.01f)
                            EnemyRigidbody.velocity = new Vector2(3 * movementDirection, 1);

                    }


                }
            }
            else
            {
                if (!shellSlide)
                    EnemyRigidbody.velocity = new Vector2(1.5f * movementDirection, -10);
                else
                    EnemyRigidbody.velocity = new Vector2(10 * movementDirection, -10);

            }

        }
    }

    protected override void DisableColliders()
    {
        boxCollider.enabled = false;
        edgeCollider.enabled = false;
        if (childCollider != null)
            childCollider.enabled = false;
    }

    protected override void Stomped()
    {
        if (!inAShell)
        {
    //Each stomp in a row will give Mario more pints

            switch (stompsInARow)
            {


                case 0:
                    {
                        SetScoore("200");
                        break;
                    }
                case 1:
                    {
                        SetScoore("400");
                        break;
                    }
                case 2:
                    {
                        SetScoore("800");
                        break;
                    }
                case 3:
                    {
                        SetScoore("1000");
                        break;
                    }
                default:
                    {
                        SetScoore("1000");
                        break;
                    }

            }

            Mario_script.shortJumpBool = true;

            if (ParaTroopa) // If Koopa was (Red)ParaTroopa, he turns to normal KoopaTroopa...
            {
                Damaged = false;
                ParaTroopa = false;
                EnemyRigidbody.gravityScale = 1;
                EnemyRigidbody.constraints = RigidbodyConstraints2D.None;
                EnemyRigidbody.freezeRotation = true;
                freezeMovement = false;
                childCollider.enabled = false;
            }
            else                              //... in other case he hides in a shell        
            {


                freezeMovement = true;
                gameObject.tag = "Untagged";
                EnemyRigidbody.velocity = Vector2.zero;
                if (!BuzzBeatle)
                { 
                    edgeCollider.points = newColliderPoints;
                boxCollider.size = new Vector2(0.8421053f, 0.7368421f);
            }
                movementDirection = 0;
                inShellExecuted = true;
                EnemyRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                animator.SetBool("BackToLife", false);
                Damaged = true;
                inAShell = true;
            }
        }

    }

    protected override void TouchedWithFireBall()
    {
        if (!BuzzBeatle)
        {
            Damaged = true;
            flip = true;
            animator.SetBool("Dead", true);
            freezeMovement = true;
            EnemyRigidbody.velocity = Vector2.zero;
            SetScoore("200");
        }
    }

    protected override void TouchedByInvincibleMario()
    {
        flip = true;
        animator.SetBool("Dead", true);
        freezeMovement = true;
        EnemyRigidbody.velocity = Vector2.zero;
        SetScoore("200");
    }

}


