using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//----------DESCRIPTION--------
// When facing Bowser (level boss) at the end of each world, Mario must touch axe behind Bowser in order to defeat them.
public class Axe_script : MonoBehaviour {


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bridge_script.activate = true; // Bridge on which Bowser stands. When Mario touches Axe, Bridge dissapears.
        Destroy(gameObject);
    }
}
