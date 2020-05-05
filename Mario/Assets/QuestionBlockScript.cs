using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
---------------
DESCRIPTION
---------------
Question blocks are objects which contain coins, mushrooms, fire flowers or stars hidden in them. To collect objects hidden in Question block player must hit it with
head. Question blocks might be invisible too.
 */
public class QuestionBlockScript : MonoBehaviour
{
    BoxCollider2D QuestionBlockCollider; 
    Animator myAnimator;
    public Transform ChildObject; // Transform of an object hidden in Question block, if there is only one
    public List<Transform> CoinsArray; // If there are multiple coins in a question block, they are placed here
    public bool invisible; // Question block is invisible and ignores collision with box colliders
    SpriteRenderer spriteRenderer;
    public bool isBrick; // Question block will look like a brick
    Scoore_script script;
    public int childCount; 
    public int childIndex; // Is used to activate coins when there are more than one
    //bool singleCoin; // Indicates that question block has only one coin
    int coinsCount;
    public bool hit { get; private set; }
    // Use this for initialization
    void Start()
    {
        childCount = transform.childCount;

        if (childCount > 1) // If question block has multiple coins
        {
           Transform[] temporaryArray = GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < temporaryArray.Length; i++)
            {
                if (temporaryArray[i].tag == "Coin")
                    CoinsArray.Add(temporaryArray[i]);
            }
                coinsCount = CoinsArray.Count;
        }
        else
            ChildObject = transform.GetChild(0);

        spriteRenderer = GetComponent<SpriteRenderer>();
        QuestionBlockCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (scene.Contains("OVERWORLD")) // Both over world and under world question blocks use same animator.
            myAnimator.SetBool("OverWorld", true);                                                // Following animation is chosen here
        else if(scene.Contains("CASTLE"))
            myAnimator.SetBool("Castle", true);
        else
            myAnimator.SetBool("OverWorld", false);                                                // Following animation is chosen here

        if (invisible)
        {
            spriteRenderer.enabled = false;
        }

        myAnimator.SetBool("IsBrick", isBrick);

    }

    // Update is called once per frame
    void Update()
    {
/*
            if(childIsCoin && !scooreExecuted)
            if(transform.childCount < childCount)
            {
            childCount--;
          //  if (singleCoin)
            //    script.SetScoore("200");
            //else
              //  Scoores[childIndex].SetScoore("200");
                childIndex++;
                scooreExecuted = true;
            }
        
    */
        // Script starts working after OnCollisionEnter2D is invoked

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.collider is EdgeCollider2D) // If Question block collided with Mario's edge collider(located on his head)
        {
            if (invisible)
            {
                spriteRenderer.enabled = true;
            }
            if (ChildObject != null)
            {
                hit = true; // Allows mushroom to star moving .
                            // He is always set as Active, to prevent animations changing during his movement up

                ChildObject.gameObject.SetActive(true);

            }
            if (!isBrick)
                myAnimator.SetBool("IsHit", true); //Changes sprite of a Question block

            if (childIndex < coinsCount)
            {
                CoinsArray[childIndex].gameObject.SetActive(true);
                childIndex++;
            }
            else
                myAnimator.SetBool("IsHit", true); //Changes sprite of a Question block


        }

            
        

        
    }
}
