using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------DESCRIPTION------------
// Flag at the end of each level (except CASTLE levels). 

public class Flag_Script : MonoBehaviour {
    Transform Flag; // Child object
    Rigidbody2D FlagRigidbody; // Child's rigidbody
    bool executed;  // Prevets part of the code to be run more than once per certain time
    public static bool flagDown; // Indicates that flag finished moving down the pole
    Scoore_script scooreScript; 
    public static bool FlagTouched; // Indicates that Mario touced the flag's pole

    // Use this for initialization
    void Start () {
        scooreScript = GetComponentInChildren<Scoore_script>();
        Flag = transform.Find("Flag child");
        FlagRigidbody = Flag.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Mario_script.MarioInCastle) // Resets static bool
        {
            FlagTouched = false;
            flagDown = false;
        }
        if(Flag.position.y < 1.1 && !Mario_script.MarioInCastle)
         {
            FlagRigidbody.velocity = Vector2.zero;
            flagDown = true;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FlagTouched = true;
        FlagRigidbody.velocity = new Vector2(0, -6);
        if (!executed)
        {
            AudioSource_script.FlagTouch = true; // Changes background music
            executed = true;
        }

            float marioPositionY = collision.transform.position.y;
            if (!Mario_script.InAir)                // Sets scoore depending where Mario touched flag pole
                    scooreScript.SetScoore("100");
                else if (marioPositionY < 7.2f)
                scooreScript.SetScoore("400");
                   else
                scooreScript.SetScoore("5000");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
                scooreScript.SetScoore("2000"); // Indicates that Mario touched the flag before the pole
    }


}
