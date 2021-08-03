using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSummonController : MonoBehaviour
{
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] ItemSelection ItemSelection; //so, we just need to make sure the current item is the summon controller. 

    private ControllableSummon SelectedSummon = null;

    private void Update()
    {
        if (MousePressedUsingController(0))
        {
            if (SelectedSummon == null)
            {
                ControllableSummon s = GetSummonUnderMouse();
                SelectSummon(s);
            }
            else
            {
                SelectedSummon.HoldPoint(PlayerInput.GetWorldMousePosition());
                DeselectSummon();

            }
        }

        if (MousePressedUsingController(1))
        {
            if (SelectedSummon != null)
            {
                ITargetable target = GetTargetUnderMouse();

                if (target != null && target != (SelectedSummon as ITargetable) && target.CanBeTargetedBy(Factions.Player))
                {
                    SelectedSummon.SetTarget(target);
                }

                DeselectSummon();
            }
        }

        if (ShouldTellSummonToToggleGuardMode())
        {
            if (SelectedSummon != null)
            {
                SelectedSummon.ToggleGuardMode();
                DeselectSummon();
            }
        }
    }

    bool ShouldTellSummonToToggleGuardMode() 
    {
        return Input.GetKeyDown(KeyCode.G) && UsingController();
    }

    bool ShouldTellSummonToHoldPoint()
    {
        return Input.GetKeyDown(KeyCode.E) && UsingController();
    }

    bool MousePressedUsingController(int mouseKey)
    {
        return Input.GetMouseButtonDown(mouseKey) && UsingController();
    }

    bool UsingController()
    {
        return ItemSelection.HasItem() && (ItemSelection.SelectedItem.GetItemType() == ItemType.SummonController);
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

            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * Time.fixedDeltaTime;
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

        Time.fixedDeltaTime = 1f/50f;
        Time.timeScale = 1f;
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
        Collider2D[] cols = Physics2D.OverlapCircleAll(PlayerInput.GetWorldMousePosition(), 0.5f);
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
