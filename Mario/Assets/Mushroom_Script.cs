using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
/*
 * Gameobject might appear in 3 forms:
 * 1up mushroom, which gives Mario 1 extra live (public bool "1up" is set manually, to make gameobject 1up mushroom)
 * Super mushroom, which turns Mario into Super Mario
 * Fire flower, super mushroom turns into fireflower when Mario is in Super/Fire Mario form. Fire flower turns Mario into Fire Mario. Once it appeared, it doesn't move(unlike mushrooms)
 */
//
public class Mushroom_Script : MonoBehaviour {
    public LayerMask QuestionBlock; 
    BoxCollider2D MushroomCollider;
    Rigidbody2D Mushroom;
     int movementDirection;
    SpriteRenderer spriteRenderer;
    bool mushRoomAppeared; // Indicates that mushroom/fire flower isn't covered by question block anymore
    Animator myAnimator;
    EdgeCollider2D edgeCollider;
    bool fireFlower; // Turns super mushroom into fireflower
    AudioSource audioSource;
    public AudioClip powerUp;
    public bool Is1Up;  // Bool is set manually, to make mushroom appear as 1Up mushroom
    bool activate; // Allows mushroom/fire flower to start moving
    bool executed; // Pevents some part of the code to be run more than once per certain time
    QuestionBlockScript script;
    // Use this for initialization
    void Start()
    {
     //   QuestionBlock = LayerMask.GetMask("QuestionBlock");
        script = GetComponentInParent<QuestionBlockScript>();
        myAnimator = GetComponent<Animator>();
        MushroomCollider = GetComponent<BoxCollider2D>();
        Mushroom = GetComponent<Rigidbody2D>();
        movementDirection = -1; // Default direction
        spriteRenderer = GetComponent<SpriteRenderer>();
        MushroomCollider.enabled = false;
        edgeCollider = GetComponent<EdgeCollider2D>();
        spriteRenderer.enabled = false;
        Mushroom.constraints = RigidbodyConstraints2D.FreezePositionX;
        edgeCollider.enabled = false;
        audioSource = GetComponent<AudioSource>();
        Mushroom.isKinematic = true;
        if (script.invisible)
            spriteRenderer.enabled = false;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("OVERWORLD"))
            myAnimator.SetBool("OverWorld", true);
        else
            myAnimator.SetBool("OverWorld", false);
    }

    // Update is called once per frame
    void Update () {
        if (script.hit)
        {
            activate = true;
            spriteRenderer.enabled = true;
        }
        
        if (!activate) // Prevents animation changing after mushroom appearance
        {

            if (Is1Up)   // 1Up mushroom acts like ordinary super mushroom, but they look different
                myAnimator.SetBool("1Up", true);
            else
            {
                if (Mario_script.SuperMario)
                {
                    myAnimator.SetBool("SuperMushroom", false);
                    myAnimator.SetBool("FireFlower", true);
                    fireFlower = true;
                }
                else
                {
                    myAnimator.SetBool("FireFlower", false);
                    myAnimator.SetBool("SuperMushroom", true);
                    fireFlower = false;
                }
            }
        }
        else 
        {
            if (!executed)
            {
                audioSource.Play();
                executed = true;
            }
            if (!spriteRenderer.enabled)
                spriteRenderer.enabled = true;

            MushroomMoving();
        }

      
    }
public void MushroomMoving()
    {
              if (!mushRoomAppeared)
          {
        if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y), QuestionBlock)) // Mushroom moving up from question block
        {
            Mushroom.velocity = new Vector2(0, 0.7f);
        }
            else
            {
                spriteRenderer.sortingLayerName = "Default";
                Mushroom.isKinematic = false;
                mushRoomAppeared = true;
                MushroomCollider.enabled = true;
                edgeCollider.enabled = true;
                Mushroom.velocity = new Vector2(0, 0);
                if (!fireFlower) 
                    Mushroom.constraints = RigidbodyConstraints2D.FreezeRotation;
                else
                    Mushroom.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        else if (!fireFlower)
            Mushroom.velocity = new Vector2(2 * movementDirection, -10);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")          
                Destroy(gameObject);
        
        else if (collision.otherCollider is EdgeCollider2D)
            movementDirection = movementDirection * -1;
        }
}
