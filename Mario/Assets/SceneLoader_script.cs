using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//------------DESCRIPTION----------------
// Game object which loads scenes

public class SceneLoader_script : MonoBehaviour {

    public string SceneToLoad; // Scene which will be loaded
    public Vector2 Coordinates; // Mario's coordinates afte loading new scene
    public AudioSource audio;
    public bool touched;
    GameObject Mario;
    public bool rotate; // Rotates edge collider 90 deg.
    public static bool loading { get;  set; }
    string currentScene;
    public bool setBlackScreen;
    // Use this for initialization
    void Start () {
        currentScene = SceneManager.GetActiveScene().name;
        loading = false;
   

        Mario = GameObject.Find("Mario");
        if (rotate)
            Rotate();

	}

    // Update is called once per frame
    void Update() {
        if (audio == null)
            audio = GameObject.Find("Audio Source Music").GetComponent<AudioSource>();

        if (touched)
        {
            if (!audio.isPlaying && !GameControl.setBlackScreen)
            {
                LoadScene(SceneToLoad);
            }
        }
    }

    // Makes collider vertical
    void Rotate()
    {
        EdgeCollider2D collider = GetComponent<EdgeCollider2D>();
        Vector2[] newPoints = new Vector2[2];
        Vector2[] oldPoints = collider.points;

        for(int i = 0; i < 2; i++)
        {
            newPoints[i].x = oldPoints[i].y;
            newPoints[i].y = oldPoints[i].x;
        }
        collider.points = newPoints;


    }

    public void LoadScene(string scene)
    {

         //   GameControl.currentScene = scene;
            if (setBlackScreen)
            {
                GameControl.setCurrent = true;

                GameControl.levelLoaded = true;
            }
            Mario_script.previousCoordinates = new Vector2(Mario_script.PositionX, Mario_script.PositionY);
            Mario_script.CheckPoint = Coordinates;

            Mario_script.moveUp = true;

            SceneManager.LoadScene(scene);
    }

    // Returns coordinates where Mario will appear after returning from hidden level( Same Hidden levels are used in a few levels)
    Vector2 GetCoordinates(string scene)
    {
        switch(scene)
        {
            case "W1_L1_OVERWORLD":
                return new Vector2(160, 0);
            case "W1_L2_UNDERWORLD":
                return new Vector2(113, 0);
            case "W2_L1_OVERWORLD":
                if(currentScene.Contains("UNDERWORLD"))
                return new Vector2(132.4f, 0);
                else
                    return new Vector2(176, 10);
            case "W3_L1_OVERWORLD_BLACK":
                if (currentScene.Contains("UNDERWORLD"))
                    return new Vector2(79, 0);
                else
                    return new Vector2(189, 0);
            case "W4_L1_OVERWORLD":
                return new Vector2(153.2f, 0);
            case "W5_L1_OVERWORLD":
                return new Vector2(163.5f, 0);
            case "W5_L2_OVERWORLD":
                if (currentScene.Contains("SEA"))
                    return new Vector2(111, 0);
                else
                    return new Vector2(130, 7);
            case "W6_L2_OVERWORLD_BLACK":
                if (currentScene.Contains("SEA"))
                    return new Vector2(112, 0);
                else if (currentScene.Contains("UNDERWORLD_5"))
                    return new Vector2(31, 0);
                else if (currentScene.Contains("UNDERWORLD_4"))
                    return new Vector2(180, 0);
                else
                    return new Vector2(160, 7);
            case "W7_L1_OVERWORLD":
                return new Vector2(108.3f, 0);
            case "W8_L1_OVERWORLD":
                return new Vector2(108.6f, 0);
            case "W8_L2_OVERWORLD":
                return new Vector2(170, 0);
        }

        return Vector2.zero;
    }

    // Method is called each time Mario appears in a MultiLevel1 scene, from where, depending on a previous scene name, he travels to certain UNDERWORLD or SEA level
    void SceneAndCoordinatesIN(string scene)
    {
        Coordinates = new Vector2(0, 8);

        switch (scene)
        {
            case "W1_L1_OVERWORLD":
                {
                    SceneToLoad = "W1_L2_UNDERWORLD";
                    break;
                }
            case "W2_L1_OVERWORLD":
                {
                    SceneToLoad = "W2_L2_SEA";
                    break;
                }
            case "W4_L1_OVERWORLD":
                {
                    SceneToLoad = "W4_L2_UNDERWORLD";
                    break;
                }
            case "W7_L1_OVERWORLD":
                {
                    SceneToLoad = "W7_L2_SEA";
                    break;
                }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        loading = true;
        touched = true;
        string current = SceneManager.GetActiveScene().name;
        if (current.Contains("Hidden") && !current.Equals("Hidden_OVERWORLD_3"))
        {
            SceneToLoad = GameControl.previousScene; // Mario returns to scene, from which he moved to hidden level
            Coordinates = GetCoordinates(SceneToLoad);
        }
        else if (current.Contains("MultiLevel1"))
        {
            SceneAndCoordinatesIN(GameControl.previousScene); 
        }

    }

    private void OnLevelWasLoaded(int level)
    {
        loading = false;
    }
}
