using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Fire balls are thrown by Mario when he is in a Fire Mario form.

public class FireBall_Script : MonoBehaviour {
    public bool Fire { get; set; }
    Rigidbody2D rigidBody;
    Transform ParentTransform;
    BoxCollider2D boxCollider;
    Animator myAnimaotor;
    Camera camera;
    float MarioDirection;
    public List<Rigidbody2D> ClonesList; // List is used to prevent creation of more than two fire balls at a time.
    AudioSource audioSource;
    public AudioClip fire;
 //   Mario_Move script;
	// Use this for initialization
	void Start () {
        audioSource = GetComponentInParent<AudioSource>();
        ClonesList = new List<Rigidbody2D>();
        camera = Camera.current;
        Fire = false;
        rigidBody = GetComponent<Rigidbody2D>();
     boxCollider = GetComponent<BoxCollider2D>();
        ParentTransform = GetComponentInParent<Transform>();
        myAnimaotor = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (myAnimaotor.GetCurrentAnimatorStateInfo(0).IsName("Destroy Object")) //Destroys fireball after explosion
        {
            if(transform.parent == null)
            Destroy(gameObject);
        }
        if(gameObject.name.Contains("Clone") && transform.position.y <-3) // Destroys fire ball if it fell down without touching anything      
            Destroy(gameObject);

        FixCloneList();

        if (Fire)
        {
            CreateFireBall();
            Fire = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // Fire ball explodes when he touches enemy with any of it's colliders
            Explode();
   
        if (collision.otherCollider is EdgeCollider2D && collision.gameObject.tag != "Player") // Fire ball explodes when touches something with edge collider located
            Explode();                                                                         // on it's side
    }

    // Deletes destroyed fire balls from Clone list
    void FixCloneList()
    {
        if (ClonesList.Count > 0)
        {
            for (int i = 0; i < ClonesList.Count; i++)
            {
                if (ClonesList[i] == null)
                    ClonesList.Remove(ClonesList[i]);
            }
        }
    }

    // Creates and fires fire ball
    void CreateFireBall()
    {
        if (ClonesList.Count < 2) // Mario can't fire more than two fire balls at once
        {
            audioSource.clip = fire;
            audioSource.Play();
            Rigidbody2D clone = Instantiate(rigidBody, new Vector3(Mario_script.PositionX + 1 * Mario_script.spriteDirection, Mario_script.PositionY + 1, 0), transform.rotation);
            clone.gravityScale = 5;
            clone.velocity = new Vector2(9 * Mario_script.spriteDirection, -6); // Fie ball is pushed once and then move by inertia
            ClonesList.Add(clone);
        }
    }

    void Explode()
    {
        myAnimaotor.SetBool("Explosion", true);
        rigidBody.velocity = new Vector2(0, 0);
        rigidBody.gravityScale = 0;
    }

}
