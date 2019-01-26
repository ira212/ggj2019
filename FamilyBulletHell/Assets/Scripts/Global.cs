using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Global : MonoBehaviour
{
    public static Global Instance; //Creates a new instance if one does not yet exist

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject); //makes instance persist across scenes
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); //deletes copies of global which do not need to exist, so right version is used to get info from
        }
    }


    public float childSpawnTimeMin = 2.0f;
    public float childSpawnTimeMax = 15.0f;

    public float bulletSpeedMin = 15.0f;
    public float bulletSpeedMax = 30.0f;

    public float GoalAreaRadMin = 4.0f;
    public float GoalAreaRadMax = 8.0f;

    public float HomeAreaRadMin = 5.0f;
    public float HomeAreaRadMax = 9.0f;

    public int StartHP = 10;
    //1HP happens every X seconds. 1st heal starts at 5 seconds, not 0.
    public float GoalAreaHeal = 5.0f;

    public float ShieldRespawnTime = 8.0f;
    public float ShieldHPmax = 20.0f;

    public float BulletDamageSpeedFactor = 120.0f;
    public float BulletHealChance = 0.2f;
    public float BulletHealAmount = -5.0f;

    //relative sizes
    public float SquareScale = 4.0f;
    public float TriangleScale = 4.0f;
    public float BulletScale = 4.0f;
    public float ChildScale = 2.0f;
   
    //scale factor
    public float SquareSpeed = 1.0f;
    public float TriangleSpeed = 1.0f;
    public float ChildSpeed = 1.2f;

    //after being clicked, how long the target keeps following the player
    public float TriangleAttSpan = 6.0f;
    public float ChildAttenSpanMIn = 2.0f;
    public float ChildAttenSpanMax = 4.0f;

    //number of bullets to spawn at the start of the game
    public int BulletStartSpawnMin = 2;
    public int BulletStartSpawnMax = 5;
    public float NewBulletSpawnMin = 0.5f;
    public float NewBulletSpawnMax = 2.0f;
    // every 1 minute, multiply min/max this factor
    public float SpawnFreqTimeFact = 0.95f;

    public float TriangleArriveTimeMin = 5.0f;
    public float TriangleArriveTimeMax = 10.0f;


    //once both parents are in the Home area, start the timer
    //They must remain in the zone until the timer completes.
    //If only one parent exists, then only 1 parent needs to be in the zone
    public float SecndChildArriveTimeMin = 2.0f;
    public float SecndChildArriveTimeMax = 15.0f;



}
