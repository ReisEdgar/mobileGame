using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//------DESCRIPTION-------------
// Spinny egg's are thrown by Lakitu on the ground. Once the touch the ground, they start moving horiznotally like other enemies. Mario will be damaged, if he tries to
// stomp Spinny egg. Spinny egg is parented by Lakitu.

public class SpinyEgg_script : EnemyAbstract_script {
    const int ThrowTimerValue = 3; // Delay between each throw  
    const int EnableColliderValue = 1; // Delay between throw and colliders enableing. IT is used to prevent spinny egg from colliding with Lakitu
    const int GroundLayer = 8;
    const int BricksLayer = 10;
    const int QuestionBlockLayer = 11;
    float enableColliderTimer;
    float throwTimer;
     bool grounded;
    Animator animator;
    public BoxCollider2D boxCollider;
    public EdgeCollider2D edgeCollider;
    EnemyAbstract_script script;
     bool hasParent; // Indicates that Spinny egg always stays with Lakitu and that he will clone himself, in order to perform Spinny egg throw
    Transform scoore;
    SpriteRenderer spriteRenderer;
    Transform groundCheck; // Child object located under game object
   new bool enabled;
    public int ppp;
    public bool ttt;
    public bool qqq;
    public bool www;
    // Use this for initialization
    void Start()
    {
        groundCheck = transform.Find("Ground check");
        hasParent = transform.parent != null;
       enableColliderTimer = EnableColliderValue;
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        script = GetComponent<EnemyAbstract_script>();
        scoore = GetComponentInChildren<Transform>();

        boxCollider = GetComponent<BoxCollider2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        scooreScript = GetComponentInChildren<Scoore_script>();
        animator = GetComponent<Animator>();
        boxCollider.enabled = false;
        edgeCollider.enabled = false;

        if (hasParent)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            EnemyRigidbody.isKinematic = true;
            throwTimer = ThrowTimerValue;
        }
        else
        {
            float number = UnityEngine.Random.value;
            if (number > 0.5f) // Random throw direction
            movementDirection = -1;
        else
            movementDirection = 1;

            EnemyRigidbody.isKinematic = false;
            EnemyRigidbody.velocity = new Vector2(0, 5);
        }
    }

    // Update is called once per frame
    void Update() {
        ttt = grounded;
        ttt = freezeMovement;
        ttt = GlobalFreezMovement;
        if (flip && !Damaged) // Flip was performed because Mario hit block under enemy
        {
            SetScoore("200");
            Damaged = true;
        }

        if (GlobalFreezMovement)
            EnemyRigidbody.velocity = Vector2.zero;

        if (flip)
            Flip(transform);

        if(!GlobalFreezMovement)
        ThrowingSpinnyEggs();

        if (!enabled)
        {
            if (enableColliderTimer <= 0)
            {
                boxCollider.enabled = true;
                edgeCollider.enabled = true;
                enabled = true;
            }
            else
                enableColliderTimer -= Time.deltaTime;
        }

        NormalizeLocalScale();

        if (grounded/* && !freezeMovement && ! GlobalFreezMovement*/)
        Movement();
	}

    // Spinny egg parented by Lakitu constantly creates clones of himself which fall on the ground.
    void ThrowingSpinnyEggs()
    {
        if (hasParent)
        {
            if (!script.Damaged)
            {
                if (throwTimer <= 1)
                    spriteRenderer.sortingLayerName = "Frontal Objects"; // A moment before egg throw, egg appears in front of Lakitu
                if (throwTimer <= 0)
                {
                    Instantiate(gameObject, transform.position, new Quaternion());
                    spriteRenderer.sortingLayerName = "Behind Objects";
                    throwTimer = ThrowTimerValue;
                }
                else
                    throwTimer -= Time.deltaTime;
            }
            else
                Destroy(gameObject);
        }

    }

    void NormalizeLocalScale()
    {
        if (movementDirection < 0)
        {
            transform.localScale = new Vector2(1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(1, 1);
        }
        else if (movementDirection > 0)
        {
            transform.localScale = new Vector2(-1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(-1, 1);
        }
    }

    protected override void DisableColliders()
    {
        boxCollider.enabled = false;
        edgeCollider.enabled = false;
    }

    protected override void Movement()
    {
        CheckIfStuck();
        EnemyRigidbody.velocity = new Vector2(2*movementDirection, EnemyRigidbody.velocity.y);
    }

    protected override void Stomped()
    {
    }

    protected override void TouchedByInvincibleMario()
    {
        TouchedWithFireBall();
    }

    protected override void TouchedWithFireBall()
    {
        if (!hasParent)
        {
            Damaged = true;
            freezeMovement = true;
            flip = true;
            SetScoore("200");
        }
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (!grounded)
        {
            grounded = true;
            animator.SetBool("grounded", true);
        }
        if (collision.otherCollider is EdgeCollider2D && collision.gameObject.tag != "Player") // Each time Spinny egg bumps into something, he will change movement direction{
        {
            ppp++;
            movementDirection = movementDirection * -1;
        }
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

}