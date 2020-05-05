using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------DESCRIPTION-------------
// Hammer thrown by Hammer Bros. Kills Mario on touch.

public class Hammer_script : MonoBehaviour {
    const float TimerValue = 0.7f; // Value of a delay between instanciating of a hammer and throwing 
    bool Throw; // Allows to instantiate new hammer
    Rigidbody2D rigidBody;
    float timer; // Timer for delay between instanciating of a hammer and throwing 
    bool rotate; // Starts rotation of a hammer
    public bool startTimer { get; private set; }  
    Rigidbody2D hammer; // Clone of a gameobject
    HammerBro_script scriptH; // \ 
    Bowser_script scriptB;//     /
    bool HammerBro;//           \  Hammer is used by both Hammer bros and Bowsers
    bool Bowser;//              /
    public Hammer_script Instance { get; set; } // Instance of parented hammer, which creates clones
 
    // Use this for initialization
    void Start()
    {
        if (transform.parent != null)
        {
            Instance = this;
            if (transform.parent.name.Contains("Hammer"))
            {
                scriptH = GetComponentInParent<HammerBro_script>();
                HammerBro = true;
            }
            else
            {
                scriptB = GetComponentInParent<Bowser_script>();
                Bowser = true;
            }
        }
        Throw = false;
        rigidBody = GetComponent<Rigidbody2D>();
        timer = TimerValue;

    }

    // Update is called once per frame
    void Update()
    {
        if (Instance == null) // Prevents clones from being stuck in the air in case of hammer bro killing, while he was holding hammer.
            Destroy(gameObject);

        if (HammerBro)
        {
            if (scriptH.Damaged)
                Destroy(gameObject);
        }
        else if (Bowser)
        {
            if (scriptB.Damaged)
                Destroy(gameObject);
        }

        if (transform.position.y < -20) // Destroys object when it fell down far enough
            Destroy(gameObject);

        if (Throw) 
        {
              if(transform.parent.localScale.x > 0)
            hammer = Instantiate(rigidBody, new Vector3(transform.parent.position.x - 0.15f, transform.parent.position.y + 1.5f, 0), transform.rotation);
            else
                hammer = Instantiate(rigidBody, new Vector3(transform.parent.position.x - 0.0f, transform.parent.position.y + 1.5f, 0), transform.rotation);

            hammer.GetComponent<Hammer_script>().Instance = this; // each clone has an instance of it's creator

            startTimer = true;
            Throw = false;
         }
      if(startTimer) // Hammer bro will hold hammer a for a few moements before throwing it 
        {
            if(transform.parent.localScale.x > 0)   // Allows to reposition hammer if Hammer bro turns around
            hammer.transform.position = new Vector3(transform.parent.position.x - 0.15f, transform.parent.position.y + 1.5f, 0);
            else
                hammer.transform.position = new Vector3(transform.parent.position.x - 0.0f, transform.parent.position.y + 1.5f, 0);

            timer -= Time.deltaTime;
            if (timer <= 0 || ! HammerBro) // Bowser don't hold hammers and thorw them immediately 
            {                
                if(HammerBro)
                hammer.velocity = new Vector2(-6 * transform.parent.localScale.x, 7);
                else 
                    hammer.velocity = new Vector2(-9 * transform.parent.localScale.x, 11);

                hammer.freezeRotation = false;
                hammer.GetComponent<Hammer_script>().rotate = true;
                startTimer = false;
                timer = TimerValue;
                hammer.gravityScale = 2;
            }
        }
      if(rotate)
            transform.Rotate(Vector3.forward, 10);
        
            
    }
    public void ThrowHammer()
    {
        Throw = true;
    }
}
