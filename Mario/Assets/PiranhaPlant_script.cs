using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------DESCRIPTION--------
// Piranha plants hide in pipes and periodically move up. Mario can't stomp them, but can kill them with fire balls or by touching while being in an invincible form

public class PiranhaPlant_script : EnemyAbstract_script {
    const float movementSpeed = 0.9f;
    const float Delay = 0.8f;
    float timer;
    SpriteRenderer spriteRenderer;
    float becomesVisible;
    float startPositionY;
    float finishPositionY;
    float startTimer;
    float finishTimer;
    bool changeDirection; //Changes direction of movement
    public bool hidden; // Indicates that Piranha plant is in pipe
    bool moving; // Indicates that Piranha plant moves
    float spriteHeight;
    bool moveDown;
    // Use this for initialization
    void Start () {
        Animator animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteHeight = spriteRenderer.sprite.bounds.size.y;
        finishPositionY = transform.position.y + spriteHeight;
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        startPositionY = transform.position.y;
        startTimer = finishTimer = 1;
        hidden = true;
        movementDirection = 1;
        timer = Delay;
        becomesVisible = transform.position.y + 0.1f;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("OVERWORLD"))
            animator.SetBool("Overworld", true);
        else
            animator.SetBool("Overworld", false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
        
    }

    //Moves up and down
    protected override void Movement()
    {
        if(moving)
        {
                if (!moveDown)
            {
                hidden = false;
                if (transform.position.y < finishPositionY)
                    EnemyRigidbody.velocity = new Vector2(0, movementSpeed);
                else
                {
                    EnemyRigidbody.velocity = Vector2.zero;

                    if (timer <= 0)
                    {
                        moveDown = true;
                        timer = Delay;
                    }
                    else
                        timer -= Time.deltaTime;
                }

            }
            else
            {
                if (transform.position.y > startPositionY)
                {
                    EnemyRigidbody.velocity = new Vector2(0, -movementSpeed);
                    hidden = false;
                }
                else
                {
                    EnemyRigidbody.velocity = Vector2.zero;
                    hidden = true;

                    if (timer <= 0)
                    {
                    moveDown = false;
                    hidden = true;
                        timer = Delay;
                    }
                    else
                        timer -= Time.deltaTime;
                }
            }
        }

        if (transform.position.y < becomesVisible && Mathf.Abs(Mario_script.PositionX - transform.position.x) < 1.5f)
            {
                moving = false;
                EnemyRigidbody.velocity = Vector2.zero;
            transform.position = new Vector2(transform.position.x, startPositionY);
            }
            else
                moving = true;
    }

    protected override void DisableColliders()
    {

    }

    protected override void Stomped()
    {
    }

    protected override void TouchedWithFireBall()
    {
        SetScoore("200");
        Destroy(gameObject);
    }

    protected override void TouchedByInvincibleMario()
    {
        TouchedWithFireBall();
    }


}
