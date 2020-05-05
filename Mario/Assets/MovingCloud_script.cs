using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moving platform appearing in hidden levels in the sky. Once Mario jumps on it, ir moves constantly in one direction.

public class MovingCloud_script : MonoBehaviour {
    Rigidbody2D rigidbody;
    bool start;
    bool stop;
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (start && ! stop)
            rigidbody.velocity = new Vector2(4, 0);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "ChangeDirection")
        {
            stop = true;
            rigidbody.velocity = Vector2.zero;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y)
            start = true;
    }
    
    
}
