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
    [SerializeField] PlayerAttackState PlayerAttackState;

    private IControllableSummon SelectedSummon = null;

    int frameOfDeselection = -1;

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
        if (Input.GetMouseButtonDown(0) && !MouseOverUIComponent() && !PlayerAttackState.AttackedThisFrame())
        {
            IControllableSummon s = GetSummonUnderMouse();
            SelectSummon(s);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //so, right click, we'll get rid of the blueprint
            //and then basically we'll check in wall generator if one of 'our' summons has been moved, or our satisfied blueprints, and then 
            //we'll deal with that there. 
            BlueprintManager.RemoveBlueprint(VectorRounder.RoundVector(PlayerInput.GetWorldMousePosition()));
        }
    }

    public bool IsMouseOverControllableSummon()
    {
        return GetSummonUnderMouse() != null; 
    }

    public bool MouseOverUIComponent()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool HadSelectionThisFrame()
    {
        return frameOfDeselection == Time.frameCount || SelectedSummon != null; 
        //so, either we have a summon right now, or we deselected it this frame. 
    }

    //I don't like this but we're going to have this access the UI. 
    //this is getting a little branch-y

    //so, the problem with this is that they're both eating up input, so it's not really clear which one will go first...
    //so we just need to see if we had a summon this frame, right. 
    void SelectSummon(IControllableSummon s)
    {
        if (s != null)
        {
            if (SelectedSummon != null)
            {
                DeselectSummon();
            }

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
        else
        {
            if (SelectedSummon != null)
            {
                DeselectSummon();
            }
        }
    }

    public void DeselectSummon()
    {
        SelectedSummonUI.DeselectSummon();

        frameOfDeselection = Time.frameCount;

        SelectableComponent sc;
        if (SelectedSummon != null && SelectedSummon.GetTransform().TryGetComponent<SelectableComponent>(out sc))
        {
            sc.Deselect();
        }

        SelectedSummon = null;

        Time.fixedDeltaTime = 1f/50f;
        Time.timeScale = 1f;
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
