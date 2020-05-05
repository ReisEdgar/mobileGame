using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -------------DESCRIPTION----------
// Most common enemy in game. Walks horizontally and changes movement direction when he hits a wall.
public class Goomba_Script :EnemyAbstract_script {

    Animator GoombaAnimator;
    const float GoombaDeadCountValue = 0.85f; 
    float GoombaDeadCount; // Goomba will be shown squezed for a short time after being stomped
    bool executed; // Prevents some part of a code to be run more than once per certain time
    Transform groundCheck; // Child object located under game object
    const int GroundLayer = 8;
    const int BricksLayer = 10;
    const int QuestionBlockLayer = 11;
    BoxCollider2D boxCollider;
    EdgeCollider2D edgeCollider;

    // Use this for initialization
    void Start()
    {
        groundCheck = transform.Find("Ground check");
        movementDirection = -1;
        flipDirection = 1;
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        GoombaAnimator = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (scene.Contains("OVERWORLD"))
            GoombaAnimator.SetBool("OverWorld", true);
        else if (scene.Contains("CASTLE"))
                GoombaAnimator.SetBool("Castle", true);
        
        GoombaDeadCount = GoombaDeadCountValue;
        scooreScript = GetComponentInChildren<Scoore_script>();
        EnemyRigidbody.freezeRotation = true;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;

        if (flip && !Damaged) // Flip was performed because Mario hit block under enemy
        {
            SetScoore("200");
            Damaged = true;
        }

        if (flip)
            Flip(transform);

        // If Goomba dies there is a short period of time
        // to show animation of him being compressed, after that he disappears
        if (Damaged)
        {

            GoombaDeadCount -= Time.deltaTime;

            if (GoombaDeadCount < 0)
                Destroy(gameObject);
        }

        if (!freezeMovement && !GlobalFreezMovement)
        {
            Movement();
        }
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.tag != "Player" && collision.otherCollider is EdgeCollider2D)
            movementDirection = movementDirection * -1;
        else if (collision.gameObject.tag == "ShellSlide") // Koopa changes his tag to "ShellSlide" when performing shell slide
            flipedByKoopa();

        // If Brick is destroyed edge collider covering surface won't let enemy fall down. In this case collision with it is ignored
        if(collision.gameObject.tag == "EdgeCollider")
        {
            if (!(Physics2D.OverlapPoint(groundCheck.position,GroundLayer) || Physics2D.OverlapPoint(groundCheck.position, BricksLayer) || Physics2D.OverlapPoint(groundCheck.position, QuestionBlockLayer)))
            {
                Physics2D.IgnoreCollision(boxCollider, collision.collider);
                Physics2D.IgnoreCollision(edgeCollider, collision.collider);

            }
        }
    }

    protected override void Movement()
    {
        CheckIfStuck();
        if (transform.position.x - Camera_script.cameraPositionX < 15) 
        EnemyRigidbody.velocity = new Vector2(movementDirection * 1.5f, EnemyRigidbody.velocity.y);
    }

    protected override void DisableColliders()
    {
        boxCollider.enabled = false;
        edgeCollider.enabled = false;

    }

    // With each enemy flip performed by shell sliding Koopa, amount of points Mario receive increases
    void flipedByKoopa()
    {
        flip = true;
        switch (KoopaTroopa_Script.shellSlideFlips)
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
        KoopaTroopa_Script.shellSlideFlips++;

    }

    protected override void Stomped()
    {
        stompsInARow++;
        Damaged = true;
        EnemyRigidbody.velocity = Vector2.zero;
        DisableColliders();
        freezeMovement = true;
        GoombaAnimator.SetBool("Dead", Damaged);
        EnemyRigidbody.gravityScale = 0;
        Mario_script.shortJumpBool = true;

        switch (stompsInARow)
        {
           
            case 0:
                {
                    SetScoore("100");
                    break;
                }
            case 1:
                {
                    SetScoore("200");
                    break;
                }
            case 2:
                {
                    SetScoore("400");
                    break;
                }
            case 3:
                {
                    SetScoore("800");
                    break;
                }
            case 4:
                {
                    SetScoore("1000");
                    break;
                }
            default:
                {
                    SetScoore("1000");
                    break;
                }

        };

    }

    protected override void TouchedWithFireBall()
    {
        freezeMovement = true;
        SetScoore("100");
        flip = true;

    }

    protected override void TouchedByInvincibleMario()
    {
        TouchedWithFireBall();
    }



}
