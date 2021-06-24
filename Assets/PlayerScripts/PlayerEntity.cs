using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, ILivingEntity
{
    [SerializeField] int Precedence;

    bool Alive = true;
    
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
        return Alive;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void HandleEvent (Event e)
    {
        switch (e)
        {

        }
    }
}
