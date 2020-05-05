using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string SceneToLoad; // Scene which will be loaded
    public Vector2 Coordinates; // Mario's coordinates afte loading new scene
    public AudioSource audio;
    public bool touched;
    GameObject Mario;
    public bool rotate; // Rotates edge collider 90 deg.
    public static bool loading { get; set; }
    string currentScene;
    public bool setBlackScreen;
    public static bool flagRaised { get; set; }
    GameControl GameControlScript;
    // Use this for initialization

        public void Start()
    {
        GameControlScript = GameObject.Find("Game controler").GetComponent<GameControl>();
    }

    public void LoadScene(string scene)
    {
        setBlackScreen = true;

        GameControlScript.LoadNewLevel();

    //    GameControl.currentScene = scene;
        if (setBlackScreen)
        {
            GameControl.setCurrent = true;

       //     GameControl.levelLoaded = true;
        }
        if (scene.Contains("CASTLE"))
            Mario_script.CheckPoint = new Vector2(0, 6);
        else
            Mario_script.CheckPoint = new Vector2(0, 0);


        Mario_script.moveUp = true;

        SceneManager.LoadScene(scene);
    }
}