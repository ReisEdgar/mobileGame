using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 -------------------
  DESCRIPTION
  ------------------
  This script is added to both Bricks and Question blocks and allows them to move up and down a little after Mario htis them with his head. Also this script allows
  Bricks to be destroyed after Mario hits them with his head while being in Super/Fire Mario form.
 */
public class BrickScript : MonoBehaviour
{
    Vector3 destination; // Top Y point of brick oscillation
    float startPositionY; // Bottom Y point of Brick oscilation( Y point of Brick in static state)
    int delay; // Causes brick to make a short pause at the top Y point during oscillation.
    bool destroyBrick;
    Rigidbody2D[] SmallBricks; // When Brick is destroied by Super Mario, he turns into 4 small bricks 
    AudioSource audioSource;
    bool executed; // Prevents some part of a code to be run more than once
    float destroyTimer; // Delay between crushing a brick and destruction of game object
   public bool oscillating { get; set; }
    int destroctionDealy; // Short delay before crushing brick into four small bricks
    // Use this for initialization

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        destroctionDealy = 3;
        SmallBricks = GetComponentsInChildren<Rigidbody2D>();
        startPositionY = transform.position.y;
        destination = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    void FixedUpdate()
    {

        // Script start working from OnCollisionEnter2D

        if (transform.position.y + 0.5f > startPositionY) // If Brick reached his top point during oscillation
        {                                                 // he makes a short delay and then moves back down.
            if (delay == 1)
            {

                transform.position = Vector3.MoveTowards(transform.position, destination, -0.2f);

            }
            delay--;
        }

        if(transform.position.y < startPositionY)
        {
            transform.position = new Vector3(transform.position.x, startPositionY, transform.position.z);
        }

        if (destroyBrick)
            DestroyBrick();

        if (destroyTimer > 0)
        {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0)
                Destroy(gameObject);
        }

        if (transform.position.y == startPositionY)
            oscillating = false;
    }

    // Turning brick into four small bricks
    void DestroyBrick()

    {
        if (destroctionDealy > 0)
            destroctionDealy--;
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            for (int i = 1; i < 5; i++)
            {
                SmallBricks[i].isKinematic = false;
            }

            SmallBricks[1].velocity = new Vector2(3, 8);
            SmallBricks[2].velocity = new Vector2(-3, -2);
            SmallBricks[3].velocity = new Vector2(3, -2);
            SmallBricks[4].velocity = new Vector2(-3, 8);
            destroyBrick = false;
            destroyTimer = 0.7f;
            for (int i = 1; i < 5; i++)
            {
                SmallBricks[i].gravityScale = 3;

            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (oscillating)
            if (collision.gameObject.tag == "Enemy") // If enemy is touching Brick, he will perform flip.
            {
                if (collision.transform.position.x > transform.position.x)
                    collision.gameObject.GetComponent<EnemyAbstract_script>().StartFlip(1);
                else
                    collision.gameObject.GetComponent<EnemyAbstract_script>().StartFlip(-1);

            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.collider is EdgeCollider2D) // If Mario touched Brick with his Edge collider (which is located on his head)
        {
            if (!oscillating)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, 0.2f); //Brick starts moving up
                oscillating = true;
            }
            delay = 4; // Short delay setting

            if (Mario_script.SuperMario)
                if (gameObject.tag == "Bricks")
                {
                    if (!audioSource.isPlaying)
                        audioSource.Play();
                    destroyBrick = true;

                }
        }

    }

}
