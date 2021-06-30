﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSummonController : MonoBehaviour
{
    [SerializeField] PlayerInput PlayerInput;

    private ControllableSummon SelectedSummon = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectedSummon == null)
            {
                ControllableSummon s = GetSummonUnderMouse();
                SelectSummon(s);
            }
            else
            {
                ITargetable target = GetTargetUnderMouse();

                if (target != null && target != (SelectedSummon as ITargetable))
                {
                    SelectedSummon.SetTarget(target);
                }

                DeselectSummon();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedSummon != null)
            {
                SelectedSummon.GoToPoint(PlayerInput.GetWorldMousePosition());
                DeselectSummon();
            }
        }
    }

    void SelectSummon(ControllableSummon s)
    {
        if (s != null)
        {
            SelectedSummon = s;

            SelectableComponent sc;
            if (SelectedSummon.TryGetComponent<SelectableComponent>(out sc))
            {
                print("selectable component should show graphic!");
                sc.Select();
            }
        }
    }

    void DeselectSummon()
    {
        SelectableComponent sc;
        if (SelectedSummon.TryGetComponent<SelectableComponent>(out sc))
        {
            sc.Deselect();
        }

        SelectedSummon = null;
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

    ControllableSummon GetSummonUnderMouse()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(PlayerInput.GetWorldMousePosition(), 2f);
        print("colliders found: " + cols.Length);
        foreach (Collider2D col in cols)
        {
            ControllableSummon s;
            if (col.TryGetComponent<ControllableSummon>(out s))
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