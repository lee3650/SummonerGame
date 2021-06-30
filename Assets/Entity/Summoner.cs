using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    [SerializeField] ManaManager ManaManager;
    public void OnSummonDeath(float manaCost)
    {
        ManaManager.IncreaseMaxMana(manaCost);
    }
    
    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
