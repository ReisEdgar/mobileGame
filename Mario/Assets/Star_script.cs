using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//---------DESCRIPTION------------
// Rare gameobject appearing from Question blocks or Bricks. If caught, activates Mario's Invincible form and allows him to be immune to all types of enemies and flip
// them on touch.
public class Star_script : MonoBehaviour {
    Rigidbody2D rigidbody;
    BoxCollider2D boxCollider;
    EdgeCollider2D edgeCollider;
    // Use this for initialization
    void Start () {
        edgeCollider = GetComponent<EdgeCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(5, 0); // star is being thrown out of Question block/Brick

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.otherCollider is EdgeCollider2D)
            if (collision.transform != transform.parent)
                rigidbody.velocity = new Vector2(4, 5);

    }
}
