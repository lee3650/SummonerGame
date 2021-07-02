using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    [SerializeField] ManaManager ManaManager;

    List<Summon> Summons = new List<Summon>();

    public void OnSummonDeath(float manaCost)
    {
        ManaManager.IncreaseMaxMana(manaCost);
    }
    
    public void AddSummonToParty(Summon s)
    {
        Summons.Add(s);
    }

    public void RemoveSummonFromParty(Summon s)
    {
        Summons.Remove(s);
    }

    public void MoveSummonsToSummoner()
    {
        foreach (Summon s in Summons)
        {
            s.TryToMoveToSummoner();
        }
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
