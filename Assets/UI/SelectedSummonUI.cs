using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSummonUI : MonoBehaviour
{
    [SerializeField] DisplayUpgrade UpgradePanelPrefab;
    [SerializeField] StringDisplayPanel StringDisplayPanelPrefab;

    [SerializeField] PlayerSummonController PlayerSummonController;

    [SerializeField] UnlockedUpgradeManager UnlockedUpgradeManager;  

    [SerializeField] PanelDisplayer PanelDisplayer;
    [SerializeField] RectTransform UpgradePanelParent;

    [SerializeField] SellPanel SellPanel; 

    [SerializeField] PlayerInput PlayerInput;

    bool SummonSelected = false;
    TileType lastTile = TileType.Barracks; 

    private void Update()
    {
        if (!SummonSelected)
        {
            Vector2 mousePos = VectorRounder.RoundVector(PlayerInput.GetWorldMousePosition());
            if (MapManager.IsPointInBounds((int)mousePos.x, (int)mousePos.y))
            {
                TileType mousedTile = MapManager.GetTileType(mousePos);
                if (mousedTile != lastTile)
                {
                    PanelDisplayer.HideAllPanels();
                    lastTile = mousedTile;
                    PanelDisplayer.ShowPanel(StringDisplayPanelPrefab, TileDescription.GetTileDescription(mousedTile));
                }

            } else
            {
                PanelDisplayer.HideAllPanels();
            }
        }
    }

    public void SelectSummon(IControllableSummon s)
    {
        SummonSelected = true; 

        PanelDisplayer.HideAllPanels();

        List<UpgradePath> upgrades = UnlockedUpgradeManager.GetUnlockedUpgrades(s.GetSummonType(), s.SummonTier);

        if (upgrades != null)
        {
            foreach (UpgradePath p in upgrades)
            {
                PanelDisplayer.ShowPanel(UpgradePanelPrefab, (p, PlayerSummonController));
            }
        }

        Sellable sellable;
        if (s.CanBeSold() && s.GetTransform().TryGetComponent<Sellable>(out sellable))
        {
            PanelDisplayer.ShowPanel(SellPanel, (sellable, PlayerSummonController)); 
            //so, yeah that's sketchy because if the object it's asking for changes, we won't know until we get a runtime error 
        }

        PanelDisplayer.ShowPanel(StringDisplayPanelPrefab, s.GetStatString());
    }
    
    public void DeselectSummon()
    {
        PanelDisplayer.HideAllPanels();
        SummonSelected = false; 
    }
}