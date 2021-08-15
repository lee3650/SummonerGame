using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSummonUI : MonoBehaviour
{
    [SerializeField] DisplayUpgrade UpgradePanelPrefab;
    [SerializeField] StringDisplayPanel StringDisplayPanelPrefab;

    [SerializeField] PlayerSummonController PlayerSummonController;

    [SerializeField] PanelDisplayer PanelDisplayer;
    [SerializeField] RectTransform UpgradePanelParent;

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

        UpgradePath[] upgrades = s.GetTransform().GetComponents<UpgradePath>();
        if (upgrades != null)
        {
            foreach (UpgradePath p in upgrades)
            {
                if (p.Useable)
                {
                    PanelDisplayer.ShowPanel(UpgradePanelPrefab, (p, PlayerSummonController));
                }
            }
        }

        PanelDisplayer.ShowPanel(StringDisplayPanelPrefab, s.GetStatString());
    }

    public void DeselectSummon()
    {
        PanelDisplayer.HideAllPanels();
        SummonSelected = false; 
    }
}
