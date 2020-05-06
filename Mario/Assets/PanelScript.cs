using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{
    public static PanelScript Instance;

    public static bool MenuOpened;
    public string aaa;
    public GameObject Menu;
    public GameObject Panel;
    public GameObject Options;
    public GameObject About;
    public GameObject Levels;
    public GameObject HighScore;

    public Button MenuButton;
    public Button BackButton;
    public Button OptionsButton;
    public Button AboutButton;
    public Button ExitButton;
    public Button LevelsButton;
    public Button LoadButton;
    public Button HighscoreButton;
    public Toggle EasyModeToggle;
    
    public Dropdown WorldDropdown;
    public Dropdown LevelDropdown;
    public float panelX;
    public float panelY;
    public float panelZ;
    public LevelLoader LevelLoader;
    public Mario_script mario_Script;
    public GameControl gameControl_script;
    bool init;
    // Start is called before the first frame update
    void Start()
    {
        mario_Script = GameObject.Find("Mario").GetComponent<Mario_script>();
        gameControl_script = GameObject.Find("Game controler").GetComponent<GameControl>();
        Menu = GameObject.Find("Menu component");
        Panel = GameObject.Find("Panel");
        Options = GameObject.Find("Options component");
        About = GameObject.Find("About component");
        Levels = GameObject.Find("Levels component");
        HighScore = GameObject.Find("Highscore component");
        
        MenuButton = GameObject.Find("MenuButton").GetComponent<Button>();
        BackButton = Menu.transform.Find("Back").GetComponent<Button>();
        OptionsButton = Menu.transform.Find("Options").GetComponent<Button>();
        AboutButton = Menu.transform.Find("About").GetComponent<Button>();
        ExitButton = Menu.transform.Find("Exit").GetComponent<Button>();
        LoadButton = Levels.transform.Find("Load").GetComponent<Button>();
        LevelsButton = Menu.transform.Find("Levels").GetComponent<Button>();
        EasyModeToggle = Menu.transform.Find("Toggle").GetComponent<Toggle>();
        HighscoreButton = Menu.transform.Find("Highscore").GetComponent<Button>();
        
        WorldDropdown = Levels.transform.Find("Worlds").GetComponent<Dropdown>();
        LevelDropdown = Levels.transform.Find("Levels").GetComponent<Dropdown>();
        //  MenuButton.OnSubmit();
        //   MenuButton.OnPointerClick(OpenMenu);
        HighscoreButton.onClick.AddListener(ShowHighscore);
        MenuButton.onClick.AddListener(OpenMenu);
        LevelsButton.onClick.AddListener(ShowLevels);
        LoadButton.onClick.AddListener(LoadLevel);
        BackButton.onClick.AddListener(CloseMenu);
        OptionsButton.onClick.AddListener(ShowOptions);
        AboutButton.onClick.AddListener(ShowAbout);
        ExitButton.onClick.AddListener(CloseMenu);
        var fff = gameControl_script.Easy;
        init = true;
        EasyModeToggle.onValueChanged.AddListener(delegate {
            EasyModeSwtich();
        });

        LevelLoader = GetComponent<LevelLoader>();
        panelX = Panel.transform.position.x;
        panelY = Panel.transform.position.y;
        panelZ = Panel.transform.position.z;

          HidePanel();
        gameControl_script.Easy = fff;
        EasyModeToggle.isOn = fff;


    }

    private void LoadLevel()
    {

        var sceneName = "";
        if (WorldDropdown.value == 0)
        {
            if (LevelDropdown.value == 0)
            {
                sceneName = "W1_L1_OVERWORLD";
            }
            else if (LevelDropdown.value == 1)
            {
                sceneName = "W1_L2_UNDERWORLD";
            }
            else if (LevelDropdown.value == 2)
            {
                sceneName = "W1_L3_OVERWORLD";
            }
            else if (LevelDropdown.value == 3)
            {
                sceneName = "W1_L4_CASTLE";
            }
        }
        else if (WorldDropdown.value == 1)
        {
            if (LevelDropdown.value == 0)
            {
                sceneName = "W2_L1_OVERWORLD";
            }
            else if (LevelDropdown.value == 1)
            {
                sceneName = "W2_L2_SEA";
            }
            else if (LevelDropdown.value == 2)
            {
                sceneName = "W2_L3_OVERWORLD";
            }
            else if (LevelDropdown.value == 3)
            {
                sceneName = "W2_L4_CASTLE";
            }
        }
        else if (WorldDropdown.value == 2)
        {
            if (LevelDropdown.value == 0)
            {
                sceneName = "W3_L1_OVERWORLD_BLACK";
            }
            else if (LevelDropdown.value == 1)
            {
                sceneName = "W3_L2_OVERWORLD_BLACK";
            }
            else if (LevelDropdown.value == 1)
            {
                sceneName = "W3_L3_OVERWORLD_BLACK";
            }
            else if (LevelDropdown.value == 3)
            {
                sceneName = "W3_L4_CASTLE";
            }
        }
        else if (WorldDropdown.value == 3)
        {
            if (LevelDropdown.value == 0)
            {
                sceneName = "W4_L1_OVERWORLD";
            }
            else if (LevelDropdown.value == 1)
            {
                sceneName = "W4_L2_UNDERWORLD";
            }
            else if (LevelDropdown.value == 2)
            {
                sceneName = "W4_L3_OVERWORLD";
            }
            else if (LevelDropdown.value == 3)
            {
                sceneName = "W4_L4_OVERWORLD";
            }
        }
        aaa = sceneName;
        CloseMenu();
        LevelLoader.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
   

    }
    public void SClick()
    {
        MyButtons.SClick = true;
        Debug.Log("ttttttttttttttttt");
    }
    public void SUp()
    {
        MyButtons.SUp = true;
        MyButtons.SDown = false;
        MyButtons.SClick = false;
        Debug.Log("up");
    }
    public void SDown()
    {
        MyButtons.SDown = true;
        MyButtons.SUp = false;
    }

    public void XClick()
    {
        MyButtons.XClick = true;
    }
    public void XUp()
    {
        MyButtons.XUp = true;
        MyButtons.XDown = false;
        MyButtons.XClick = false;
    }
    public void XDown()
    {
        MyButtons.XDown = true;
        MyButtons.XUp = false;
    }


    public void ZClick()
    {
        MyButtons.ZClick = true;
    }
    public void ZUp()
    {
        MyButtons.ZUp = true;
        MyButtons.ZDown = false;
        MyButtons.ZClick = false;
    }
    public void ZDown()
    {
        MyButtons.ZDown = true;
        MyButtons.ZUp = false;
    }


    public void RightClick()
    {
        MyButtons.RightClick = true;
    }
    public void RightUp()
    {
        MyButtons.RightUp = true;
        MyButtons.RightDown = false;
        MyButtons.RightClick = false;
    }
    public void RightDown()
    {
        MyButtons.RightDown = true;
        MyButtons.RightUp = false;
    }


    public void LeftClick()
    {
        MyButtons.LeftClick = true;
    }
    public void LeftUp()
    {
        MyButtons.LeftUp = true;
        MyButtons.LeftDown = false;
        MyButtons.LeftClick = false;
    }
    public void LeftDown()
    {
        MyButtons.LeftDown = true;
        MyButtons.LeftUp = false;
    }


    public void UpClick()
    {
        MyButtons.UpClick = true;
    }
    public void UpUp()
    {
        MyButtons.UpUp = true;
        MyButtons.UpDown = false;
        MyButtons.UpClick = false;
    }
    public void UpDown()
    {
        MyButtons.UpDown = true;
        MyButtons.UpUp = false;
    }


    public void DownClick()
    {
        MyButtons.DownClick = true;
    }
    public void DownUp()
    {
        MyButtons.DownUp = true;
        MyButtons.DownDown = false;
        MyButtons.DownClick = false;
    }
    public void DownDown()
    {
        MyButtons.DownDown = true;
        MyButtons.DownUp = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CloseMenu()
    {
        MenuOpened = false;

        HidePanel();
    }

    public void EasyModeSwtich()
    {
        if (!init)
        {
            Debug.Log("-----------------------------");
            gameControl_script.Easy = !gameControl_script.Easy;
            if (gameControl_script.Easy)
            {
                mario_Script.Go();
            }
        }
        else
        {
            init = false;
        }
    }

    public void OpenMenu()
    {
        MenuOpened = true;

        ShowPanel();
     }
    public void HidePanel()
    {
        Panel.transform.position = new Vector3(-1000, -1000, 0);
    }

    public void ShowHighscore()
    {
        HideAll();
        HighScore.transform.localPosition = Vector3.zero;
    }

    public void ShowOptions()
    {
        HideAll();
        Options.transform.localPosition = Vector3.zero;
    }

    public void ShowLevels()
    {
        HideAll();
        Levels.transform.localPosition = Vector3.zero;
    }

    public void ShowAbout()
    {
        HideAll();
        About.transform.localPosition = Vector3.zero;
    }
    public void ShowMenu()
    {
        HideAll();
        Menu.transform.localPosition = Vector3.zero;
    }
    public void HideAll()
    {
        Levels.transform.localPosition = new Vector3(-1000, -1000, -1000);
        Options.transform.localPosition = new Vector3(-1000, -1000, -1000);
        About.transform.localPosition = new Vector3(-1000, -1000, -1000);
        Menu.transform.localPosition = new Vector3(-1000, -1000, -1000);
        HighScore.transform.localPosition = new Vector3(-1000, -1000, -1000);

    }
    public void ShowPanel()
    {
     //   Panel.transform.position = Vector3.zero;
        Panel.transform.position = new Vector3(panelX, panelY, panelZ);
      //  Panel.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 11));
        ShowMenu();
    }
    void Awake()
    {
        if (Instance)  // Prevents creation of second Mario while loading level
        {
            Destroy(gameObject);
            Start();
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
     }
}
