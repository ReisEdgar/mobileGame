using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Character standing at the end of each Castle level, shows message once Mario touches him, and loads new scene shortly after.

public class MushroomCitizen_script : MonoBehaviour {

    Transform child;
    GameObject text1;
    GameObject text2;
    float delay;
    float messageDelay;
    bool countDown;
    public static bool messageShown { get; private set; }
    Transform Mario;
    string[] Scenes;

	// Use this for initialization
	void Start () {
        Scenes = new string[7];
        Scenes[0] = "W2_L1_OVERWORLD";
        Scenes[1] = "W3_L1_OVERWORLD_BLACK";
        Scenes[2] = "W4_L1_OVERWORLD";
        Scenes[3] = "W5_L1_OVERWORLD";
        Scenes[4] = "W6_L1_OVERWORLD_BLACK";
        Scenes[5] = "W7_L1_OVERWORLD";
        Scenes[6] = "W8_L1_OVERWORLD";

        child = transform.GetChild(0);
        text1 = child.GetChild(0).gameObject;
        text2 = child.GetChild(1).gameObject;
        delay = 1.5f;
        messageDelay = 1;
        text1.SetActive(false);
        text2.SetActive(false);
        Mario = transform.Find("Mario");

    }

    // Update is called once per frame
    void Update () {
        if (countDown)
        {
            if (delay > 0)
                delay -= Time.deltaTime;
            else
            {
                text2.SetActive(true);
                if (messageDelay > 0)
                    messageDelay -= Time.deltaTime;
                else
                    messageShown = true;
            }

            if (GameControl.loadScene)
            {
                Mario_script.CheckPoint = Vector2.zero;
                int count = int.Parse(SceneManager.GetActiveScene().name.Substring(1, 1));
                if (count == 8)
                {
                    GameControl.reloadGame = true;
                    SceneManager.LoadScene("W1_L1_OVERWORLD");

                }
                SceneManager.LoadScene(Scenes[count - 1]);


            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bowser_script.BowserFellDown = false;
        Mario = collision.transform;
        text1.SetActive(true);
        countDown = true;
    }

}
