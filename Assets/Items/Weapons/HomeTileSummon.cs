﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeTileSummon : TileRestrictedSummon
{
    private HealthManager healthManager = null;
    Summoner playerSummoner; 
    public void Initialize(HealthManager hm, Transform player)
    {
        OnPickup(player, player);
        playerSummoner = player.GetComponent<Summoner>();
        healthManager = hm; 
    }

    public override void UseWeapon(Vector2 mousePos)
    {
        GameObject spawnedSummon = Instantiate(Summon, VectorRounder.RoundVector(mousePos), Quaternion.Euler(Vector3.zero));
        spawnedSummon.GetComponent<HomeSummon>().SetHealthManager(healthManager);

        InitializeSpawnedSummon(spawnedSummon.GetComponent<Summon>(), playerSummoner);
    }
    //this is really not a great solution - anything that relies on the health manager on awake is going to break. 
}
