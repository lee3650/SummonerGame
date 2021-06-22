using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEntity : MonoBehaviour, ILivingEntity
{
    [SerializeField] Factions Faction;
    [SerializeField] int Precedence;

    bool Alive;

    public void HandleEvent(Event e)
    {
        switch (e.MyType)
        {

        }
    }

    public bool IsAlive()
    {
        return Alive;
    }

    public Factions GetFaction()
    {
        return Faction;
    }

    public int GetPrecedence()
    {
        return Precedence;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
