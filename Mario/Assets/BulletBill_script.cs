using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//----------DESCRIPTION------------
// Bullet Bill is an enemy, which moves thorugh everything in a single direction. Infinite amount of Bullet Bills are shoot from indestructable Bullet Blaster canon,
// although when Mario stands next to it, canon won't shoot. New Bullet bills are created by the Bullet Bill, which is parented to Bullet Blaster.
public class BulletBill_script : EnemyAbstract_script {

    const int DelayValue = 2; // Delay between shooting
    Rigidbody2D rigidBody;
     bool executed; // Prevents part of a code to be run more than once per certain time
    float delay;
    BoxCollider2D boxCollider;
    bool hasParent;
    // Use this for initialization
    void Start()
    {
        hasParent = transform.parent != null;
        boxCollider = GetComponent<BoxCollider2D>();
        delay = DelayValue;
        rigidBody = GetComponent<Rigidbody2D>();

        if (Mario_script.PositionX < transform.position.x)
        {
            movementDirection = -1;
            transform.localScale = new Vector2(1, 1);
            
        }
        else
        {
            movementDirection = 1;
            transform.localScale = new Vector2(-1, 1);
        }
            executed = false; // Clones of Bullet Bill are created with "executed" = TRUE
        scooreScript = GetComponentInChildren<Scoore_script>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;
        if(!GlobalFreezMovement && transform.position.x - Camera_script.cameraPositionX < 15)
        Movement();
    }

    protected override void DisableColliders()
    {
        boxCollider.enabled = false;
    }

    protected override void Movement()
    {
        if (!executed)
        {
            if (hasParent)
            {
                if (delay <= 0)
                {

                    if(Math.Abs(Mario_script.PositionX - transform.position.x)>2) // Instantiating new Bullet Bill
                        Instantiate(gameObject, transform.position, new Quaternion() );
                        delay = DelayValue;
                    
                }
                else
                    delay -= Time.deltaTime;
            }
            else
            {
                rigidBody.velocity = new Vector2(3 * movementDirection, 0);
                executed = true;
            }
        }
    }

    protected override void Stomped()
    {
        if (!hasParent) // Prevents Mario from killing parented Bullet Bill, which instantiates other Bullet Bills
        {
            Damaged = true;
            DisableColliders();
            Mario_script.shortJumpBool = true;
            SetScoore("200");
            rigidBody.velocity = Vector2.zero;
            rigidBody.gravityScale = 1;
        }
    }

    protected override void TouchedByInvincibleMario()
    {
        if (!hasParent) // Prevents Mario from killing parented Bullet Bill, which instantiates other Bullet Bills
        {
            Damaged = true;
            DisableColliders();
            SetScoore("100");
            rigidBody.velocity = Vector2.zero;  // Instead of doing flip, Bullet Bill simply falls down
            rigidBody.gravityScale = 1;
        }
    }

    protected override void TouchedWithFireBall()
    {
        TouchedByInvincibleMario();
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); 
    }
}
