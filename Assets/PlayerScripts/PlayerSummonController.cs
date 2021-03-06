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
    [SerializeField] Summoner Summoner;
    [SerializeField] BlueprintFees BlueprintFees;
    [SerializeField] InventorySlotManager WeaponInv;
    [SerializeField] AnimateInAndOut BlueprintHotbar; 

    const float SelectionRadius = 0.1f;

    private IControllableSummon SelectedSummon = null;

    int frameOfDeselection = -1;

    public void SellSummon(Sellable sellable)
    {
        if (SelectedSummon != null)
        {
            ManaManager.IncreaseMana(sellable.SellPrice);
            IControllableSummon s = SelectedSummon;
            DeselectSummonAndHideHotbar();
            TryRefundBuildingSummons(s);
            s.HandleCommand(new SellCommand()); //okay good... this can work then. 

            Summoner.OnFinancialsChanged();
        }
    }

    private void RefundBlueprint(Blueprint b)
    {
        if (b == null)
        {
            return; 
        }

        ManaManager.IncreaseMana(b.MaintenanceFee);
    }

    private void TryRefundBuildingSummons(IControllableSummon s)
    {
        Transform t = s.GetTransform();

        BlueprintSatisfier building;

        if (t.TryGetComponent<BlueprintSatisfier>(out building))
        {
            List<BlueprintSummon> summons = building.GetBlueprintSummons();

            foreach (BlueprintSummon b in summons)
            {
                if (b != null && b.IsAlive())
                {
                    RefundBlueprint(b.Blueprint);
                }
            }
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
                DeselectSummonAndHideHotbar();
                Summoner.OnFinancialsChanged();
            }
        }
    }

    public Vector2 GetSelectedSummonPosition()
    {
        if (SelectedSummon == null)
        {
            return new Vector2();
        }
        return SelectedSummon.GetTransform().position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !MouseOverUIComponent() && !PlayerAttackState.AttackedThisFrame())
        {
            IControllableSummon s = GetSummonUnderMouse();

            if (s != null)
            {
                SelectSummon(s);
            } else
            {
                //if we have an item, only deselect it if you clicked far away from a usable tile 
                if (ItemSelection.HasItem())
                {
                    DeselectItemBasedOnAdjacency();
                } else 
                {
                    //if you don't have an item, just deselect the current summon 
                    DeselectSummonAndHideHotbar();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SelectedSummon != null)
            {
                DeselectSummonAndHideHotbar();
            }
        }

        if (Input.GetMouseButtonDown(1) && WaveSpawner.IsCurrentWaveDefeated)
        {
            Blueprint b = BlueprintManager.TryRemoveBlueprint(VectorRounder.RoundVectorToInt(PlayerInput.GetWorldMousePosition())); 

            if (b != null)
            {
                if (b.Satisfied)
                {
                    DesatisfyBlueprint(b);
                }

                if (LetterManager.UseGameplayChange(GameplayChange.IncrementPrice)) { 
                    BlueprintFees.BlueprintRemoved(b.BlueprintType);
                }

                Summoner.OnFinancialsChanged();
            }
        }

        if (SelectedSummon != null)
        {
            if (SelectedSummon.CanBeSelected() == false)
            {
                DeselectSummonAndHideHotbar();
            }
        }
    }

    private void DeselectItemBasedOnAdjacency()
    {
        bool passed = false;

        Vector2Int[] dirs = new Vector2Int[]
        {
            new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, -1)
        };

        for (int i = 0; i < 4; i++)
        {
            if (PlayerAttackState.IsPositionSpawnable(PlayerInput.GetWorldMousePosition() + dirs[i]))
            {
                passed = true;
                break;
            }
        }

        if (!passed)
        {
            DeselectSummonAndHideHotbar();
        } 
    }

    private void DesatisfyBlueprint(Blueprint b)
    {
        float refund = Mathf.Max(b.MaintenanceFee, BlueprintManager.GetMaxSatisfiedFee(b.BlueprintType));
        print("Removed fee: " + b.MaintenanceFee + ",  refund: " + refund);

        ManaManager.IncreaseMana(refund);
        print("Current balance: " + ManaManager.GetCurrent());
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
            BlueprintSatisfier bs = s.GetTransform().GetComponent<BlueprintSatisfier>();

            if (SelectedSummon != null)
            {
                if (bs == null)
                {
                    DeselectSummonAndHideHotbar();
                } else
                {
                    ItemSelection.DeselectItem();
                    DeselectSummon();
                }
            }

            SelectedSummon = s;

            SelectedSummonUI.SelectSummon(s);

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

            if (bs != null)
            {
                List<BlueprintType> blueprints = bs.GetBlueprintTypes();

                //okay, now we just tell the 
                //hotbar to show only those types

                WeaponInv.ShowHotbarItemsOfTypes(blueprints);
                BlueprintHotbar.PlayAnimateIn();
            }
        } 
    }

    public void HideHotbar()
    {
        if (BlueprintHotbar.IsShown)
        {
            BlueprintHotbar.ToggleVisibility();
        }
    }

    public void DeselectSummonAndHideHotbar()
    {
        HideHotbar();
        DeselectSummon();
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
        if (SelectedSummon != null && SelectedSummon.GetTransform().TryGetComponent<RangeVisualizer>(out rv))
        {
            rv.Hide(); 
        }

        SelectedSummon = null;
    }

    IControllableSummon GetSummonUnderMouse()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(PlayerInput.GetWorldMousePosition(), SelectionRadius);
        
        foreach (Collider2D col in cols)
        {
            IControllableSummon s;
            if (col.TryGetComponent<IControllableSummon>(out s))
            {
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
