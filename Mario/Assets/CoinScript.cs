using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Coins are hidden in Question blocks and "jump up" after Mario hits Question block with his head.
public class CoinScript : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    public Rigidbody2D CoinRigibody;
    float QuestionBlockCenter; // Y posiion of an object hidden in question block
    bool goingDown; 
    public AudioSource audioSource;
  public  Scoore_script script;
    public Transform scoore;

    // Use this for initialization
    void Start () {
        CoinRigibody = GetComponent<Rigidbody2D>();

        scoore = transform.GetChild(0);
        script = GetComponentInChildren<Scoore_script>();
        audioSource = GetComponent<AudioSource>();
         spriteRenderer = GetComponent<SpriteRenderer>();
        CoinRigibody.isKinematic = false;
        CoinRigibody.AddRelativeForce(new Vector2(0, 9), ForceMode2D.Impulse); // Pushes coin up
        QuestionBlockCenter = transform.position.y - 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
   
        if ((transform.position.y > QuestionBlockCenter + 3)) // If a coin moved vertically by more than 3 units 
        {                                                // it is being pushed down
            
            CoinRigibody.AddRelativeForce(new Vector2(0, -9), ForceMode2D.Impulse);
            goingDown = true;
        }

        if (transform.position.y < QuestionBlockCenter + 1.4f && goingDown) // If a coin, while moving down, is less than 1.4 units away from Question block center it disapears.
        {
            scoore.position = new Vector2(scoore.position.x, QuestionBlockCenter);
            script.SetScoore("200");
            GameControl.AddCoin();
            Destroy(gameObject);
        }
    
    }
}
