using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEntity : MonoBehaviour, ILivingEntity
{
    [SerializeField] Factions Faction;
    [SerializeField] int Precedence;
    [SerializeField] HealthManager HealthManager;
    [SerializeField] AIDeathState AIDeathState;
    [SerializeField] StateController StateController;

    private void Start()
    {
        HealthManager.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        //enter death state
        StateController.TransitionToState(AIDeathState);
    }

    public void HandleEvent(Event e)
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

    public bool IsAlive()
    {
        return HealthManager.IsAlive();
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
