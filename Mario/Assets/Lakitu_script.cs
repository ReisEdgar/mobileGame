using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lakitu_script : EnemyAbstract_script
{
    public const int ThrowSpinnyEggTimer = 3;
    public const float NotMovingTimer = 0.2f; // Value of delay before starting movement around. Without it, it looks like
                                              // Lakitu bounces of the wall, when he stops chasing and starts moving around
    float throwSpinnyTimer;
     bool chasing; //Indicates that Lakitu is chasing Mario
     bool movingAround; // Idicates that Lakitu doesn't chase MArio and just moves around him
     float notMovingTimer; // Delay before moving around counter
    public float movingAroundPoint; // When moving around, Lakitu wont move further than 3 units for this point
     bool movingLeft; // Indicates that Lakitu moves left during movement around
     bool movingRight;// Indicates that Lakitu moves right during movement around
     float movingAroundPath; // Distance between transform and Lakitu's direction-change point when moving around
    bool chasingRight; // Lakitu chases Mario and moves to the right
    bool chasingLeft;// Lakitu chases Mario and moves to the left
    BoxCollider2D boxCollider;
    bool stop; // Indicates that lakitu won't move further and will just move aroud
    bool stopExecuted;
    Transform scoore;
    // Use this for initialization
    void Start()
    {

        scoore = transform.Find("Scoore");
        scooreScript = scoore.GetComponent<Scoore_script>();
        boxCollider = GetComponent<BoxCollider2D>();
        throwSpinnyTimer = ThrowSpinnyEggTimer;
        notMovingTimer = NotMovingTimer;
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        chasing = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;

        NormalizeLocalScale();

        if (transform.position.x - Camera_script.cameraPositionX < 15)
            if (!freezeMovement && ! GlobalFreezMovement && !stop)
        Movement();

        if(stop) // Lakitu stops following Mario
        {
            if(!stopExecuted)
            {
                movingAroundPoint = transform.position.x;
                stopExecuted = true;
                chasing = false;
                chasingLeft = false;
                chasingRight = false;
                EnemyRigidbody.velocity = Vector2.zero;
            }
            MovementAround();
        }

        if (flip)
            Flip(transform);

    }

    // If Mario doesn't move, Lakitu starts moving around him
    void MovementAround()
    {
        if (movingLeft)
            if (transform.position.x > movingAroundPoint - 3) //When moving around, Lakitu doesn't go further than 3 points from movingAroundPoint
            {
                // Depending on a distance from movingAroundPoint Lakitu moves at a different speed
                movingAroundPath = -(transform.position.x - movingAroundPoint - 3);

                MovementAroundSpeed(-1, movingAroundPath);
            }
            else // Lakitu starts moving to the other direction
            {
                movingLeft = false;
                movingRight = true;
                EnemyRigidbody.velocity = new Vector2(0, 0);
            }

        if (movingRight)
            if (transform.position.x < movingAroundPoint + 3)
            {
                movingAroundPath = transform.position.x - movingAroundPoint + 3;

                MovementAroundSpeed(1, movingAroundPath);
            }
            else
            {
                movingLeft = true;
                movingRight = false;
                EnemyRigidbody.velocity = new Vector2(0, 0);

            }
    }

    // Control of a Lakitu's speed when he moves around Mario
    void MovementAroundSpeed(int direction, float movingAroundPath)
    {
        if (movingAroundPath < 0.9f)
            EnemyRigidbody.velocity = new Vector2(1.5f * direction, 0);
        else if (movingAroundPath < 1.2)
            EnemyRigidbody.velocity = new Vector2(2.9f * direction, 0);
        else if (movingAroundPath < 2.6f)
            EnemyRigidbody.velocity = new Vector2(4.3f * direction, 0);
        else if (movingAroundPath < 3.2f)
            EnemyRigidbody.velocity = new Vector2(5.2f * direction, 0);
        else if (movingAroundPath < 4.7f)
            EnemyRigidbody.velocity = new Vector2(4 * direction, 0);
        else if (movingAroundPath < 5)
            EnemyRigidbody.velocity = new Vector2(3.8f * direction, 0);
        else if (movingAroundPath < 5.5f)
            EnemyRigidbody.velocity = new Vector2(2.5f * direction, 0);
        else
            EnemyRigidbody.velocity = new Vector2(1 * direction, 0);
    }

    // Chasing Mario at various speed to the right
    void ChasingtRight()
    {
        // If Mario is moving and if he is in front of (or really close to) a Lakitu, Lakitu stops movement around and starts chasing
        if (Mario_script.spriteDirection > 0 && Mario_script.moving)
            if (Mario_script.PositionX >= transform.position.x - 1)
            {
                chasingRight = true;
                chasing = true;
                movingRight = false;
                movingLeft = false;
                movingAround = false;
            }

        if (chasing)
        {

            if (Mario_script.PositionX > transform.position.x + 2) // If Mario is 3 points in front of a Lakitu, he doubles his speed
            {
                EnemyRigidbody.velocity = new Vector2(12, 0);
                notMovingTimer = NotMovingTimer;

            }

            else if (Mario_script.PositionX >= transform.position.x - 3)
            {
                EnemyRigidbody.velocity = new Vector2(6, 0);
                notMovingTimer = NotMovingTimer;

            }


            else   //Prepears Lakitu for movement around
            {
                notMovingTimer -= Time.deltaTime;
                EnemyRigidbody.velocity = new Vector2(0, 0);
                movingRight = true;

            }
        }


    }

    // Chasing Mario at a constant speed to the left
    void ChasingLeft()
    {
        if (Mario_script.InputDirection < 0)   // If Mario is moving and if he is behind Lakitu, Lakitu stops movement around and starts chasing
            if (Mario_script.PositionX < transform.position.x - 1)
            {
                chasingLeft = true;
                chasing = true;
                movingRight = false;
                movingLeft = false;
                movingAround = false;
            }

        if (chasing)
        {
            if (Mario_script.PositionX <= transform.position.x + 3) // Lakitu will chase Mario until he will be 3 points behind Mario
            {
                EnemyRigidbody.velocity = new Vector2(-2, 0);
                notMovingTimer = NotMovingTimer;
            }
            else   //Prepears Lakitu for movement around
            {
                notMovingTimer -= Time.deltaTime;
                EnemyRigidbody.velocity = new Vector2(0, 0);
                movingRight = true;

            }
        }

    }

    protected override void Movement()
    {

        if (!chasingLeft)
            ChasingtRight();

        if (!chasingRight)
            ChasingLeft();

        if (notMovingTimer < 0)
        {
            chasing = false;
            chasingRight = false;
            chasingLeft = false;
            notMovingTimer = NotMovingTimer;
            movingAround = true;
            movingAroundPoint = Mario_script.PositionX;

        }

        if (movingAround)
            MovementAround();
    }

    protected override void DisableColliders()
    {
        boxCollider.enabled = false;
    }

    protected override void Stomped()
    {
        Damaged = true;
        freezeMovement = true;
        SetScoore("1000");
        EnemyRigidbody.velocity = new Vector2(0, -10);

    }

    protected override void TouchedWithFireBall()
    {
        Damaged = true;
        SetScoore("800");
        flip = true;
    }

    protected override void TouchedByInvincibleMario()
    {
        TouchedWithFireBall();
    }

    // Changes local scale of game object and childs
    void NormalizeLocalScale()
    {
        if (EnemyRigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector2(1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(1, 1);
        }
        else if (EnemyRigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector2(-1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(-1, 1);
        }
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.tag == "ChangeDirection")
            stop = true;
    }
}
