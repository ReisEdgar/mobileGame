using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// --------DESCRIPTION------------
// In the final level of each world, Bowser (level boss) stands on a Bridge. Once Mario jumps over Bowser and touches Axe, Bridge moves away and Bowser falls into fire.
// Bridge is composed of many child objects. Each of them is a single vertical part of a Bridge. When Bridge moves away, the leftmost child is destroyed and all other 
// childs are moved to the left. Process is repeated with short delay between each movement until all of the children are destroyed.
public class Bridge_script : MonoBehaviour {
    const float Delay = 0.01f; // Delay between Bridge movements
    float delay; // Timer for delay
    public static bool activate { get; set; } // Starts Bridge movement
     Transform[] childObjects;
     int childCount;
    bool executed; // Prevents some part of the code to be run more than once per certain time
	// Use this for initialization
	void Start () {
        delay = Delay;
        childObjects = GetComponentsInChildren<Transform>();
        childCount = transform.childCount;
	}
	
	// Update is called once per frame
	void Update () {
        if (activate)
            Movement();
	}
    
    void Movement()
    {
        if (!executed)
            GetComponent<EdgeCollider2D>().enabled = false; // Edge colider covering surface of Bridge. Sometimes Bowser can't move through intersection of two childs'
            // box colliders and gets stuck, even though their Y position are same. Edge collider prevents this bug occurance.
        if (delay <= 0)
        {
            transform.position = new Vector2(transform.position.x - 0.25f, transform.position.y);
            delay = Delay;
            Destroy(childObjects[childCount].gameObject);
            childCount--;

        }
        else
            delay -= Time.deltaTime;
    }
}


