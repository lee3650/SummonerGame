using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    [SerializeField] ManaManager ManaManager;
    ILivingEntity Entity;

    List<Summon> Summons = new List<Summon>();

    private void Awake()
    {
        Entity = GetComponent<ILivingEntity>();
    }

    public void OnHit(IEntity hit)
    {
        Entity.OnHit(hit);
    }

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

    public void RefundManaFromAllLivingSummons()
    {
        foreach (Summon s in Summons)
        {
            if (s.CanRefundMana())
            {
                ManaManager.IncreaseMaxMana(10f);
                ManaManager.IncreaseMana(10f);
            }
            Destroy(s.gameObject);
        }

        Summons = new List<Summon>();
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
