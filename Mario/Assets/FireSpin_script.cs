using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Line of fire balls rotating around an empty question block. Damages Mario on touch

public class FireSpin_script : MonoBehaviour {
  //  Transform fireBall1;
    Transform fireBall2;
    Transform fireBall3;
    Transform fireBall4;
    Transform fireBall5;
    Transform fireBall6;
    Vector2 rotationPivot;
    // Use this for initialization
    void Start () {
       // fireBall1 = transform.Find("FireBall (1)");
        fireBall2 = transform.Find("FireBall (2)");
        fireBall3 = transform.Find("FireBall (3)");
        fireBall4 = transform.Find("FireBall (4)");
        fireBall5 = transform.Find("FireBall (5)");
        fireBall6 = transform.Find("FireBall (6)");

     
    }

    // Update is called once per frame
    void Update () {

        transform.Rotate(Vector3.forward, 1);
        
    }
}
