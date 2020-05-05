using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-----------DESCRIPTION--------------
// If Mario is standing on one of the platforms, it starts moving down and other moving up. If any of platforms reached top of "construction" (local position Y > -1)
// both platforms fall down.
public class DoublePlatform_script : MonoBehaviour
{
    Transform LeftRope;
    Transform RightRope;
    Transform LeftPlatform;
    Transform RightPlatform;
   public bool rightPlatformCollision { get; set; } // Indicates that Mario is standing on a right platform
   public  bool leftPlatformCollision { get; set; } // Indicates that Mario is standing on a left platform
    float rightRopeScale;
    float leftRopeScale;
     bool falling;
   Rigidbody2D leftRigidbody;
     Rigidbody2D rightRigidbody;
    BoxCollider2D rightCollider;
    BoxCollider2D leftCollider;

    // Use this for initialization
    void Start()
    {
        
        LeftRope = transform.Find("Left rope");
        RightRope = transform.Find("Right rope");
        LeftPlatform = transform.Find("Left platform");
        RightPlatform = transform.Find("Right platform");
        leftRigidbody = LeftPlatform.GetComponent<Rigidbody2D>();
        rightRigidbody = RightPlatform.GetComponent<Rigidbody2D>();
        rightCollider = RightPlatform.GetComponent<BoxCollider2D>();
        leftCollider = LeftPlatform.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!falling)
        {
            if (RightPlatform.localPosition.y > -1 || LeftPlatform.localPosition.y > -1)
            {
                falling = true;
                rightCollider.enabled = false;
                leftCollider.enabled = false;
            }
            if (leftPlatformCollision)
            {
                LeftRope.localScale = new Vector2(LeftRope.localScale.x, LeftRope.localScale.y + 0.07f); // Left rope becomes longer...
                RightRope.localScale = new Vector2(RightRope.localScale.x, RightRope.localScale.y - 0.07f); // right rope becomes shorter

                leftRigidbody.velocity = new Vector2(0, -2f);
                rightRigidbody.velocity = new Vector2(0, 2f);



            }
            else if (rightPlatformCollision)
            {
                LeftRope.localScale = new Vector2(LeftRope.localScale.x, LeftRope.localScale.y - 0.07f);
                RightRope.localScale = new Vector2(RightRope.localScale.x, RightRope.localScale.y + 0.07f);

                leftRigidbody.velocity = new Vector2(0, 2f);
                rightRigidbody.velocity = new Vector2(0, -2f);


            }
            else
            {
                leftRigidbody.velocity = Vector2.zero; // Stops both platforms when Mario is not standing on neither of them
                rightRigidbody.velocity = Vector2.zero;
            }
        }

            if (falling)
        {

            leftRigidbody.velocity = new Vector2(0, -7);
            rightRigidbody.velocity = new Vector2(0, -7);
        }
    
    }
}
