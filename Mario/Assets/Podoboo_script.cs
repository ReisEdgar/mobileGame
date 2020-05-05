using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Lethal object that damages Mario on touch. Jumps up and down from lava 

public class Podoboo_script : MonoBehaviour {
    const int maxVelocity = 7;
    public float currentVelocity; // Vertical velocity of gameobject when it slows down
    Rigidbody2D rigidbody;
    public bool slowDown; // Starts slowing Podoboo down
    public int direction; // Vertical moveent direction
	// Use this for initialization
	void Start () {
        direction = 1;
        currentVelocity = maxVelocity;
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!slowDown)  
            rigidbody.velocity = new Vector2(0, maxVelocity*direction);

        if (rigidbody.velocity.y > 0)
            transform.localScale = new Vector2(1, 1);
        else
            transform.localScale = new Vector2(1, -1);


        if (slowDown)  // Slows down, changes direction and then speeds up gameobject
            {
            if (direction == 1)
            {
                
                rigidbody.velocity = new Vector2(0, currentVelocity -= 0.2f);
                if (Mathf.Abs(currentVelocity) >= maxVelocity)
                {
                    slowDown = false;
                    direction = -1;
                    currentVelocity = -maxVelocity;
                    
                }
            }
            else 
            {
                rigidbody.velocity = new Vector2(0, currentVelocity += 0.2f);
                if (Mathf.Abs(currentVelocity) >= maxVelocity)
                {
                    slowDown = false;
                    direction = 1;
                    currentVelocity = maxVelocity;

                }
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ChangeDirection") 
            slowDown = true;
    }
}
