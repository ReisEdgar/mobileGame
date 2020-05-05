using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///   -------------NEREIKALINGAS------------------
/// </summary>
public class FireFlower_Script : MonoBehaviour {
    LayerMask Ground;
    public LayerMask QuestionBlock;
    Transform groundCheck;
    Transform LeftSide;
    Transform RightSide;
    BoxCollider2D MushroomCollider;
    Rigidbody2D Mushroom;
    int movementDirection;
    SpriteRenderer spriteRenderer;
    bool mushRoomAppeared;
    // Use this for initialization
    void Start()
    {
        groundCheck = transform.Find("Ground collision");
        //   LeftSide = transform.FindChild("Left side");
        //    RightSide = transform.FindChild("Right side");
        MushroomCollider = GetComponent<BoxCollider2D>();
        Mushroom = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
        movementDirection = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //     spriteRenderer.enabled = false;
        MushroomCollider.enabled = false;
        Mushroom.isKinematic = true;
        mushRoomAppeared = false;
    }

    // Update is called once per frame
    void Update()
    {
        //   MusroomDirection();

        MushroomMoving();


    }
    public void MushroomMoving()
    {
        if (Physics2D.OverlapPoint(groundCheck.position, QuestionBlock) && !(mushRoomAppeared))
        {
            Mushroom.velocity = new Vector2(0, 0.7f);
        }
        else
        {
            mushRoomAppeared = true;
            MushroomCollider.enabled = true;
            Mushroom.velocity = new Vector2(0, 0);

        }
    }

    public void MusroomDirection()
    {
        //  if (Physics2D.OverlapPoint(RightSide.position, Ground))
        //      movementDirection = -1;
        // if (Physics2D.OverlapPoint(LeftSide.position, Ground))
        //    movementDirection = -1;
    }
    private void OnCollisionEnter2Dm(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            //  gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
