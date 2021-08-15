using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSummonController : MonoBehaviour
{
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] ItemSelection ItemSelection; //so, we just need to make sure the current item is the summon controller. 
    [SerializeField] SelectedSummonUI SelectedSummonUI;
    [SerializeField] ManaManager ManaManager;

    private IControllableSummon SelectedSummon = null;

    public void UpgradeSummon(UpgradePath path)
    {
        print("upgrade button pressed!");

        if (SelectedSummon != null)
        {
            if (ManaManager.TryDecreaseMana(path.GetUpgradeCost()))
            {
                SelectedSummon.HandleCommand(new UpgradeCommand(path));
                DeselectSummon();
            }
        }
    }

    private void Update()
    {
        if (MousePressedUsingController(0) && !MouseOverUIComponent())
        {
            if (SelectedSummon == null)
            {
                IControllableSummon s = GetSummonUnderMouse();
                SelectSummon(s);
            }
            else
            {
                SelectedSummon.HandleCommand(new HoldPointCommand(VectorRounder.RoundVector(PlayerInput.GetWorldMousePosition())));
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
                    SelectedSummon.HandleCommand(new SetTargetCommand(target));
                }

                DeselectSummon();
            }
        }

        if (ShouldTellSummonToToggleGuardMode())
        {
            if (SelectedSummon != null)
            {
                SelectedSummon.HandleCommand(new ToggleGuardModeCommand());
                DeselectSummon();
            }
        }

        if (ShouldTellSummonToRest())
        {
            if (SelectedSummon != null)
            {
                SelectedSummon.HandleCommand(new RestCommand());
                DeselectSummon();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //so, right click, we'll get rid of the blueprint
            //and then basically we'll check in wall generator if one of 'our' summons has been moved, or our satisfied blueprints, and then 
            //we'll deal with that there. 

            BlueprintManager.RemoveBlueprint(VectorRounder.RoundVector(PlayerInput.GetWorldMousePosition()));
        }
    }

    bool MouseOverUIComponent()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    bool ShouldTellSummonToToggleGuardMode() 
    {
        return Input.GetKeyDown(KeyCode.G) && UsingController();
    }

    bool ShouldTellSummonToRest()
    {
        return Input.GetKeyDown(KeyCode.R) && UsingController();
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

    //I don't like this but we're going to have this access the UI. 
    void SelectSummon(IControllableSummon s)
    {
        if (s != null)
        {
            SelectedSummonUI.SelectSummon(s);

            SelectedSummon = s;

            SelectableComponent sc;
            if (SelectedSummon.GetTransform().TryGetComponent<SelectableComponent>(out sc))
            {
                print("selectable component should show graphic!");
                sc.Select();
            }

            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * Time.fixedDeltaTime;
        }
    }

    public void DeselectSummon()
    {
        SelectedSummonUI.DeselectSummon();

        SelectableComponent sc;
        if (SelectedSummon != null && SelectedSummon.GetTransform().TryGetComponent<SelectableComponent>(out sc))
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

    IControllableSummon GetSummonUnderMouse()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(PlayerInput.GetWorldMousePosition(), 0.5f);
        print("colliders found: " + cols.Length);
        foreach (Collider2D col in cols)
        {
            IControllableSummon s;
            if (col.TryGetComponent<IControllableSummon>(out s))
            {
                print("There was a summon!");
                //so, there is a summon. 
                if (s.GetTransform().GetComponent<ILivingEntity>().GetFaction() == Factions.Player)
                {
                    if (s.CanBeSelected())
                    {
                        return s;
                    } else
                    {
                        print("Could not select entity!");
                    }
                }
            }
        }
        return null;
    }
}
