using UnityEngine;
using UnityEngine.SceneManagement;

/*-------------------------------------------
    
              DESCRIPTION
  This script controls main audio source of the game, which is responsible for all
  the background music and sounds(the ones which aren't played by gameobjects' individual audio sources) in the game.

-------------------------------------------*/


public class AudioSource_script : MonoBehaviour
{

    AudioSource audioSource;

    //Bools
    bool OverWorld;
    bool UnderWorld;
    bool GameOver;
    public static bool FlagTouch { get; set; }
    bool StageClear;
    bool gameOverExecuted;
    bool HurryUpPlayed;
    bool invinciblePlayed;
    bool replayExecuted;
    bool movingDownExecuted;
    public static bool pauseGame { get; private set; }

    public AudioClip GameOverSound;
    public AudioClip OverWorldMain;
    public AudioClip flagTouch;
    public AudioClip stageClear;
    public AudioClip HurriedOverworld;
    public AudioClip HurriedUnderworld;
    public AudioClip InvincibleMario;
    public AudioClip HurriedInvincibleMario;
    public AudioClip PauseAudio;
    public AudioClip HurryUp;
    public AudioClip UnderWorldMain;
    public AudioClip Wrap;
    public AudioClip GameOverAudio;
    public AudioClip Castle;
    public AudioClip HurriedCastle;
    public AudioClip CastleClear;
    public AudioClip Sea;
    public AudioClip HurriedSea;
    public AudioClip MenuMusic;


    public AudioClip CurrentClip;
    bool menuSongExecuted;

    static AudioSource_script Instance; // Object created in a first scene. Used to prevent copying of the object in further levels.
    Scene CurrentScene; // Active scene
    bool stopMusic; // Stops music
    float time; // Saves time at which music stoped playing before pausing the game. Allows music to continue playing from where it stoped, after unpausing.
    public AudioClip temporary; // Saves currently playing audio clip before playing
    bool executed;
    bool pauseExecuted;
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Instance = GetComponent<AudioSource_script>();
        CurrentScene = SceneManager.GetActiveScene();
        SetBackgroundSong();
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.setBlackScreen)
        {
            audioSource.Pause();
            stopMusic = true;
            pauseExecuted = false;
        }
        else if (!pauseExecuted)
        {
            pauseExecuted = true;
            audioSource.UnPause();
            stopMusic = false;
        }

        if (!GameOver && !stopMusic) // Prevents music from playing
        {

            if (!StageClear) // Prevents theme songs from playing after Mario touched flag at the end of a level
            {

                GameOver = Mario_script.GameOver;

                Pause();

                if (Bowser_script.BowserFellDown) // Audio is played after MArio defeats boss at the end of any world.
                {
                    audioSource.clip = CastleClear;
                    audioSource.Play();
                    stopMusic = true;
                }


                if (GameControl.GameTimer == 100 && !HurryUpPlayed) // Sound indicating that player is runnig out of time
                {
                    HurryUpPlayed = true;
                    audioSource.clip = HurryUp;
                    audioSource.Play();

                }
                
                if (PanelScript.MenuOpened)
                {
                    if (CurrentClip == null)
                    {
                        CurrentClip = audioSource.clip;
                        audioSource.clip = MenuMusic;
                        audioSource.Play();
                        menuSongExecuted = false;
                    }

                }
                else if(!menuSongExecuted && CurrentClip != null)
                {
                    audioSource.clip = CurrentClip;
                    CurrentClip = null;
                    audioSource.Play();
                    menuSongExecuted = true;
                }

                if (HurryUpPlayed && !audioSource.isPlaying)
                {
                    SetHurriedBackgroundSong();

                    audioSource.Play();
                }

                if (PanelScript.MenuOpened)

                    if (Mario_script.InvincibleMario && !invinciblePlayed)
                    {
                        invinciblePlayed = true; // Mario won't turn invincible more than once in a single level

                        if (GameControl.GameTimer < 100)
                            audioSource.clip = HurriedInvincibleMario;
                        else
                            audioSource.clip = InvincibleMario;
                        audioSource.Play();
                    }




                if (GameOver && !gameOverExecuted)
                {
                    gameOverExecuted = true;
                    audioSource.clip = GameOverSound;
                    audioSource.Play();
                }

                if (invinciblePlayed && !Mario_script.InvincibleMario && !replayExecuted)
                {
                    replayExecuted = true;
                    if (GameControl.GameTimer > 100)
                    {
                        SetBackgroundSong();

                    }
                    else
                    {
                        SetHurriedBackgroundSong();
                    }
                    audioSource.Play();

                }

            }

            if (FlagTouch)
            {
                audioSource.clip = flagTouch;
                audioSource.Play();
                FlagTouch = false;
                StageClear = true;
            }

            if (Mario_script.MovingDown && !movingDownExecuted)
            {
                audioSource.clip = Wrap;
                audioSource.Play();
                movingDownExecuted = true;
                stopMusic = true;

            }

            if (StageClear && !audioSource.isPlaying)
            {
                audioSource.clip = stageClear;
                audioSource.Play();
                StageClear = false;
                stopMusic = true;

            }
        }

    }

    // Reseting variables when loading new level
    public void Restart()
    {
        executed = false;

        gameOverExecuted = false;
        invinciblePlayed = false;
        HurryUpPlayed = false;
        GameOver = false;
        replayExecuted = false;
        movingDownExecuted = false;
        CurrentScene = SceneManager.GetActiveScene();

        if (GameControl.GameTimer < 100)
        {
            SetHurriedBackgroundSong();

        }
        else
        {
            audioSource = GetComponent<AudioSource>();
            SetBackgroundSong();

        }

        stopMusic = false;
        audioSource.Play();

    }

    // Pausing the game
    public void Pause()
    {
        if (Input.GetKeyDown("p"))
        {
            try
            {
                if (!pauseGame)
                {
                    temporary = audioSource.clip;
                    time = audioSource.time;

                    audioSource.clip = PauseAudio;
                    audioSource.Play();
                }
                if (pauseGame)
                {

                    pauseGame = false;
                    audioSource.clip = temporary;
                    audioSource.time = time;
                    audioSource.Play();

                }
                else
                    pauseGame = true;
            }
            catch (System.Exception)
            {

            }
        }
    }


    void SetBackgroundSong()
    {
        Debug.Log("------");
        Debug.Log(audioSource == null);
        Debug.Log(OverWorldMain);
        if (CurrentScene.name.Contains("OVERWORLD"))
            audioSource.clip = OverWorldMain;
        else if (CurrentScene.name.Contains("UNDERWORLD"))
            audioSource.clip = UnderWorldMain;
        else if (CurrentScene.name.Contains("CASTLE"))
            audioSource.clip = Castle;
        else
            audioSource.clip = Sea;
    }

    void SetHurriedBackgroundSong()
    {
        if (CurrentScene.name.Contains("OVERWORLD"))
            audioSource.clip = HurriedOverworld;
        else if (CurrentScene.name.Contains("UNDERWORLD"))
            audioSource.clip = HurriedUnderworld;
        else if (CurrentScene.name.Contains("CASTLE"))
            audioSource.clip = HurriedCastle;
        else
            audioSource.clip = HurriedSea;


    }

    public void SetGameOverAudio()
    {
        if (!executed)
        {
            audioSource.clip = GameOverAudio;
            audioSource.Play();
            executed = true;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Restart();
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
