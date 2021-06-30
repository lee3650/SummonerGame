using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummonController : MonoBehaviour
{
    [SerializeField] PlayerInput PlayerInput;

    private Summon SelectedSummon = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectedSummon == null)
            {
                Summon s = GetSummonUnderMouse();
                if (s != null)
                {
                    SelectedSummon = s;
                    print("Selected summon!");
                } else
                {
                    print("there was no summon!");
                }
            }
            else
            {
                ITargetable target = GetTargetUnderMouse();

                SelectedSummon.SetTarget(target);
                
                SelectedSummon = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedSummon != null)
            {
                SelectedSummon.GoToPoint(PlayerInput.GetWorldMousePosition());
                SelectedSummon = null;
            }
        }
    }

    ITargetable GetTargetUnderMouse()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(PlayerInput.GetWorldMousePosition(), 0.5f);
        
        foreach (Collider2D col in cols)
        {
            ITargetable targetable;
            if (col.TryGetComponent<ITargetable>(out targetable))
            {
                return targetable;
            }
        }
        
        return null;
    }

    Summon GetSummonUnderMouse()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(PlayerInput.GetWorldMousePosition(), 2f);
        print("colliders found: " + cols.Length);
        foreach (Collider2D col in cols)
        {
            Summon s;
            if (col.TryGetComponent<Summon>(out s))
            {
                print("There was a summon!");
                //so, there is a summon. 
                if (s.GetComponent<ILivingEntity>().GetFaction() == Factions.Player)
                {
                    return s;
                }
            }
        }
        return null;
    }
}
