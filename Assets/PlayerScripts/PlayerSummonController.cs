using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSummonController : MonoBehaviour
{
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] ItemSelection ItemSelection; //so, when we change selection we want to deselect the current summon. 
    [SerializeField] SelectedSummonUI SelectedSummonUI;
    [SerializeField] ManaManager ManaManager;
    [SerializeField] PlayerAttackState PlayerAttackState;

    const float SelectionRadius = 0.1f;

    private IControllableSummon SelectedSummon = null;

    int frameOfDeselection = -1;

    private void Awake()
    {
        ItemSelection.SelectedItemChanged += SelectedItemChanged;
    }

    private void SelectedItemChanged()
    {
        if (SelectedSummon != null)
        {
            DeselectSummon();
        }
    }

    public void SellSummon(Sellable sellable)
    {
        if (SelectedSummon != null)
        {
            ManaManager.IncreaseMana(sellable.SellPrice);
            IControllableSummon s = SelectedSummon;
            DeselectSummon();
            s.HandleCommand(new SellCommand());
        }
    }

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SelectedSummon != null)
            {
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

        if (SelectedSummon != null)
        {
            if (SelectedSummon.CanBeSelected() == false)
            {
                DeselectSummon();
            }
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

            RangeVisualizer rv;
            if (SelectedSummon.GetTransform().TryGetComponent<RangeVisualizer>(out rv))
            {
                rv.Show();
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

        RangeVisualizer rv; //hm. Is this okay? I mean, it's procedular, but not a bad way, right? 
        if (SelectedSummon.GetTransform().TryGetComponent<RangeVisualizer>(out rv))
        {
            rv.Hide(); 
        }

        SelectedSummon = null;

        Time.fixedDeltaTime = 1f/50f;
        Time.timeScale = 1f;
    }

    IControllableSummon GetSummonUnderMouse()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(PlayerInput.GetWorldMousePosition(), SelectionRadius);
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
