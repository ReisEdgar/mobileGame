using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//-----------DESCRIPTION-------------
// Coins which arent hidden in a Question blocks. Some coins are parented by Bricks in order to count as "collected" when Mario hits Bricks under them. These ones have 
// have separate prefab called Double coins
public class SingleCoin_script : MonoBehaviour {
    BrickScript script; // Used by Double coins
    bool hasParent;
    public Transform ChildObject;

	// Use this for initialization
	void Start () {
         if (SceneManager.GetActiveScene().name.Contains("UNDERWORLD"))
            GetComponent<Animator>().SetBool("Underworld", true);
        else if (SceneManager.GetActiveScene().name.Contains("SEA"))
            GetComponent<Animator>().SetBool("Sea", true);
         else
                    GetComponent<Animator>().SetBool("Overworld", true);


        if (transform.parent != null)
        {
            hasParent = true;
            script = GetComponentInParent<BrickScript>();
        }
           
	}
	
	// Update is called once per frame
	void Update () {
		if(hasParent)
        {
            if(script.oscillating)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.DetachChildren();
                GameControl.AddCoin(); // Coins are calculated and dispayed in an upper part of the screen
                Destroy(gameObject); 
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameControl.AddCoin();
            Destroy(gameObject);
        }
    }

}
