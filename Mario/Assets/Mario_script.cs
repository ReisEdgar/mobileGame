using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mario_script : MonoBehaviour
{

    //Constants
    const float offsetOnCrouch = 0.6111111f; // Box collider Y offset when crouched
    Vector2 colliderOnCrouch = new Vector2(0.8888f, 1.2222f); // Size of a box collider when Mario crouches
    public const int LivesConst = 3;
    const float SpeedUpValue = 0.15f;
    const float ShortJumpTime = 0.2f;
    const float GameOverTimer = 1.2f;
    const float SlowDownValue = 0.2f;
    const float SuperMarioSizeY = 1.8f;
    const float SuperMarioSizeX = 0.9f;
    const float ColliderOffset = 0.444f;
    const float SuperColliderOffset = 0.9f;
    const int NormalGravity = -12;   // Gravity everywhere apart Under Water Levels
    const int UnderWaterGravity = -4; // Gravity in a Under Water levels
    const int IncreaseCollidersDelay = 60;
    const int DecreaseCollidersDelay = 110;
    const int MaxAmountOfFireBalls = 2;
    const int SpeedValue = 5;
    const int UnderWaterSpeedValue = 3;
    const float TransparencyTimer = 6; // Duration of Mario being transparent and ignoing collision with enemies after being damaged in Super/Fire Mario form
    const float SwimUpTime = 0.6f;     // Indicates how long Mario will move up in an Under Water levels after presing spacebar
    const float InvincibleTimer = 6;   // Indicates how long Mario stays in an Invincible form, after catching a star
    const float SuperTransformationLenght = 1.4f; // Aprox. lenght of animation, when Mario transforms to Super Mario
    const float FireTransformationLenght = 1.9f; // Aprox. lenght of animation, when Super Mario transforms to Fire Mario
    const float TrampoliJumpTimer = 0.25f; // Indicates how long Mario will be moving vertically during tampoline jump
    const float FlagTimerValue = 0.7f;
    //------------------------------------------------

    //Layers
    public LayerMask Ground;
    public LayerMask Bricks;
    public LayerMask QuestionBlock;
    const int EnemyLayer = 15;   //Numeric value of Enemy Layer
    const int MarioLayer = 16;  //Numeric value of Mario Layer
    const int GroundLayer = 8;  //Numeric value of Ground Layer
    const int PipeLayer = 19;  //Numeric value of Pipe Layer

    //------------------------------------------------

    //Transforms   
    Transform groundCheck;  // Child object located below Mario. Used for stomping enemies
    Transform fireBall;     // Fire ball which Mario can throw in a Fire Mario form. It kills majority of enemies
    Transform scoore;
    Transform[] childObjects; // Array of child object transforms. Used to prevent Fire Mario from throwing more than two fire balls at once


    //Bools
    // bool speedUpExecuted;
    public static bool InAir { get; private set; } // Indicates that Mario is in air
    bool Jumping;  // Indicates that Mario performs jump
    public static bool shortJumpBool { get; set; } // Allows Mario to start Short Jump after stomping an enemy
    public static bool SuperMario { get; set; } // Indicates that Mario is in A Super Mario form
    static bool Climb; // Indicates that Mario touched flag pole in the end of level
    public static bool moving { get; private set; }  // Indicates that Mario moves horizontally
    public static bool MarioDamaged { get; private set; } // Indicates that Mario was damaged by enemy or lethal object
    public static bool FireMario { get; set; }  // Indicates that MArio is in a Fire Mario form
    bool SuperMarioDamaged;  // Used to start SuperMarioDamaged animtion (Super/Fire Mario transforms into normal Mario).
    bool transparentMario;   // Indicates that Mario is partly transparent and ignores collisions with enemies, although he still can stomp them
    bool flagTouch;  // Allows flag at the end of a level to move down, after Mario touches it 
    public static bool InvincibleMario { get; private set; } // Indicates that Mario is in an Invincible form.
    public static bool MarioInCastle;

    bool canMoveDown; // Allows Mario to move down the pipe to hidden level
    public static bool MovingDown;// Indicates that Mario is moving down the pipe. When Mario Y position i less than -3, Mario_script ends the game, thinking that Mario fell down from
                                  // the edge. Bool MovingDown prevents this when Mario moves down the pipe.
    bool damageExecuted; // Allows part of a code to be executed only once, after Mario is damaged
    public static bool lowTrampolineJump { get; set; }  // Allows Mario to do low trampoline jump
    public static bool highTrampolineJump { get; set; } // Allows Mario to do high trampoline jump
    bool canShoot; // Indicates that there are less thatn two fire balls in the scene and that Mario can shoot another one
    bool flagExecuted;   // Prevent some part of a code to be executed more than once
    bool transformationExecuted;
    public static bool GameOver { get; private set; }
    bool UnderWater;
    public static bool freezeMovement;// Blocks user input
    public static bool spaceReleased { get; set; }
    bool stopAnimations;
    bool bowserExecuted; // Prevents some part of a code to be run more than once per certain time
    bool loaded;
    bool moveUpExecuted;
    public static bool moveUp;
    public static bool oneDirectionMovement { get; set; } // Allows Mario to move without user input(for example after he walks into castle)
    bool cameraStayExecuted;
    bool changeWhenGrounded;
    public bool stop; // Blocks one direction movement
    public static bool play1Up { get; set; } // Plays 1up audio
    bool skidAnimation;
    bool crouch;
    public static bool mario { get; set; }         //
    public static bool luigi { get; set; }         //   Used to change animations set in multiplayer mode
    public static bool setAnimations { get; set; } //
    bool diedInHidden; // Mario died in a hidden level
    // Floats 

    float speedUp;
    float swimUpTimer;
    public float gameoverTimer;
    public float shortJumpTime;
    public static float InputDirection;
    float trampolineJumpTimer;
    float jumpUpSpeed;
    float doubleSpeedUp;
    float speedValue;
    float SpriteSizeX;
    float SpriteSizeY;
    public static float PositionX { get; private set; }
    public static float PositionY { get; private set; }
    public static float VelocityX { get; private set; }
    public float transparentTimer;
    float falgX;
    float invincibleTimer;
    float flagTimer;
    float animationDelay;
    float moveDownPosX;
    float spriteSizeY;
    float aerialMovementDirection;
    float moveDownPointX;
    float bubbleDelay;


    // Audio
    AudioSource audioSource;
    public AudioClip jump;
    public AudioClip powerUp;
    public AudioClip enemyStomp;
    public AudioClip coin;
    public AudioClip plusLive;

    //------------------------------------------------

    Button Down;
    Button Right;
    Button Left;
    Button S;
    Button X;
    Button Y;

    PhysicsMaterial2D material;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    EdgeCollider2D edgeCollider;
    Animator myAnimator;
    Rigidbody2D Mario;
    GameObject mushroom;
    Scoore_script scooreScript;
    EdgeCollider2D groundCollisionEdge;
    public static int spriteDirection { get; private set; }
    Rigidbody2D Bubble;

    public static int Lives { get; set; }
    int childCount;
    int Gravity;
    int direction;
    public static Vector2 CheckPoint { get; set; }
    public static Mario_script Instance;

    Vector2[] CheckPoints;

    public static bool restart { get; set; }
    public static Vector2 previousCoordinates;
    FireBall_Script fireScript;
    string currentScene;
    string previousScene;
    void Start()
    {

    

        currentScene = "W1_L1_OVERWORLD";
        previousScene = currentScene;

        CheckPoints = new Vector2[32];

        CheckPoints[0] = new Vector2(86, 0);
        CheckPoints[1] = new Vector2(83, 0);
        CheckPoints[2] = new Vector2(76, 5);
        CheckPoints[3] = new Vector2(87, 0);
        CheckPoints[4] = new Vector2(127, 0);
        CheckPoints[5] = new Vector2(120, 0);
        CheckPoints[6] = new Vector2(123, 1.5f);
        CheckPoints[7] = new Vector2(92, 0);
        CheckPoints[8] = new Vector2(122, 5);
        CheckPoints[9] = new Vector2(128.5f, 2);
        CheckPoints[10] = new Vector2(83, 8);
        CheckPoints[11] = new Vector2(118.5f, 0);
        CheckPoints[12] = new Vector2(100, 0);
        CheckPoints[13] = new Vector2(113, 5);
        CheckPoints[14] = new Vector2(68.5f, 9);
        CheckPoints[15] = new Vector2(94, 0);

        CheckPoints[16] = new Vector2(113.5f, 3);
        CheckPoints[17] = new Vector2(127.5f, 3);
        CheckPoints[18] = new Vector2(76.5f, 5);
        CheckPoints[19] = new Vector2(75, 4);


        CheckPoints[20] = new Vector2(116, 0);
        CheckPoints[21] = new Vector2(103, 0);
        CheckPoints[22] = new Vector2(104, 0);
        CheckPoints[23] = new Vector2(82, 0);
        CheckPoints[24] = new Vector2(101.5f, 4);
        CheckPoints[25] = new Vector2(110, 0);
        CheckPoints[26] = new Vector2(130, 2);
        CheckPoints[27] = new Vector2(188.5f, 2);
        CheckPoints[28] = new Vector2(158, 0);
        CheckPoints[29] = new Vector2(114.5f, 2);
        CheckPoints[30] = new Vector2(120, 4);
        CheckPoints[31] = new Vector2(143, 5);



        aerialMovementDirection = 1;
        Lives = LivesConst;

        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        myAnimator = GetComponent<Animator>();
        Mario = GetComponent<Rigidbody2D>();

        Bubble = transform.Find("Bubble").GetComponent<Rigidbody2D>();
        scoore = transform.Find("Scoore");
        groundCheck = transform.Find("GroundCollision");
        fireBall = transform.Find("FireBall");

        groundCollisionEdge = groundCheck.GetComponent<EdgeCollider2D>();
        scooreScript = GetComponentInChildren<Scoore_script>();
        childCount = transform.GetComponentsInChildren<Transform>().Length;



        transparentTimer = TransparencyTimer;
        flagTimer = FlagTimerValue;
        invincibleTimer = InvincibleTimer;
        speedValue = SpeedValue;
        trampolineJumpTimer = TrampoliJumpTimer;
        speedUp = SpeedUpValue;
        doubleSpeedUp = SpeedUpValue * 2;
        shortJumpTime = ShortJumpTime;

        SpriteSizeX = spriteRenderer.sprite.bounds.size.x;
        SpriteSizeY = spriteRenderer.sprite.bounds.size.y;
        scooreScript.parent = transform;
        Gravity = NormalGravity;
        CheckPoint = Vector2.zero;
        setAnimations = true;
        mario = true;
        luigi = false;
        fireScript = fireBall.GetComponent<FireBall_Script>();

       
     ///   SuperMario = true;
      ///  FireMario = true;
    }

    void FixedUpdate()
    {
        if (setAnimations)
        {
            SetAnimations();
            setAnimations = false;
        }

        if (restart)
            Restart();

        if (play1Up)
            Play1Up();

        DontDestroyOnLoad(gameObject);
        Instance = this;
        Deparent();  // Deparents Mario from movement platform

        GroundingCheck(); // Checks if MArio is grounded

        // Crouching
        if (MyButtons.DownDown  && !freezeMovement && SuperMario)
        {
            crouch = true;
            boxCollider.size = colliderOnCrouch;
            boxCollider.offset = new Vector2(0, offsetOnCrouch);

            edgeCollider.enabled = false;
        }
        else
        {
            if (crouch)
            {
                boxCollider.size = new Vector2(SuperMarioSizeX, SuperMarioSizeY);
                boxCollider.offset = new Vector2(0, SuperColliderOffset);
                crouch = false;
                edgeCollider.enabled = true;
            }
        }

        if (Camera_script.cameraPositionX - PositionX > 8 && Camera_script.cameraPositionX - PositionX < 13) // Prevents Mario from going back beyond camera rendering area
        {
            if (!GameControl.setBlackScreen)
            {
                Mario.velocity = new Vector2(0, Gravity);
                transform.position = new Vector2(Camera_script.cameraPositionX - 8, transform.position.y);
            }

        }

        if (GameControl.GameTimer <= 0 && !MarioInCastle) // If timer reached 0 and Mario is not in castle, he loses a live
        {
            MarioDamaged = true;
            transform.parent = null;
            Mario.isKinematic = false;
        }
        if (Bowser_script.BowserFellDown && !bowserExecuted) // If in any of the castles(final level of each world), Bowser(level boss) fell down in fire, Mario will 
        {
            oneDirectionMovement = true;  // move straight until he reaches Mushroom citizen.
            bowserExecuted = true;
        }

        if (oneDirectionMovement && !stop)
            OneDirectionMovement();

        if (moveUp)  //Moves Mario up the pipe from UNDERWORLD levels.
            MoveUp();

        //   loaded = true;
        VelocityX = Mario.velocity.x;

        if (canMoveDown)         // Moves Mario down the pipe to UNDERWORLD levels.
            StartMovingDown();

        if (transparentMario)  // Makes Mario transparent and immune to enemies shortly after being damaged in Super/Fire Mario form.
            TransparentMario();

        if (MyButtons.XDown && !freezeMovement)
        {
            if (!InAir) // Doubles horizontal movement speed
            {
                if (!UnderWater)
                    speedValue = SpeedValue * 2;
                else
                    speedValue = UnderWaterSpeedValue * 2;

                speedUp = doubleSpeedUp;
            }
        }
        else // Sets horizontal speed back to normal
        {
            if (InAir)
                changeWhenGrounded = true;
            else
            {
                if (!UnderWater)
                    speedValue = SpeedValue;
                else
                    speedValue = UnderWaterSpeedValue;

                speedUp = SpeedUpValue;
            }
        }

        if (changeWhenGrounded) // Horizontal speed can't be set from double to normal while Mario is in air
        {
            if (!UnderWater)
                speedValue = SpeedValue;
            else
                speedValue = UnderWaterSpeedValue;
            speedUp = SpeedUpValue;

            changeWhenGrounded = false;
        }

        if (FireMario) // Allows Mario to shoot no more than 2 Fireballs at once
        {

            if (transform.childCount < childCount + MaxAmountOfFireBalls)
                canShoot = true;
            else
                canShoot = false;
        }

        //------------------------------------------
        if (Mario.velocity.x != 0)
            moving = true;
        else
            moving = false;

        PositionX = transform.position.x;
        PositionY = transform.position.y;

        if (flagTouch)  // Disables user input and moves Mario to castle after he touches the flag at the end of a level
            FlagDown();

        if (!freezeMovement)
            if (MyButtons.ZDown && FireMario && canShoot)// Shoots FireBall
                fireScript.Fire = true;

        Animations();

        if (!freezeMovement)
        {
            HorizontalMovement();// Movement method MUST be calles before Jump method !!!!

            if (!UnderWater)
                Jump();

            if (UnderWater)
            {
                SwimUp();
                Bubbles();

            }
        }
        if (lowTrampolineJump)
            TrampolineJump();

        Invincible(); // If Mario touched the star, he becomes Invincible (all the enemies perform flip on collision with Mario)

        Gameover();

        if (shortJumpBool) //Short jump after stomping an enemy
            ShortJump();

    }

    // Blocks user input and freezes Mario in a current position. If bool setGravity is TRUE, Mario is "unblocked"
    public void Stop(bool setGravity)
    {
        freezeMovement = true;
        stop = true;
        Mario.velocity = Vector2.zero;
        Gravity = 0;
        if (setGravity)
        {
            freezeMovement = false;
            stop = false;
            if (UnderWater)
                Gravity = UnderWaterGravity;
            else
                Gravity = NormalGravity;
        }
    }

    // Method is used when Mario has to walk to a single direction with user input disabled (example: after finishing level, he walks to the castle automatically)
    void OneDirectionMovement()
    {
        if (!freezeMovement)
        {
            freezeMovement = true;
            Mario.gravityScale = 1;
        }
        Mario.velocity = new Vector2(speedValue * 0.8f, -10);

    }

    // Method is called when Mario touches the flag at the end of a level
    void FlagDown()
    {
        freezeMovement = true;
        Mario.velocity = new Vector2(0, Gravity);

        if (Flag_Script.flagDown && !flagExecuted) // When flag goes down... 
        {
            transform.localScale = new Vector2(-1, 1);  //... Mario turns around ..
            transform.position = new Vector3(transform.position.x + Mathf.Abs(transform.position.x - falgX) * 2, transform.position.y, 0); //.. and appears at the
            flagExecuted = true;                                                                                                      // other side of a flag pole
        }
        if (flagExecuted)
        {
            if (flagTimer <= 0) // Short delay before Mario goes to the castle
            {
                transform.localScale = new Vector2(1, 1);
                oneDirectionMovement = true;
                flagTouch = false;
            }
            else
                flagTimer -= Time.deltaTime;
        }
    }

    // When Mario is damaged while in Super/Fire Mario form, he becomes partly transparent and ignores collisions with enemies for a certain time 
    void TransparentMario()
    {

        Physics2D.IgnoreLayerCollision(EnemyLayer, MarioLayer);



        spriteRenderer.color = new Color(1, 1, 1, 0.65f);
        transparentTimer -= Time.deltaTime;

        if (transparentTimer <= 0)
        {
            transparentTimer = TransparencyTimer;
            transparentMario = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);
            SuperMarioDamaged = false;
            Physics2D.IgnoreLayerCollision(EnemyLayer, MarioLayer, false);
        }
    }

    // Resizes box collider and changes location of upper edge collider when Mario change his form into Super Mario and vice versa
    void ResizeColliders()
    {
        if (SuperMario)
        {
            Vector2[] newColliderPoints = new Vector2[2];
            newColliderPoints[0] = new Vector2(edgeCollider.points[0].x, 1.9f);
            newColliderPoints[1] = new Vector2(edgeCollider.points[1].x, 1.9f);

            edgeCollider.points = newColliderPoints;

            boxCollider.size = new Vector2(SuperMarioSizeX, SuperMarioSizeY);
            boxCollider.offset = new Vector2(0, SuperColliderOffset);
        }
        else
        {
            Vector2[] newColliderPoints = new Vector2[2];
            newColliderPoints[0] = new Vector2(edgeCollider.points[0].x, 0.9f);
            newColliderPoints[1] = new Vector2(edgeCollider.points[1].x, 0.9f);

            edgeCollider.points = newColliderPoints;

            boxCollider.size = new Vector2(SpriteSizeX, SpriteSizeY);
            boxCollider.offset = new Vector2(0, ColliderOffset);

        }
    }

    // Method checks if Mario hasn't died and takes certain actions if he did
    void Gameover()
    {
        if (transform.position.y < -3 && !MovingDown) // Cheking if Mario hasn't fell down
        {
            MarioDamaged = true;
            Mario.velocity = Vector2.zero;

        }

        if ((MarioDamaged && !damageExecuted))
        {
            if (currentScene.Contains("Hidden"))
                diedInHidden = true;

            Lives--;
            freezeMovement = true;
            boxCollider.enabled = false;
            edgeCollider.enabled = false;
            groundCollisionEdge.enabled = false;
            damageExecuted = true;
            if (transform.position.y > -3)
                gameoverTimer = GameOverTimer; // Prevents Mario from making a jump if he died because of falling down
            SuperMario = false;
            Mario.mass = 10;
            InvincibleMario = false;
            invincibleTimer = InvincibleTimer; FireMario = false;
            GameOver = true;
            ResizeColliders();
        }

        if (gameoverTimer > 0)   // When Mario dies, he makes a jump up and then falls down
        {
            if (gameoverTimer > 0.8)
                Mario.velocity = new Vector2(0, 0);
            else if (gameoverTimer > 0.7)
                Mario.velocity = new Vector2(0, 12);
            else if (gameoverTimer > 0.6)
                Mario.velocity = new Vector2(0, 10);
            else if (gameoverTimer > 0.5)
                Mario.velocity = new Vector2(0, 7);
            else if (gameoverTimer > 0.4)
                Mario.velocity = new Vector2(0, 5);
            else if (gameoverTimer > 0.3)
                Mario.velocity = new Vector2(0, 3);
            else if (gameoverTimer > 0.25)
                Mario.velocity = new Vector2(0, -3);
            else if (gameoverTimer > 0.2)
                Mario.velocity = new Vector2(0, -7);
            else if (gameoverTimer > 0.15)
                Mario.velocity = new Vector2(0, -11);
            else if (gameoverTimer > 0.1)
            {
                Mario.velocity = new Vector2(0, -16);
                mario = false;
                luigi = false;
            }
            gameoverTimer -= Time.deltaTime;
        }

    }

    void HorizontalMovement()
    {
        if (!freezeMovement)
        {
            if (MyButtons.LeftDown)
            {
                InputDirection = -1;
            }else if (MyButtons.RightDown)
            {
                InputDirection = 1;
            }
            else
            {
                InputDirection = 0;
            }

            MovementOnAPlatform();  // Method will do something only if Mario is standing on a Moving platform

            if (!InAir)
            {
                if (MyButtons.RightDown)                         
                    direction = 1;  // Allows to modify ONLY movement direction (InputDirection value changes from 0 to 1/-1 gradually,
                else if (MyButtons.LeftDown) // and modifies movement velocity ) 
                    direction = -1;

                NormalizeLocalScale();
                GroundedMovement();

            }
            else // In air Mario can speed up, but can't slow down
            {
                if (UnderWater)
                {
                    if (InputDirection > 0)
                        direction = 1;  // Allows to modify ONLY movement direction (InputDirection value changes from 0 to 1/-1 gradually,
                    else             // and modifies movement velocity ) 
                        direction = -1;

                    NormalizeLocalScale(); // Mario can change local scale in air only in Sea levels

                }

                if (InputDirection != 0) // In air Mario can't change his movement direction
                {
                    if (!shortJumpBool)
                    {
                        if (Mathf.Sign(InputDirection) == Mathf.Sign(direction))
                        {
                            if (Mathf.Abs(Mario.velocity.x) < speedValue)
                                SpeedUp();
                            else
                                Mario.velocity = new Vector2(direction * speedValue, Gravity);
                        }
                        else
                            SlowDown(false);
                    }
                }
                else
                {
                    //SlowDown(false);
                    Mario.velocity = new Vector2(Mario.velocity.x, Gravity); // Provides gravity even when there is no user input
                }
            }
        }

    }

    // Horizontal movement on a ground
    void GroundedMovement()
    {
        if (MyButtons.RightDown || MyButtons.LeftDown) // Works faster than if(InoutDirection != 0)
        {
            if (Mathf.Sign(spriteDirection) == Mathf.Sign(direction) || Mario.velocity.x == 0)
            {
                if (Mathf.Abs(Mario.velocity.x) < speedValue)
                    SpeedUp();
                else
                    Mario.velocity = new Vector2(direction * speedValue, Gravity);
                skidAnimation = false;

            }
            else  // If Mario is moving, and user tries to change his movement direction, Mario will slow down faster than usual
            {
               
                SlowDown(true);
            }
        }
        else
        {
            if (GameControl.EasyMode)
            {
                Mario.velocity = new Vector2(0, Mario.velocity.y);
            }
            else
            {
                SlowDown(false);
            }            
        }
    }

    // Changes local scale of Mario and scoore depending on a user input
    void NormalizeLocalScale()
    {
        if (UnderWater ? (InputDirection > 0) : (Mario.velocity.x > 0))
        {
            transform.localScale = new Vector2(1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(1, 1);
            spriteDirection = 1;
        }
        if (UnderWater ? (InputDirection < 0) : (Mario.velocity.x < 0))
        {
            transform.localScale = new Vector2(-1, 1);
            if (scooreScript.renderTimer <= 0)
                scoore.localScale = new Vector2(-1, 1);
            spriteDirection = -1;
        }
    }

    // If Mario stands on a Moving platform, he is parented by it and moves by changing his transform position. It allows him to move together with the 
    // platform and walk normaly on it. Whenever he stops touching platform, he is deparented.
    void MovementOnAPlatform()
    {
        if (transform.parent != null)
        {
            if (MyButtons.RightDown)
                transform.localPosition = new Vector2(transform.localPosition.x + 0.03f, transform.localPosition.y);
            else if (MyButtons.LeftDown)
                transform.localPosition = new Vector2(transform.localPosition.x - 0.03f, transform.localPosition.y);
        }
    }

    // If Mario cacthes a star, he becomes invincible. He will is immune to enemies and flips them on touch.
    void Invincible()
    {
        if (InvincibleMario)
        {
            Mario.mass = 1000000;// Mass is increased, in order to allow him not to slow down after each collision with enemy.

            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                Mario.mass = 10;
                InvincibleMario = false;
                invincibleTimer = InvincibleTimer;
            }
        }
    }

    // Speeds up Mario to it's max horiznotal velocity. 
    void SpeedUp()
    {
        int direction;
        if (InputDirection > 0)
            direction = 1;
        else
            direction = -1;

        if (Mario.velocity.x == 0)
        {
            Mario.velocity = new Vector2(direction, Gravity);
        }
        else if (Mario.velocity.x < speedValue)
        {
            Mario.velocity = new Vector2(direction * (Mathf.Abs(Mario.velocity.x) + speedUp), Gravity);
        }
    }

    // Slows Mario down when he is moving without user input or when user wants to change movement direction
    // Bool skid indicates, that Mario is changing movement direction and will slow down twice faster than usual
    void SlowDown(bool skid)
    {
        if (Mathf.Abs(Mario.velocity.x) > 0)
        {
            if (skid)
            {
                Mario.velocity = new Vector2(1 * Mathf.Sign(Mario.velocity.x) * (Mathf.Abs(Mario.velocity.x) - SlowDownValue * 2), Gravity);
                skidAnimation = true;

            }
            else
            {
                Mario.velocity = new Vector2(1 * Mathf.Sign(Mario.velocity.x) * (Mathf.Abs(Mario.velocity.x) - (InAir ? SlowDownValue / 2 : SlowDownValue)), Gravity);
                skidAnimation = false;
            }
        }

        if (Mathf.Abs(Mario.velocity.x) < 2f) // /Completely stops Mario and prevents him from starting to move in an opposite direction
        {
            Mario.velocity = new Vector2(0, Gravity);
            skidAnimation = false;

        }

    }

    // Ths method is used instead of Jump() in a Sea levels
    void SwimUp()
    {
        if (MyButtons.SDown)
            swimUpTimer = SwimUpTime;

        float velocity; // Horizontal velocity during a swim up

        if ((Mario.velocity.x > 0 && spriteDirection > 0) || (Mario.velocity.x < 0 && spriteDirection < 0))
            velocity = Mario.velocity.x; // While being in air in Sea levels, Mario can change spriteDirection without changing movement direction...
        else                             //... In this case, his velocity will be set to 0 during a swim up, if user wont press arrow key to move horizontally.
            velocity = 0;

        if (swimUpTimer > 0) // Control of Y velocity during swim up
        {
            if (swimUpTimer > 0.4)
                Mario.velocity = new Vector2(velocity, 5);
            else if (swimUpTimer > 0.3)
                Mario.velocity = new Vector2(velocity, 4);
            else if (swimUpTimer > 0.24)
                Mario.velocity = new Vector2(velocity, 3);
            else if (swimUpTimer > 0.17)
                Mario.velocity = new Vector2(velocity, 2);
            else if (swimUpTimer > 0)
                Mario.velocity = new Vector2(velocity, 1);
            else
                Mario.velocity = new Vector2(velocity, Gravity); // Pushes MArio down, if after swim up, user doesn't press any keys


            swimUpTimer -= Time.deltaTime;

        }
    }

    // Deparents Mario from moving platform
    void Deparent()
    {
        if (transform.parent != null)
        {
            if (InAir) // Deparenting when Mario walks off the platform (ground check stops intersecting with platform when Mario stands on the edge).
            {

                transform.parent = null;
                Mario.isKinematic = false;

            }
            else if (MyButtons.SDown)  // Deparenting on a jump
            {

                transform.parent = null;
                Mario.isKinematic = false;

            }
        }
    }

    // Jumping works everywhere, except Sea levels
    void Jump()
    {
         if (MyButtons.SDown)
      //  if(true)
        {
            if (!InAir)
            {

                audioSource.clip = jump;
                audioSource.Play();

                Jumping = true;
                if (InputDirection == 0)
                    Mario.velocity = Vector2.zero;
                jumpUpSpeed = 13;
            }

            if (Jumping)
                Mario.velocity = new Vector2(Mario.velocity.x, jumpUpSpeed -= 0.35f); // Slowing down Y velocity

            if (jumpUpSpeed <= 0)
                Jumping = false;
        }
        if (MyButtons.SUp && jumpUpSpeed != 0) // Stops jump when user realeases space bar
            Jumping = false;

    }

    public static void AddLive()
    {
        Lives++;

    }

    // Short jump is performed after stomping an enemy
    public void ShortJump()
    {
        if (shortJumpTime > 0) // Short jump continues for a 0.2 second
        {
            Mario.velocity = new Vector2(InputDirection == 0 ? spriteDirection * 2 : speedValue * InputDirection, 11);

            shortJumpTime -= Time.deltaTime;

        }
        else
        {
            shortJumpBool = false;
            shortJumpTime = ShortJumpTime;
        }
    }

    // Trampoline allows Mario to jump higher
    // If user doesn't press "space" Mario is still thrown in the air anyway a little
    // This method only controls MArio's jump speed, everything else is done in Trampoline_script
    public void TrampolineJump()
    {
        if (trampolineJumpTimer > 0)
        {
            if (highTrampolineJump) // If Mario jumps right before being thrown up in the air by trampoline, he will be thrown twice higher
                Mario.velocity = new Vector2(Mario.velocity.x, 37);
            else
                Mario.velocity = new Vector2(Mario.velocity.x, 15); // Normal jump without pressing "space"
            trampolineJumpTimer -= Time.deltaTime;
        }
        else   // Reseting variables after jump
        {
            highTrampolineJump = false;
            lowTrampolineJump = false;
            trampolineJumpTimer = TrampoliJumpTimer;
        }

    }

    // Control over all animations
    void Animations()
    {
        if (Mathf.Abs(Mario.velocity.x) <= SpeedValue)
            myAnimator.speed = 2;
        else if (Mathf.Abs(Mario.velocity.x) > SpeedValue) // When Mario runs on a double speed, animations are payed faster
            myAnimator.speed = 3;

        if (SuperMario || FireMario)
        {
            if (!transformationExecuted)
            {
                if (FireMario)
                    animationDelay = FireTransformationLenght;
                else if (SuperMario)
                    animationDelay = SuperTransformationLenght;
                transformationExecuted = true;
            }
            if (animationDelay > 0.2f) // During transformation into Super/Fire Mario, user input is disabled and Mario freezes at his current position
            {
                freezeMovement = true;
                Mario.velocity = Vector2.zero;
                Mario.gravityScale = 0;
                animationDelay -= Time.deltaTime;
            }
            else if (animationDelay <= 0.2f && animationDelay >= 0)
            {
                animationDelay = -1;
                freezeMovement = false;
                Mario.gravityScale = 1;
            }
        }
        if (!oneDirectionMovement)
        {
            if (!stopAnimations)
            {
                myAnimator.SetFloat("Speed", Mathf.Abs(InputDirection));
                myAnimator.SetBool("jumping", InAir);

                myAnimator.SetBool("Sea", UnderWater);

                myAnimator.SetBool("Inair", InAir);

                myAnimator.SetBool("GameOver", MarioDamaged);
                myAnimator.SetBool("Invincible", InvincibleMario);
                myAnimator.SetBool("SuperMario", SuperMario);
                myAnimator.SetBool("FireMario", FireMario);
                myAnimator.SetBool("Climb", Climb);
                myAnimator.SetBool("Skid", skidAnimation);
                myAnimator.SetBool("Crouch", crouch);
                myAnimator.SetBool("SuperMarioDamaged", SuperMarioDamaged);
            }
        }
        else
        {
            myAnimator.SetBool("Climb", false);
            myAnimator.SetFloat("Speed", 1);
            myAnimator.SetBool("jumping", false);
        }
    }

    // Method is used, when Mario Moves up the pipie, while returning from Under world or Sea levels
    void MoveUp()
    {
        if (!moveUpExecuted)
        {

            Mario.gravityScale = 0;

            Physics2D.IgnoreLayerCollision(MarioLayer, GroundLayer); // Allows MArio to move through box collider of the pipe
            Physics2D.IgnoreLayerCollision(MarioLayer, EnemyLayer); // Mario might MoveUp through pipe with piranha plant hidden in it
            spriteRenderer.sortingLayerName = "Behind Objects";
            freezeMovement = true;
            Mario.velocity = new Vector2(0, 4);
            moveUpExecuted = true;
        }
        if (!Physics2D.OverlapPoint(groundCheck.position, Ground) && !Physics2D.OverlapPoint(transform.position, Ground)) // When ground check stops intersecting with pipe, everything is set as it was, before moving up
        {
            Physics2D.IgnoreLayerCollision(MarioLayer, GroundLayer, false);
            Physics2D.IgnoreLayerCollision(MarioLayer, EnemyLayer, false);
            spriteRenderer.sortingLayerName = "Default";

            Mario.velocity = Vector2.zero;
            freezeMovement = false;
            moveUp = false;
            moveUpExecuted = false;
        }
    }

    // Moving down the pipe to the hidden level
    void StartMovingDown()
    {
        if (!InAir && Mathf.Abs(moveDownPointX - transform.position.x) < 0.2) // If Mario is standing next to (or directly on) the center of a pipe leading to hidden level
            if (MyButtons.DownDown && !freezeMovement)
            {
                previousCoordinates = transform.position;
                spriteRenderer.sortingLayerName = "Behind Objects";

                Mario.gravityScale = 0;
                Physics2D.IgnoreLayerCollision(MarioLayer, EnemyLayer); // There might be piranha plant hidden in the pipe
                Physics2D.IgnoreLayerCollision(MarioLayer, GroundLayer);

                freezeMovement = true;
                Mario.velocity = new Vector2(0, -5);
                MovingDown = true;
                canMoveDown = false;
            }
    }

    // Cheking if Mario is damaged
    void DamageCheck()
    {
        if(!GameControl.EasyMode)
        { 
        if (SuperMario || FireMario)
        {
            if (!InvincibleMario) // Invincible Mario doesn't take any damage
            {
                SuperMario = false;
                FireMario = false;
                SuperMarioDamaged = true; // Is used to change animation
                transparentMario = true;
                ResizeColliders(); // Collider size must be decreased, in order to suround small Mario
            }
        }
        else if (!transparentMario && !InvincibleMario)
            MarioDamaged = true;
    }
    }

    // Cheking if Mario is grounded
    void GroundingCheck()
    {
        if (Physics2D.OverlapPoint(groundCheck.position, Ground) || Physics2D.OverlapPoint(groundCheck.position, Bricks) || Physics2D.OverlapPoint(groundCheck.position, QuestionBlock))
        {
            InAir = false;
            Jumping = false;
        }
        else
            InAir = true;

    }

    void Play1Up()
    {
        audioSource.clip = plusLive;
        audioSource.Play();
        play1Up = false;
    }

    void SetCheckpoint(string currentScene)
    {
        if (GameOver)
        {
            if (!currentScene.Contains("Multi") && !currentScene.Contains("Hidden"))
            {
                if (!GameControl.MultiPlayer)
                {
                    int world = int.Parse(currentScene.Substring(1, 1));
                    int level = int.Parse(currentScene.Substring(4, 1));

                    Vector2 checkPoint = CheckPoints[4 * (world - 1) + level - 1];

                    if (transform.position.x < checkPoint.x)
                        CheckPoint = Vector2.zero;
                    else
                        CheckPoint = checkPoint;

                    if (currentScene.Contains("CASTLE") && CheckPoint == Vector2.zero)
                        transform.position = new Vector2(0, 6);
                    else
                        transform.position = CheckPoint;

                }
                else
                {
                    int world = int.Parse(SaveLoad.GetLevel(GameControl.IsMario).Substring(1, 1));
                    int level = int.Parse(SaveLoad.GetLevel(GameControl.IsMario).Substring(4, 1));

                    Vector2 checkPoint = CheckPoints[4 * (world - 1) + level - 1];

                    if (SaveLoad.GetCoordinates(GameControl.IsMario).x < checkPoint.x)
                        CheckPoint = Vector2.zero;
                    else
                        CheckPoint = checkPoint;

                    if (currentScene.Contains("CASTLE") && CheckPoint == Vector2.zero)
                        transform.position = new Vector2(0, 6);
                    else
                        transform.position = CheckPoint;
                }
            }
            else if (currentScene.Contains("Multi"))
                transform.position = Vector2.zero;
            //   try
            //   {
            if (diedInHidden) // If Mario died in a Hidden level
            {
                int world = int.Parse(currentScene.Substring(1, 1));
                int level = int.Parse(currentScene.Substring(4, 1));

                Vector2 checkPoint = CheckPoints[4 * (world - 1) + level - 1];

                if (previousCoordinates.x < checkPoint.x)
                    CheckPoint = Vector2.zero;
                else
                    CheckPoint = checkPoint;

                transform.position = CheckPoint;

            }
            //  }
            //     catch (System.Exception)
            //    { }

        }
        else
            transform.position = CheckPoint;

    }

    // Cahnges animations set in multiplayer
    void SetAnimations()
    {
        myAnimator.SetBool("Mario", mario);
        myAnimator.SetBool("Luigi", luigi);

    }

    // Resets variables
    public void Restart()
    {
        if (currentScene != null)
            previousScene = currentScene;
        stopAnimations = false; ;
        bowserExecuted = false;
        oneDirectionMovement = false;
        restart = false;
        Climb = false;
        Physics2D.IgnoreLayerCollision(MarioLayer, GroundLayer, false);
        MovingDown = false;
        spriteRenderer.sortingLayerName = "Default";
        Mario.velocity = Vector2.zero;
        if (!GameControl.reloadGame)
            freezeMovement = false;

        boxCollider.enabled = true;
        edgeCollider.enabled = true;
        groundCollisionEdge.enabled = true;
        damageExecuted = false;
        flagExecuted = false;
        flagTouch = false;
        flagTimer = FlagTimerValue;
        MarioInCastle = false;
        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.Equals("MultiLevel1_OVERWORLD"))
            oneDirectionMovement = true;
        else
            oneDirectionMovement = false;

        SetCheckpoint(currentScene);

        if (SceneManager.GetActiveScene().name.Contains("SEA"))
        {
            UnderWater = true;
            Mario.gravityScale = 0;
            Gravity = UnderWaterGravity;
            speedValue = UnderWaterSpeedValue;
        }
        else
        {
            speedValue = SpeedValue;
            Gravity = NormalGravity;
            UnderWater = false;
            Mario.gravityScale = 1;
        }
        if (GameControl.EasyMode)
        {
            Lives = LivesConst * 10;
                SuperMario = true;
                ResizeColliders();
                FireMario = true;
        }
        GameObject.Find("Camera").GetComponent<Camera_script>().Restart();

        diedInHidden = false;
        MarioDamaged = false;
        GameOver = false;
    }

    // In Sea levels Mario blows bubbles
    void Bubbles()
    {
        if (bubbleDelay < 0)
        {
            Bubble.gravityScale = -0.2f;
            Instantiate(Bubble, new Vector2(transform.position.x, transform.position.y + 0.5f), new Quaternion());
            Bubble.gravityScale = 0;
            float random = Random.value; // Random delay before another bubble
            if (random < 0.2)
                bubbleDelay = 0.7f;
            else if (random < 0.6f)
                bubbleDelay = 1.2f;
            else if (random < 0.9f)
                bubbleDelay = 1.7f;
            else
                bubbleDelay = 0.3f;

        }
        else
            bubbleDelay -= Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lethal object")
        {
            DamageCheck();
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (!moveUp)
                DamageCheck();
        }
        else if (collision.gameObject.tag == "Coin")
        {
            audioSource.clip = coin;
            audioSource.Play();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tree") // Climbing up the tree to the
        {
            freezeMovement = true;
            Climb = true;
            if (MyButtons.UpDown)
                Mario.velocity = new Vector2(0, 2);
            else
                Mario.velocity = Vector2.zero;

        }
        else if (collision.gameObject.tag == "TreeTop") // When appearing in a hidden Sky level, Mario is raised above clouds on a growing tree (parent of Mario).
        {
            if (transform.position.y < 4)
            {
                stopAnimations = true;
                myAnimator.SetFloat("Speed", 0);
                myAnimator.SetBool("jumping", false);
                freezeMovement = true;
                Mario.isKinematic = true;
                transform.parent = collision.transform;
                Physics2D.IgnoreLayerCollision(MarioLayer, GroundLayer);
            }
            else
            {
                stopAnimations = false;
                freezeMovement = false;
                Mario.isKinematic = false;
                transform.parent = null;
                Physics2D.IgnoreLayerCollision(MarioLayer, GroundLayer, false);

            }
        }

        else if (collision.gameObject.tag == "MovingPlatform")
        {
            if (!(Jumping)) //Prevents Mario from being parent by platform, if he would touch platform with his head while jumping
            {
                Mario.isKinematic = true;           // Allows Mario to move together with the platform
                transform.parent = collision.gameObject.transform;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "EdgeCollider")
        {
            if (!(Physics2D.OverlapPoint(groundCheck.position, Ground) || Physics2D.OverlapPoint(groundCheck.position, Bricks) || Physics2D.OverlapPoint(groundCheck.position, QuestionBlock)))
            {
                Physics2D.IgnoreCollision(boxCollider, collision.collider);
                Physics2D.IgnoreCollision(edgeCollider, collision.collider);

            }
        }

        if (collision.otherCollider is EdgeCollider2D) // When Mario touches something with his head, he stops jumping
            Jumping = false;

        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case "Lethal object":
                {
                    if (collision.otherCollider is BoxCollider2D)
                        DamageCheck();
                    break;
                }
            case "Castle":
                {
                    MarioInCastle = true;
                    break;
                }
            case "Flag":
                {
                    falgX = collision.transform.position.x;
                    Climb = true;
                    flagTouch = true;
                    break;
                }
            case "Mushroom":
                {
                    if (mushroom == null || !collision.gameObject.Equals(mushroom)) // Sometimes unity might call on collision enter few times,
                    {                                                               // during collision with same mushroom

                        bool is1Up = collision.gameObject.GetComponent<Mushroom_Script>().Is1Up;

                        if (!is1Up) // 1Up mushroom adds live, others tranform Mario to Super/Fire Mario
                        {
                            audioSource.clip = powerUp;
                            audioSource.Play();

                            scooreScript.SetScoore("1000"); // Mario controls Mushroom's scoore, because Mushroom's audio source would be destroyed with Mushroom
                            if (!FireMario)
                                transformationExecuted = false;
                            if (!SuperMario)
                            {
                                SuperMario = true;
                                ResizeColliders();
                            }
                            else
                                FireMario = true;
                        }
                        else
                        {
                            scooreScript.SetScoore("1Up");
                            audioSource.clip = plusLive;
                            audioSource.Play();
                            Lives++;
                        }
                        mushroom = collision.gameObject;
                    }
                    break;
                }
            case "ShellSlide": // When Koopa does shell slide, his tag is changed
                {
                    if (collision.otherCollider is BoxCollider2D) // Stomping Koopa won't damage Mario
                    {
                        if (collision.gameObject.GetComponent<KoopaTroopa_Script>().shellSlideCount > 0) // Prevents Mario from being damaged by Koopa, 
                            DamageCheck();                                                               // when pushing him before shell slide.
                    }
                    break;
                }
            case "Enemy":
                {
                    if (collision.otherCollider is BoxCollider2D && !collision.gameObject.GetComponent<EnemyAbstract_script>().Damaged)
                        DamageCheck();
                    break;
                }
            case "Axe":
                {
                    freezeMovement = true;
                    Mario.gravityScale = 0; // Mario freezes in air while bowsr is falling down into lava
                    InputDirection = 0;
                    Mario.velocity = Vector2.zero;
                    break;
                }

            case "Star":
                {
                    scooreScript.SetScoore("1000");
                    InvincibleMario = true;
                    Destroy(collision.gameObject);
                    break;
                }

            case "SceneLoader":
                {
                    MovingDown = true; // Allows Audio_script to play Wrap music. Doesn't modify movement.
                    freezeMovement = true;
                    break;
                }

            case "MoveDown":
                {
                    moveDownPointX = collision.transform.position.x;
                    canMoveDown = true;
                    break;
                }
            case "Mushroom citizen":
                {
                    stopAnimations = true;
                    oneDirectionMovement = false;
                    myAnimator.SetFloat("Speed", 0);
                    myAnimator.SetBool("jumping", false);
                    break;
                }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MoveDown")
            canMoveDown = false;

    }

    private void OnLevelWasLoaded(int level)
    {
        Restart(); //Resets variables
    }

    void Awake()
    {
        if (Instance)  // Prevents creation of second Mario while loading level
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }
}