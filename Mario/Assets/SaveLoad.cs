using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class is used to save info about characters, when user plays multiplayer mode

public class SaveLoad : MonoBehaviour {

    public static int MarioScoore { get; set; }
    public static int MarioCoins { get; set; }
    public static string MarioLevel { get; set; }
    public static Vector2 MarioCheckPoint { get; set; }
    public static int MarioLives { get; set; }
    public static int LuigiScoore { get; set; }
    public static int LuigiCoins { get; set; }
    public static string LuigiLevel { get; set; }
    public static Vector2 LuigiCheckPoint { get; set; }
    public static int LuigiLives { get; set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Reload()
    {
        MarioLives = Mario_script.LivesConst;
        MarioScoore = 0;
        MarioCoins = 0;
        MarioCheckPoint =  Vector2.zero;
        MarioLevel = "W1_L1_OVERWORLD";
        LuigiLives = Mario_script.LivesConst;
        LuigiScoore = 0;
        LuigiCoins = 0;
        LuigiCheckPoint = Vector2.zero;
        LuigiLevel = "W1_L1_OVERWORLD";
    }

    public static Vector2 GetCoordinates(bool isMario)
    {
        if (isMario)
            return MarioCheckPoint;
        else
            return LuigiCheckPoint;
                   
    }

    public static string GetLevel(bool isMario)
    {
        if (isMario)
            return MarioLevel;
        else
            return LuigiLevel;
    }
}
