using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//----------DESCRIPTION-----------
// Same game object is used for all of the Moving platform types. Depending on a selected public bools, game objects performs specific behavior.
public class MovingPlatform_script : MonoBehaviour
{
    Rigidbody2D rigibody;
    public bool acelerate;
    public bool verticalOne; // Vertical movement in one direction. 
    public bool verticalTwo; // Vertical movement in two directions. 
    public bool changeDirection; // Changes default movement direction of single directional platforms
    int direction; // Movement direction
    public bool falling; // Platform starts falling once Mario stands on them, and stops once he jumps off.
    public bool horizontal; // Horizontal two directional movement
    float slowDownTimer; // Slow down before changing movement direction
    const float SlowDownTimer = 0.2f;
    bool executed; // Prevents some part of a code to be run more than once.
    GameObject movementBound; // Prevents same Movement bound from chaning platform's movement direction twice in a row
    // Use this for initialization
    void Start()
    {
        rigibody = GetComponent<Rigidbody2D>();
        if (!changeDirection)
            direction = 1;
        else
            direction = -1;

        if (horizontal)
        {
            rigibody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (verticalOne || verticalTwo || falling)
        {
            rigibody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        }

        if (falling)
            gameObject.tag = "Untagged"; // On collision Mario becomes child of moving platform in order to move together with it, although, when platform
    }                                    // is falling parenting is unwanted

    // Update is called once per frame
    void Update()
    {

        if (horizontal)
        {
            if (slowDownTimer > 0) // Slows down platform
            {

                if (slowDownTimer >= 0.1)
                    rigibody.velocity = new Vector2(1.3f * direction, 0);
                else
                    rigibody.velocity = new Vector2(0.6f * direction, 0);
                slowDownTimer -= Time.deltaTime;
                if (slowDownTimer <= 0)
                {
                    direction = direction * -1;
                    executed = false;
                }

            }
            else
                rigibody.velocity = new Vector2(2 * direction, 0);

        }

        else if (verticalOne) // Continuous movement of platforms in one direction
        {
            rigibody.velocity = new Vector2(0, 2 * direction); //  Moves vertically in manually set direction
            if (direction > 0)
            {
                if (transform.position.y >= 11)
                    transform.position = new Vector3(transform.position.x, -3, transform.position.z); // Platform appears at the bottom of the screen and continue to move up
            }
            else
            {
                if (transform.position.y <= -2.5f)
                    transform.position = new Vector3(transform.position.x, 11, transform.position.z);// Platform appears at the top of the screen and continue to move down
            }
        }
        else if (verticalTwo)  
        {
            if (slowDownTimer > 0)
            {

                if (slowDownTimer >= 0.1)
                    rigibody.velocity = new Vector2(0, 1.3f * direction);
                else
                    rigibody.velocity = new Vector2(0, 0.6f * direction);
                slowDownTimer -= Time.deltaTime;
                if (slowDownTimer <= 0)
                {
                    direction = direction * -1;
                    executed = false;
                }

            }
            else
                rigibody.velocity = new Vector2(0, 2 * direction);


        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!executed && (collision.gameObject == null || collision.gameObject != movementBound)) // Prevents same movement bound changing direction twice in a row
            if (collision.gameObject.tag == "ChangeDirection")
            {
                movementBound = collision.gameObject;
                slowDownTimer = SlowDownTimer; 
                executed = true;
            }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (falling && collision.gameObject.tag == "Player" && collision.transform.position.y > transform.position.y)
            rigibody.velocity = new Vector2(0, -4);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && falling)
            rigibody.velocity = Vector2.zero;

    }
}
