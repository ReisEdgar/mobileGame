using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-----------DESCRIPTION-------------
// Trampoline will be throwing Mario up in the air continuously. If Mario jumps right before being thrown up, he will make very high jump.
//
public class Trampoline_script : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    EdgeCollider2D edgeCollider;
    public Vector2[] pointsArray; // Curently observable sprite's edges, used to reshape edge collider
    public float spriteTopPoint; // Allows to recognize, if trampoline is streched or compressed
    public bool start; // Indicates start of an animation
    BoxCollider2D boxCollider;
    bool executed; // PRevents some part of a code to be run more than once per certain time 
    public int counter; // Counts how many times trampoline was streched during one animation
    Animator animator;
    const float HighJumpTimer = 0.1f; // If after an animation, during this period of time user presses spacebar Mario makes high jump
    float highJumpTimer; // 
    bool startTimer; // Allows to start highJumpTimer
    //Use this for initialization
    void Start()
    {
        highJumpTimer = HighJumpTimer;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        pointsArray = new Vector2[4];

    }

    // Update is called once per frame
    void Update()
    {
        spriteTopPoint = spriteRenderer.sprite.bounds.size.y; // Geting vertical size of currently observable animation sprite

        pointsArray[0] = new Vector2(edgeCollider.points[0].x, edgeCollider.points[0].y);  // Creating array of edges of currently observable animation sprite
        pointsArray[1] = new Vector2(edgeCollider.points[1].x, spriteTopPoint);            // These points will be set to edge collider, so that it might
        pointsArray[2] = new Vector2(edgeCollider.points[2].x, spriteTopPoint);            // surround the sprite
        pointsArray[3] = new Vector2(edgeCollider.points[3].x, edgeCollider.points[3].y);

        if (start && spriteTopPoint > 1.4) //If animation started and trampoline is outstretched...
        {
            if (!executed)
            {
                counter++;  // ...program count how many times it reached this position, to know when to throw Mario up
                executed = true; // Prevents this part of a code to be run more than once at a time, when trampoline is streched
            }
        }

        if (start && spriteTopPoint < 1)
            executed = false; 


        if (counter == 2 && start)  // If during one animation, trampoline is in an outstreched position for a second time 
        {                                       //Reseting all values
            animator.SetBool("dynamic", false); 
            counter = 0;    
            executed = false;
            start = false;
            if (Mathf.Abs(Mario_script.PositionX - transform.position.x) < 0.7) // If Mario is still on a trampoline, he makes a jump
            {
                highJumpTimer = HighJumpTimer; 
                startTimer = true;
                Mario_script.lowTrampolineJump = true; 
            }
        }

        if (startTimer)  
            highJumpTimer -= Time.deltaTime;

        if (highJumpTimer > 0) // If user presses space until highJumpTimer hasn't reached 0, Mario makes jumps higer
        {
            if (MyButtons.SDown)
            {
                Mario_script.highTrampolineJump = true;

            }
        }
        else
            startTimer = false;

        boxCollider.offset = new Vector2(0, spriteTopPoint); // Changes position of boxCollider during animation
        
        edgeCollider.points = pointsArray; // Reshapes edge collider, so that it would suround sprite
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider is BoxCollider2D)
        {
            if (Mathf.Abs(collision.transform.position.x - transform.position.x) < 0.7)
            {
                start = true;
                animator.SetBool("dynamic", true);
            }

        }
    }

}