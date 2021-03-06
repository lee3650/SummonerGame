using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, ILivingEntity
{
    [SerializeField] int Precedence;
    [SerializeField] HealthManager HealthManager;
    [SerializeField] Collider2D col;
    [SerializeField] PlayerDeathState PlayerDeathState;
    [SerializeField] CoatingManager CoatingManager;
    [SerializeField] PlayerFallState FallState;

    StateController StateController;

    private void Awake()
    {
        //TargetableEntitiesManager.AddTargetable(this);
        if (CoatingManager == null)
        {
            CoatingManager = GetComponent<CoatingManager>();
        }
    }

    private void Start()
    {
        HealthManager.OnDeath += OnDeath;
        HealthManager.OnHealthChanged += OnHealthChanged;
        StateController = GetComponent<StateController>();
    }

    private void OnDeath()
    {
        //so, disable collider, etc. 
        //we'd actually want to transition to the death state, right. 
        col.enabled = false;
        StateController.TransitionToState(PlayerDeathState);
    }

    public bool CanBeTargetedBy(Factions faction)
    {
        if (faction != Factions.Player)
        {
            return true; 
        }
        return false; 
    }

    public List<Event> ModifyEventList(List<Event> umodifiedList)
    {
        return umodifiedList;
    }

    private void OnHealthChanged()
    {

    }

    public bool RequireLineOfSight()
    {
        return true; 
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnHit(IEntity hit)
    {

    }

    public bool CanBeTargeted()
    {
        return true; 
    }

    public bool IsDamaged()
    {
        return HealthManager.GetCurrent() < HealthManager.GetMax();
    }

    public bool CanChangeSelectedItem()
    {
        if (StateController.GetCurrentState() is PlayerAttackState || StateController.GetCurrentState() is PlaceHomeState)
        {
            return false;
        }
        return true; 
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
        e = CoatingManager.ModifyEvent(e);

        switch (e.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                HealthManager.SubtractHealth(e.Magnitude);
                break;
            case EventType.Fall:
                if (!(StateController.GetCurrentState() is PlayerFallState))
                {
                    StateController.TransitionToState(FallState);
                }
                break; 
        }
    }
}
