using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------DESCRIPTION--------------
// Trees allows Mario to "climb" into a hidden Sky levels. They start growing from certain Question blocks or Bricks, once Mario hit them.
// This gameobject appears in two tipes, on grows from question blocks and teleports Mario to hidden level, other (already in a hidden level) raises Mario up
// above clouds.

public class Tree_script : MonoBehaviour {
    Transform[] childObjects; // All the parts of a tree
    int childCount;
    public bool finished { get; private set; } // Indictes that tree stoped growing
    public float rr;
    public List<int> tt;
	// Use this for initialization
	void Start () {

        childObjects = GetComponentsInChildren<Transform>();
        childCount = transform.childCount;
        tt = new List<int>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!finished && !GameControl.setBlackScreen)
        {
            Move(childObjects[childCount]); // Upper part of a tree moves separetly from others...


            for (int i = childCount - 1; i > 0; i--) // ... all other parts ar moved here
            {
                if (childObjects[i + 1].localPosition.y > 0.96f) // ...if tree part's local position Y is more that 0.96,  it means that it's sprite doesn't 
                    Move(childObjects[i]);                       // intersect with parent's sprite, and that another part needs to start moving up, in order
                                                                 // to keep continuous growth
            }
            if (childObjects[1].localPosition.y > 0.9f) // Once last tree part is directly above parent, tree parts movement stops
                finished = true;

            
        }

        if(finished && gameObject.tag == "TreeTop") // "TreeTop" tag means, that this tree is located in a hidden level and that it raises Mario above clouds
            for(int i = 0; i < childCount+1; i++)        
            {
                childObjects[i].GetComponent<EdgeCollider2D>().enabled = false; // Allows Mario to move through the tree, once he jumped of from it
            }
	}

    
    void Move(Transform tr)
    {
        tr.position = new Vector2(tr.position.x, tr.position.y + 0.04f);
    }
}
