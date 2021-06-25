using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, ILivingEntity
{
    [SerializeField] int Precedence;
    [SerializeField] HealthManager HealthManager;

    private void Start()
    {
        HealthManager.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        //so, disable collider, etc. 

    }

    public Factions GetFaction()
    {
        return Factions.Player;
    }

    public int GetPrecedence()
    {
        return Precedence;
    }

    public bool IsAlive()
    {
        return HealthManager.IsAlive();
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void HandleEvent (Event e)
    {
        switch (e.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                HealthManager.SubtractHealth(e.Magnitude);
                break;
        }
    }
}
