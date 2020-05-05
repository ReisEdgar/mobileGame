using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Castles are loacated at the end of each level, MArio goes there after he touches the falg. Castles load another level.
public class Castle_script : MonoBehaviour {
    int delay; // Delay between making a colected coin sound
    const float FlagRaisedDelay = 2; // Delay between reducing timer to 0 and raising a flag
    AudioSource audioSource;
     bool touched; // Indicates that Mario is in castle
    Transform flag; // Flag raised above castle before loading a new level
    public string sceneToLoad; // Scene which will be loaded.
    GameObject Mario;
    float flagRaisedDelay;
    public List<string> eee = new List<string>();
    public string qqq = "";

    public int vvv;
    public static bool flagRaised { get; set; }
    // Use this for initialization
    void Start () {
        flag = transform.Find("Castle flag");
        audioSource = GetComponent<AudioSource>();
        delay = 1;
        Mario = GameObject.Find("Mario");
        flagRaisedDelay = FlagRaisedDelay;

        }

        // Update is called once per frame
        void Update()
    {
        if (touched && qqq == "")
        {
            qqq = GameControl.previousScene;
        }


        if (touched && !audioSource.isPlaying)
        {
            if (flag != null)  // Castles in 3rd level of each world lead to CASTLE level and look differently from standart castles and don't have a flag
            {
                if (flag.position.y < 5.35f) // Raising the flag
                    flag.position = new Vector2(flag.position.x, flag.position.y + 0.1f);
                else
                {
                    if (flagRaisedDelay > 0)  // Short pause before loading a level, after flag was raised
                        flagRaisedDelay -= Time.deltaTime;
                    else
                        flagRaised = true;
                }
            }
            else   
            {
                if (flagRaisedDelay > 0)   // Even if Castle of 3level of any world don't have a flag, there still is a pause before loading level.
                    flagRaisedDelay -= Time.deltaTime;
                else
                    flagRaised = true;
            }
        }

        if (touched)
        {

            if (GameControl.GameTimer != 0) // Rapid sound of coins
            {
                if (delay < 1)
                {
                    delay = 1;
                    audioSource.Play();
                }
                else
                    delay--;
            }

            if (GameControl.loadScene) // Loading new scene
            {
                flagRaised = false;
                if (SceneManager.GetActiveScene().name.Contains("Multi"))
                {
                    vvv++;
                }
                if (SceneManager.GetActiveScene().name.Contains("Multi")) // Each time Mario finishes UNDERWORLD or SES level, he moves up the pipe to MultiLevel scene
                {
                    SceneNameOUT(qqq);  // depending on a name of previous scene, scene to load name are set here
                }
                    SceneManager.LoadScene(sceneToLoad);

                if (sceneToLoad.Contains("CASTLE"))
                    Mario_script.CheckPoint = new Vector2(0, 6);
               else
                    Mario_script.CheckPoint = new Vector2(0, 0);



            }
        }
       
    }

    // Each time Mario finishes UNDERWORLD or SES level, he moves up the pipe to MultiLevel scene
    // depending on a name of previous scene, scene to load name are set here
    void SceneNameOUT(string scene)
    {

        eee.Add(scene);
        GameControl.setCurrent = true;
        switch (scene)
        {
    
            case "W1_L2_UNDERWORLD":
                {
                    sceneToLoad = "W1_L3_OVERWORLD";
                    break;
                }
            case "W2_L2_SEA":
                {
                    sceneToLoad = "W2_L3_OVERWORLD";
                    break;
                }
            case "W4_L2_UNDERWORLD":
                {
                    sceneToLoad = "W4_L3_OVERWORLD";
                    break;
                }
            case "W7_L2_SEA":
                {
                    sceneToLoad = "W7_L3_OVERWORLD";
                    break;
                }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Mario = collision.gameObject; // Only Mario can collide with the castle
        touched = true;
    }

}
