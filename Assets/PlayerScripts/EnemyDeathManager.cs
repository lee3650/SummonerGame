using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathManager : MonoBehaviour
{
    [SerializeField] ManaManager PlayerMana;

    static EnemyDeathManager DeathManager;

    [SerializeField] float ManaIncreaseFromKill = 5f;

    private void Awake()
    {
        DeathManager = this; 
    }

    public void IncreasePlayerMana()
    {
        /*
        PlayerMana.IncreaseMaxMana(ManaIncreaseFromKill);
        PlayerMana.IncreaseMana(ManaIncreaseFromKill);
         */
    }

    public static void OnEnemyDeath()
    {
        //so, we basically just need to tell the player to increase mana. 
        DeathManager.IncreasePlayerMana();
    }
}
