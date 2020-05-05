using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Camera_script : MonoBehaviour
{
    static Camera_script Instance;
    string currentScene;
    public GameObject Mario;
    public static float cameraPositionX { get; set; }
    Camera camera;
    Color blueBackGround;
    Color BackGroundColor;
    bool setBackground;
    bool stop; // Prevents camera movement in a Hidden Under world levels
    bool shift; // Prevents small camera movement at the end of a level, when Flag_Script.FlagTouched is set back to false 
    void Start()
    {
        blueBackGround = new Color(0.298f, 0.47f, 0.807f);
        camera = GetComponent<Camera>();
        camera.backgroundColor = blueBackGround;
        currentScene = SceneManager.GetActiveScene().name;
        Mario = GameObject.Find("Mario");
        //    transform.position = new Vector3(7, 5.5f, -11);
        SetBackGroundColor();

    }

    void Update()
    {

        cameraPositionX = transform.position.x;

        if (GameControl.setBlackScreen)  // Black background appears each time Mario dies or finishes the level
        {
            transform.position = new Vector2(-10, 30);
            camera.backgroundColor = Color.black;
            setBackground = false;
        }
        else
        {
            if (!setBackground)
                SetBackGroundColor();

            if(!stop)
            if (Mario.transform.position.x > transform.position.x)
            {
                
                if (!Flag_Script.FlagTouched || !shift)
                    transform.position = new Vector3(Mario.transform.position.x, 5.5f, -11);
                else 
                {
                        shift = true;
                    if (Mario.transform.position.x - 1 > transform.position.x) // PRevents rapid camera movement when Mario position x is changed, when he moves down with the flag at the end of a level
                        transform.position = new Vector3(Mario.transform.position.x - 1, 5.5f, -11);

                }


            }
        }

    }



    public void Restart()
    { 
            currentScene = SceneManager.GetActiveScene().name;

            Mario = GameObject.Find("Mario");
            setBackground = false;
            shift = false;
    }

    public void SetBackGroundColor()
    {
        if (currentScene.Contains("MultiLevel1"))
            transform.position = new Vector3(9, 5.5f, -11);
        else
            transform.position = new Vector3(6, 5.5f, -11);
        if (currentScene.Contains("Hidden_UNDERWORLD"))
            stop = true;
        else
            stop = false;
            setBackground = true;
        string scene = SceneManager.GetActiveScene().name;
        if (scene.Contains("BLACK") || scene.Contains("CASTLE") || scene.Contains("UNDERWORLD"))
            BackGroundColor = Color.black;
        else
            BackGroundColor = blueBackGround;

        camera.backgroundColor = BackGroundColor;
    }

    private void OnLevelWasLoaded(int level)
    {
        Restart();

    }


    void Awake()
    {
        if (Instance) // PRevents creation of camera clones when loading new level
        {
            Destroy(gameObject);
        }
        else
        {

            DontDestroyOnLoad(gameObject);
            Instance = this;

        }
    }
}
