using Assets;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// Class responsible for overall control of game: time, scoore calculation and display, reloading levels, multiplayer and etc.

public class GameControl : MonoBehaviour
{
    const int GameTimerValue = 400; // Amount of time player has to to finish the level
    public static int GameTimer { get; private set; }
    const float secondValue = 0.4f;  // Period of time, after which Game timer will decrease by 1
    const float quickSecondValue = 0.002f; // After Mario finishes level, Game timer decresases more repildy
    const int BlackScreenDelay = 3; // Delay before loading new level or reloading old one, after Mario dies. Black screen is shown during this period of time.
    float second; // Timer for Game timer decreasing
    public Text TimerText;             //    /
    public Text LevelCountText;        //    |
    public Text CoinCountText;         //    |
    public Text WorldLabelBlackScreen; //    |  Texts which are shown on a balck screen and during play-time
    public Text LevelCountBlackScreen; //    |
    public Text LivesCountBlackScreen; //    |
    public Text GameOverLabel;         //    |
    public Text ScooreText;            //    \
    SpriteRenderer spriteRenderer; // Image of Mario shown on a black screen
    public Text[] texts;
    int amountOfNumbers;  // Amout of numbers in a multi-digit scoore count
    string scoore; // String for displaying scoore
    int tempAmountOfNumbers; // This integer is used to check when amountOfNumbers should be increased
    string zeros; // Zeros ("000...") for displaying scoore
    public static bool setBlackScreen { get; private set; }
    bool setBlackScreenExecuted;
    static GameControl Instance; // First instance of game object. PRevents creation of clones when loading new level
    static int CoinCount; // Each coin collected is counted
    static bool changeCoinCount; //Allows to incresase coin count displayed on the screen
    bool restart; // Allows to restart few variables, after Mario finishes level
    private static string CurrentScene { get; set; }
    private static string PreviousScene { get; set; }
    public static string currentScene
    {
        get { return GameControl.CurrentScene; }
        set
        {
            Debug.Log("Current");
            Debug.Log(value);
            GameControl.CurrentScene = value;
        }
    } // Scene which will be reloaded after MArio dies(if he still has lives)
    public static string previousScene
    {
        get { return GameControl.PreviousScene; }
        set
        {
            Debug.Log("Previous");
            Debug.Log(value);
            GameControl.PreviousScene = value;
        }
    } // Scene which was loaded before current one
    public string crs; // Scene which will be reloaded after MArio dies(if he still has lives)
    public string pvs; // Scene which was loaded before current one

    float blackScreenDelay; // Timer 
    public static bool loadScene { get; private set; } // Allows Castle_script and MushroomCitizen_script to load new scene
    public new AudioSource audio;
    GameObject Mario;
    Mario_script marioScript;
    AudioSource_script audioScript;
    bool executed; // Prevents some part of a code to be run more than once per certain time
    bool gameOver;
    bool timeUp; // Indicates that Mario died because game timer reached 0
    public static bool levelLoaded { get; set; } // Indicates that new (not hidden) level was loaded 
    public static bool MultiPlayer { get; private set; }
    public static bool IsMario { get; set; } // When playing in multi player, bool indicates which character (Mario/Luigi) is used
    SpriteRenderer MarioImage; // Image of Mario on a black screen, used in multiplayer
    SpriteRenderer LuigiImage;// Image of Mario on a black screen, used in multiplayer
    bool multiPlayerExecuted;// Prevents some part of a code to be run more than once per certain time
    bool blackScreenOnLevelLoaded; // Sets black screen when scene was loaded with scene loader game obejct
    bool noTexts; // Black screen won't have any texts. It is required in order to allow animation sets change in an UNDERWORLD levels, in other case
                  // just after loading a scene, player would see animations changing
    public static bool GameStart; // Blocks all movements and user input while menu screen is visible
    bool gameStratExecuted;  // Prevents some part of a code to be run more than once per certain time
    public static Vector2 currentPlayerPosition { get; set; }
    Transform Selector; // Small mushroom cursor, used to choose 1/2 player game
    bool firstBlackScreen; // Showing blackscreen once at the very beggining of a game
    public static bool reloadGame { get; set; } // Reload game after game was finished or player doesn't have anymore lives
    public static bool setCurrent { get; set; }// Level count is calculated by current scene name. In other cases, it is done by using previous scene name
    // because few levels start in a MultiLevel1 scene and only then move to a actual level.
    bool stop;
    public static bool EasyMode;
    public bool gggg;
    public Highscore highscoore;
    // Use this for initialization
    void Start()
    {
        highscoore = GameObject.Find("Highscore component").GetComponent<Highscore>();
        firstBlackScreen = true;
        Selector = transform.Find("Selector");
        SaveLoad.MarioLevel = "W1_L1_OVERWORLD";
        SaveLoad.LuigiLevel = "W1_L1_OVERWORLD";
        SaveLoad.LuigiLives = Mario_script.LivesConst;
        SaveLoad.LuigiCheckPoint = Vector2.zero;
        SaveLoad.MarioCheckPoint = Vector2.zero;
        IsMario = true;
        previousScene = "W1_L1_OVERWORLD";
        Mario = GameObject.Find("Mario");
        audioScript = audio.GetComponent<AudioSource_script>();
        currentScene = "W1_L1_OVERWORLD";
        blackScreenDelay = BlackScreenDelay;
        texts = GetComponentsInChildren<Text>();
        zeros = "000000";
        foreach (Text text in texts)
        {
            if (text.name == "Time")
                TimerText = text;
            else if (text.name == "ScooreCount")
                ScooreText = text;
            else if (text.name == "Coin Count")
                CoinCountText = text;
            else if (text.name == "World label on black screen")
                WorldLabelBlackScreen = text;
            else if (text.name == "World level count on black screen")
                LevelCountBlackScreen = text;
            else if (text.name == "World level count")
                LevelCountText = text;
            else if (text.name == "Lives count")
                LivesCountBlackScreen = text;
            else if (text.name == "Game over label")
                GameOverLabel = text;

        }

        spriteRenderer = transform.Find("Display").Find("Mario image").GetComponent<SpriteRenderer>();
        MarioImage = transform.Find("Display").Find("Mario image").GetComponent<SpriteRenderer>();
        LuigiImage = transform.Find("Display").Find("Luigi image").GetComponent<SpriteRenderer>();
        LuigiImage.enabled = false;
        GameOverLabel.enabled = false;
        spriteRenderer.enabled = false;
        WorldLabelBlackScreen.enabled = false;
        LivesCountBlackScreen.enabled = false;
        LevelCountBlackScreen.enabled = false;
        GameTimer = GameTimerValue;
        second = secondValue;
        marioScript = Mario.GetComponent<Mario_script>();

    }

    // Update is called once per frame
    void Update()
    {
        gggg = EasyMode;
        crs = currentScene;
        pvs = previousScene;
        if (!GameStart)         // Menu allowing to chose single/multi player game
        {
            EnemyAbstract_script.GlobalFreezMovement = true;
            Mario_script.freezeMovement = true;
            if (MyButtons.UpDown || MyButtons.DownDown)
            {
                if (Selector.localPosition.y == -1.9f)
                    Selector.localPosition = new Vector2(-6.15f, -2.9f);
                else
                    Selector.localPosition = new Vector2(-6.15f, -1.9f);

            }
            if (MyButtons.SDown)
            {
                if (Selector.localPosition.y == -2.9f)
                    MultiPlayer = true;

                GameStart = true;
                setBlackScreen = true;
                Selector.GetComponent<SpriteRenderer>().enabled = false;
                transform.Find("Image").GetComponent<SpriteRenderer>().enabled = false;
                transform.Find("Player1").GetComponent<Canvas>().enabled = false;
                transform.Find("Player2").GetComponent<Canvas>().enabled = false;
                EnemyAbstract_script.GlobalFreezMovement = false;
                Mario_script.freezeMovement = false;
            }

        }
        else if (!gameStratExecuted)
        {
            gameStratExecuted = true;

        }

        if (Input.GetKey("escape"))
            Application.Quit();

        if (!Mario_script.InAir)
            EnemyAbstract_script.stompsInARow = 0;

        if (Mario_script.GameOver)
        {
            GameOver();
            EnemyAbstract_script.GlobalFreezMovement = true;
        }

        if (Mario_script.MarioInCastle)
            restart = true;


        if (Flag_Script.FlagTouched || Mario_script.MarioInCastle)
        {
            EnemyAbstract_script.GlobalFreezMovement = true;
        }


        if (setBlackScreen)
            SetBlackScreen();

        Pause();

        Scoore();

        TimeCounting();


        if (Castle_script.flagRaised || MushroomCitizen_script.messageShown)
            LoadNewLevel();

    }

    // Display of scoore and collected coins
    void Scoore()
    {
        scoore = Scoore_script.TotalScoore.ToString();
        tempAmountOfNumbers = scoore.Length;
        string tempZeros = zeros;

        if (tempAmountOfNumbers > 1)
            tempZeros = zeros.Substring(0, 6 - tempAmountOfNumbers);

        if (scoore == "0")
            ScooreText.text = zeros;
        else
            ScooreText.text = tempZeros + scoore;

        if (changeCoinCount) // Displaying collected coins
        {
            string coinCount = CoinCount.ToString();
            if (coinCount.Length == 2)
                CoinCountText.text = "x" + coinCount;
            else
                CoinCountText.text = "x0" + coinCount;
        }

    }

    // Increases total scoore count
    public static void AddCoin()
    {
        CoinCount++;
        changeCoinCount = true;
    }

    static void Pause()
    {
        if (AudioSource_script.pauseGame)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void TimeCounting()
    {
        if (GameStart && !stop)
            if (!Flag_Script.FlagTouched) // Time freezes once Mario touches the flag, ...
            {
                if (!SceneLoader_script.loading && !Mario_script.GameOver)//... dies or new sceene is loading.
                {
                    if (Bowser_script.BowserFellDown)
                        stop = true;
                    if (second > 0)
                        second -= Time.deltaTime;
                    else
                    {
                        if (!setBlackScreen && GameTimer > 0)
                        {
                            GameTimer--;
                            TimerText.text = GameTimer.ToString();
                            if (!Mario_script.MarioInCastle)
                                second = secondValue;
                            else
                            {
                                second = quickSecondValue; // Time passes really quick at once Mario reached the castle
                                Scoore_script.AddTotalScoore(10);
                            }
                        }
                    }
                }
            }
    }

    // Method is called each time Mario finishes level
    public void LoadNewLevel()
    {
        previousScene = currentScene;

        levelLoaded = true;
        GameTimer = GameTimerValue;
        loadScene = true; // Allows Castle or Mushroom citizen to load new scene

    }

    // Reloading level and reseting variables after Mario dies
    public void GameOver()
    {
        if (Mario_script.PositionY < -3 && !audio.isPlaying) // Method starts working only after Game over Audio stops playing
        {
            if (!executed)
            {
                gameOver = true;
                marioScript.Stop(false);
                if (GameTimer == 0)
                    timeUp = true;
                executed = true;
                GameTimer = GameTimerValue;

                if (!MultiPlayer)
                {
                    if (Mario_script.Lives > 0)
                    {
                        if (currentScene.Contains("Hidden"))
                        {
                            SceneManager.LoadScene(previousScene);
                        }
                        else
                        {
                            SceneManager.LoadScene(currentScene);
                        }
                    }
                    else
                    {
                        setBlackScreen = true;
                        var sss = "Save time: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + "Pts: " + scoore + " Coins: " + CoinCount.ToString();
                        highscoore.Add(sss);
                    }
                }
                else
                {

                    if ((IsMario && SaveLoad.LuigiLives == 0) || (!IsMario && SaveLoad.MarioLives == 0)) // If second player's lives = 0
                    {
                        if (Mario_script.Lives == 0) // If current player also doesn't have any lives game will be reloaded
                        {
                            Mario_script.setAnimations = true;
                            Mario_script.luigi = false;
                            Mario_script.mario = true;
                            reloadGame = true;
                            setBlackScreen = true;
                            var sss = "Save time: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  " + "Pts: " + scoore + " Coins: " + CoinCount.ToString();
                            highscoore.Add(sss);
                            return;
                        }
                        else
                        {
                            SceneManager.LoadScene(currentScene);
                        }
                    }
                    else
                        MultiPlayerSwitch(); // Chanigng character
                }
            }
        }
    }

    // Black screen is shown each time Mario dies or finishes level
    public void SetBlackScreen()
    {
        if (blackScreenDelay > 0)
        {

            blackScreenDelay -= Time.deltaTime;

            setBlackScreenExecuted = true;
            marioScript.Stop(false);
            EnemyAbstract_script.GlobalFreezMovement = true;
            if (noTexts)
            {

                return;
            }
            TimerText.text = " ";
            LivesCountBlackScreen.text = "x " + Mario_script.Lives.ToString();

            if (!gameOver)// Changing level and world count
            {

                if (!firstBlackScreen)
                {
                    int temp = 0;
                    string levelText;
                    string worldText;

                    if (setCurrent)
                    {
                        levelText = currentScene.Substring(4, 1); // Level calculated from current scene name
                        temp = int.Parse(levelText);
                        worldText = currentScene.Substring(1, 1);
                        //   setCurrent = false;
                    }
                    else
                    {
                        levelText = previousScene.Substring(4, 1);  // Level calculated from previous scene name
                        temp = int.Parse(levelText);
                        worldText = previousScene.Substring(1, 1);
                        temp++;
                        if (temp == 5) // Each time Mario finishes level 4, new world is loaded and level count is set to 0.
                        {
                            temp = 1;
                            int temp2 = int.Parse(worldText);
                            temp2++;
                            worldText = temp2.ToString();
                        }
                    }
                    levelText = temp.ToString();
                    LevelCountText.text = worldText + " - " + levelText;
                    LevelCountBlackScreen.text = worldText + " - " + levelText;

                }

                EnableTexts(true);
            }
            else
            {
                if (Mario_script.Lives == 0)
                {
                    GameOverLabel.text = "Game Over";
                    reloadGame = true;
                    GameOverLabel.enabled = true;
                    audioScript.SetGameOverAudio();
                    audio.Play();
                }

                else if (timeUp)
                {
                    GameOverLabel.text = "Time up";
                    GameOverLabel.enabled = true;
                }

                else
                {
                    EnableTexts(true);

                }
            }
        }


        else
        {
            setBlackScreen = false;
            noTexts = false;
            timeUp = false;
            gameOver = false;
            marioScript.Stop(true);
            EnemyAbstract_script.GlobalFreezMovement = false;
            EnableTexts(false);
            GameOverLabel.enabled = false;
            blackScreenDelay = BlackScreenDelay;
            firstBlackScreen = false;
            if (reloadGame)
            {
                SceneManager.LoadScene("W1_L1_OVERWORLD");
            }
        }
    }

    // Saves old character info and loads new one
    public void MultiPlayerSwitch()
    {
        if (IsMario)
        {
            amountOfNumbers = 0;

            SaveLoad.MarioLives = Mario_script.Lives;

            Mario_script.mario = false;
            Mario_script.luigi = true;
            IsMario = false;
            spriteRenderer = LuigiImage;
            SaveLoad.MarioScoore = Scoore_script.TotalScoore;
            SaveLoad.MarioCoins = CoinCount;
            if (currentScene.Contains("Hidden"))
            {
                SaveLoad.MarioCheckPoint = new Vector2(Mario_script.PositionX, Mario_script.PositionY);
                SaveLoad.MarioLevel = previousScene;

            }
            else
            {
                SaveLoad.MarioCheckPoint = Mario_script.previousCoordinates;
                SaveLoad.MarioLevel = currentScene;

            }
            string levelText = SaveLoad.LuigiLevel.Substring(4, 1);
            string worldText = SaveLoad.LuigiLevel.Substring(1, 1);
            LevelCountText.text = worldText + "-" + levelText;
            LevelCountBlackScreen.text = worldText + "-" + levelText;
            CoinCount = SaveLoad.LuigiCoins;
            changeCoinCount = true;
            Scoore_script.TotalScoore = SaveLoad.LuigiScoore;
            currentPlayerPosition = SaveLoad.LuigiCheckPoint;
            Mario_script.Lives = SaveLoad.LuigiLives;
            SceneManager.LoadScene(SaveLoad.LuigiLevel);


        }
        else
        {
            amountOfNumbers = 0;

            SaveLoad.LuigiLives = Mario_script.Lives;
            Mario_script.mario = true;
            Mario_script.luigi = false;
            IsMario = true;
            spriteRenderer = MarioImage;
            SaveLoad.LuigiLevel = currentScene;
            SaveLoad.LuigiScoore = Scoore_script.TotalScoore;
            SaveLoad.LuigiCoins = CoinCount;

            if (currentScene.Contains("Hidden"))
            {
                SaveLoad.LuigiCheckPoint = new Vector2(Mario_script.PositionX, Mario_script.PositionY);
                SaveLoad.LuigiLevel = previousScene;

            }
            else
            {
                SaveLoad.LuigiCheckPoint = Mario_script.previousCoordinates;
                SaveLoad.LuigiLevel = currentScene;

            }

            string levelText = SaveLoad.MarioLevel.Substring(4, 1);
            string worldText = SaveLoad.MarioLevel.Substring(1, 1);
            LevelCountText.text = worldText + "-" + levelText;
            LevelCountBlackScreen.text = worldText + "-" + levelText;
            CoinCount = SaveLoad.MarioCoins;
            changeCoinCount = true;
            Scoore_script.TotalScoore = SaveLoad.MarioScoore;
            currentPlayerPosition = SaveLoad.MarioCheckPoint;
            Mario_script.Lives = SaveLoad.MarioLives;

            scoore = Scoore_script.TotalScoore.ToString();

            SceneManager.LoadScene(SaveLoad.MarioLevel);

        }
        Mario_script.setAnimations = true;
    }

    // Reseting bools after loading new level
    void Restart()
    {
        Castle_script.flagRaised = false;
        loadScene = false;
        restart = false;
        stop = false;
    }

    // Mario imagas, level,world and Mario's live count. All of it is shown each time Mario finishes the level or dies. If he dies and doesn't have any more lives,
    // or dies because of Game timer reaching 0, instead of these texts, GameOver label is shown.
    void EnableTexts(bool enable)
    {
        spriteRenderer.enabled = enable;

        if (!enable)
        {
            MarioImage.enabled = false;
            LuigiImage.enabled = false;

        }

        WorldLabelBlackScreen.enabled = enable;
        LivesCountBlackScreen.enabled = enable;
        LevelCountBlackScreen.enabled = enable;
    }

    private void OnLevelWasLoaded(int level)
    {
        EnemyAbstract_script.GlobalFreezMovement = false;

        if (levelLoaded || gameOver) // Black screen after loading a new level
            setBlackScreen = true;

        SceneLoader_script.loading = false;
        levelLoaded = false;

        if (blackScreenOnLevelLoaded) // Black screen won't have any texts
        {
            noTexts = true;
            setBlackScreen = true;
            blackScreenOnLevelLoaded = false;
        }

        previousScene = currentScene;

        currentScene = SceneManager.GetActiveScene().name;
        levelLoaded = false;
        //if (restart)
        Restart();
        executed = false;
        if (currentScene.Contains("MultiLevel1"))
            blackScreenOnLevelLoaded = true;

        if (reloadGame)
        {
            GameStart = false;
            gameStratExecuted = false;
            reloadGame = false;
            Scoore_script.TotalScoore = 0;
            CoinCount = 0;
            multiPlayerExecuted = false;
            MultiPlayer = false;
            IsMario = true;
            spriteRenderer = MarioImage;
            Mario_script.Lives = Mario_script.LivesConst;
            Mario_script.luigi = false;
            Mario_script.mario = true;
            Mario_script.setAnimations = true;
            Mario_script.SuperMario = false;
            Mario_script.FireMario = false;
            firstBlackScreen = true;
            setBlackScreen = false;
            SaveLoad.Reload();
            Selector.GetComponent<SpriteRenderer>().enabled = true;
            transform.Find("Image").GetComponent<SpriteRenderer>().enabled = true;
            transform.Find("Player1").GetComponent<Canvas>().enabled = true;
            transform.Find("Player2").GetComponent<Canvas>().enabled = true;

            LevelCountText.text = "1" + " - " + "1";
            LevelCountBlackScreen.text = "1" + " - " + "1";
        }

    }

    void Awake()
    {

        if (Instance)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }
}
