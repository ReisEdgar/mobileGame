using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterRendering : MonoBehaviour {
    bool rendered;
    SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	//15
	// Update is called once per frame
	void Update () {

        if (transform.position.x - Camera_script.cameraPositionX < -22 && !GameControl.setBlackScreen && !Mario_script.GameOver)
        {
            if (gameObject.tag == "ShellSlide")
                KoopaTroopa_Script.shellSlideFlips = 0;
            
            Destroy(gameObject);
        }

        if (transform.position.y < -12)
            
            Destroy(gameObject);

	}
}
