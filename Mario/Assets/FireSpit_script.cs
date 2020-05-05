using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Projectiles fired by Bowsers. Damage Mario on touch
public class FireSpit_script : MonoBehaviour
{

    public bool Fire { get; set; }
    Rigidbody2D rigidBody;
    Transform ParentTransform;
    BoxCollider2D boxCollider;
    Animator myAnimaotor;
    float fireSpitPosition; // Position from which projectile start it's movement
    Rigidbody2D projectile; // Rigidbody of clone gameobjects
    int fireMovement; // Indicates type of movement
    bool executed; // Prevents some part of a code to be run more than once per certain time
    int direction; // Movement direction
    // Use this for initialization
    void Start()
    {
        
            ParentTransform = GameObject.Find("Bowser").transform;
        Fire = false;
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        myAnimaotor = GetComponent<Animator>();
        executed = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Bowser_script.BowserDead) // Once Mario touches the Axe at the CASTLE level, all fire spits are destoyed 
            Destroy(gameObject);

        if (Fire)
        {
            fireSpitPosition = ParentTransform.position.y + 1.2f;
            direction = Bowser_script.spriteDirection; 
           float randomNumber = Random.value;

            if (randomNumber <= 0.33) // Random selection of projectile movement
                fireMovement = 1;
            else if (randomNumber <= 0.66)
                fireMovement = 2;
            else
                fireMovement = 3;

            if (direction == 1)
                projectile = Instantiate(rigidBody, new Vector3(ParentTransform.position.x - 1.2f, fireSpitPosition, 0), transform.rotation);
            else
                projectile = Instantiate(rigidBody, new Vector3(ParentTransform.position.x + 1.2f, fireSpitPosition, 0), transform.rotation);

            Fire = false;
        }

        Movement();


    }
     
    // Projectile might move in three ways, which ar chosen randomly
    void Movement()
    {
        if (projectile != null)
            if (fireMovement == 1)
            {
                if (projectile.transform.position.y < fireSpitPosition + 0.7f) // Moves diagonally up and then straight formward
                    projectile.velocity = new Vector2(-6 * direction, 2.5f);
                else
                    projectile.velocity = new Vector2(-6 * direction, 0);
            }
            else if (fireMovement == 2)
            {                                                           
                    projectile.velocity = new Vector2(-6 * direction, 0); // Moves straight forward
            }
            else
            {
                if (projectile.transform.position.y > fireSpitPosition - 1.5f)// Moves diagonally down and then straight formward
                    projectile.velocity = new Vector2(-6 * direction, -2.5f);
                else
                    projectile.velocity = new Vector2(-6 * direction, 0);
            }
    }

}
