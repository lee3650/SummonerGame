using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] Summon Summon;
    [SerializeField] float Range = 1.75f;
    [SerializeField] float HealAmount = 2f; 
    [SerializeField] float HealCooldown = 1;
    [SerializeField] TargetManager TargetManager; 
    float timer = 0f; 

    void Update()
    {
        if (HealthManager.IsAlive())
        {
            SetTargetToClosestDamagedWall();

            timer += Time.deltaTime; 
            
            if (timer > HealCooldown)
            {
                timer = 0f;
                HealNearbyWalls();
            }
        }
    }

    void HealNearbyWalls()
    {
        List<Summon> Walls = GetSummonedWalls();

        foreach (Summon s in Walls)
        {
            if (Vector2.Distance(s.transform.position, transform.position) <= Range)
            {
                s.TryHealSummon(HealAmount);
            }
        }
    }

    void SetTargetToClosestDamagedWall()
    {
        //I'm going to remove the TargetSearcher from the builder, I think. 
        //Hopefully that doesn't cause player control issues. 
        List<Summon> Walls = GetSummonedWalls();

        float minDist = Mathf.Infinity;
        Summon closestSummon = null; 

        if (Walls.Count == 0)
        {
            print("No walls were found!");
        }

        foreach (Summon s in Walls)
        {
            if (s.GetIsDamaged())
            {
                float dist = Vector2.Distance(s.transform.position, transform.position);
                if (dist < minDist)
                {
                    closestSummon = s;
                    minDist = dist; 
                }
            }
        }

        if (closestSummon != null)
        {
            TargetManager.Target = closestSummon.GetComponent<ITargetable>(); 
        }
    }

    List<Summon> GetSummonedWalls()
    {
        List<Summon> result = new List<Summon>();

        foreach (Summon s in Summon.GetSummoner().GetSummons())
        {
            if (s.GetSummonType() == SummonType.Wall)
            {
                result.Add(s);
            }
        }
        return result; 
    }

}
