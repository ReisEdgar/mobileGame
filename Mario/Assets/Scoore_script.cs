using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scoore_script : MonoBehaviour {

    //-----DESCRIPTION--------
    // Scoore displaying

    Rigidbody2D rigidbody;
    const float RenderTimer = 0.7f; // Amount of time scoore will be visible
    public float renderTimer;
    Text text; // Text of scoore
    bool executed; // Prevents some part of a code to be run more than once per certain time
   public Transform parent { get; set; } 
   public static int TotalScoore { get; set; }
    // Use this for initialization
    void Start() {
        text = GetComponentInChildren<Text>();
        rigidbody = GetComponent<Rigidbody2D>();

        text.text = " ";
        parent = transform.GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update() {

        Movement();

        if (renderTimer <= 0 && executed ) // Allowing same scoore to be displayed more than once
        {
                transform.SetParent(parent);
            text.text = " ";
            if (parent != null)
                transform.position = parent.position;
            else
                Destroy(gameObject);
            rigidbody.velocity = Vector2.zero;
            executed = false;
            renderTimer = 0;
        }
	
    }
    // All scoores move up and follows Mario for a short time
    public void Movement()
    {
        if (renderTimer > 0)
        {
            renderTimer -= Time.deltaTime;


            if (gameObject.tag == "Enemy") // Scoore has same tag as it's parent
            {

                if (Mario_script.VelocityX > 0 && Mario_script.PositionX > transform.position.x)
                    rigidbody.velocity = new Vector2(Mario_script.VelocityX, 2.5f);
                else
                    rigidbody.velocity = new Vector2(0, 2.5f);

            }

            else if (gameObject.tag == "QuestionBlock")
            {

                if (Mario_script.VelocityX > 0 && Mario_script.PositionX - transform.position.x > 2)
                    rigidbody.velocity = new Vector2(Mario_script.VelocityX * (2 / 3), 2.5f);
                else if (Mario_script.VelocityX <= 0)
                    rigidbody.velocity = new Vector2(0, 2.5f);

            }
            else if (gameObject.tag == "Player")
                rigidbody.velocity = new Vector2(0, 2.5f);
            else if (gameObject.tag == "Flag")
            {
                if (transform.position.y < 9)
                    rigidbody.velocity = new Vector2(0, 3.5f);
                else
                    rigidbody.velocity = Vector2.zero;
            }
        }

    }

    // Scoore is set by it'a parent and becomes visible
    public void SetScoore(string scoore)
    {
        if (!executed)
        {
            if(gameObject.tag != "Flag")
            renderTimer = RenderTimer;
            else
                renderTimer = RenderTimer*40; // Flag's scoore doesn't dissapear

            executed = true;
            transform.parent = null; // Prevents premature scoore destruction together with it's parent
            text.text = scoore; 
            int Scoore = 0;
            try
            {
                Scoore = int.Parse(scoore); // For those cases when Mario gets extra live from mushroom and intead of numbers, text is "1UP"
            }
            catch (System.Exception)
            { }
            if (Scoore != 0)
              AddTotalScoore(Scoore);
        }

    }

    
    public static void AddTotalScoore(int scoore)
    {
        TotalScoore += scoore;
    }
}
