using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 //----------DESCRIPTION-----------
 // Child object of Double platform. Indicates when Mario is standing on a certain platform

public class LeftPlatform_script : MonoBehaviour {
    DoublePlatform_script script;
    // Use this for initialization
    void Start()
    {
        script = GetComponentInParent<DoublePlatform_script>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            script.leftPlatformCollision = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            script.leftPlatformCollision = false;
    }

}
