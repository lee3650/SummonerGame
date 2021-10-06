using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSummonUI : MonoBehaviour
{
    [SerializeField] DisplayUpgrade UpgradePanelPrefab;
    [SerializeField] StringDisplayPanel StringDisplayPanelPrefab;

    [SerializeField] Canvas TooltipCanvas; //this code really should not be here. 
    [SerializeField] PlayerInput pi;
    [SerializeField] PanelDisplayer TooltipPD;

    [SerializeField] PlayerSummonController PlayerSummonController;

    [SerializeField] UnlockedUpgradeManager UnlockedUpgradeManager;  

    [SerializeField] PanelDisplayer PanelDisplayer;
    [SerializeField] RectTransform UpgradePanelParent;

    [SerializeField] SellPanel SellPanel; 

    [SerializeField] PlayerInput PlayerInput;

    bool SummonSelected = false;
    Vector2Int lastTile = new Vector2Int(-1, -1);

    Vector2Int lastMousePos = new Vector2Int(-1, -1);

    [SerializeField] float TimeToShowTooltip = 2.5f;
    float timeSinceMouseMove = 0f;

    //we're going to have to add some timing mechanism. We should really move this code. 
    private void Update()
    {
        Vector2Int mousePos = VectorRounder.RoundVectorToInt(PlayerInput.GetWorldMousePosition());

        if (!SummonSelected)
        {
            if (mousePos == lastMousePos)
            {
                timeSinceMouseMove += Time.deltaTime;
            } else
            {
                timeSinceMouseMove = 0f; 
            }

            if (MapManager.IsPointInBounds((int)mousePos.x, (int)mousePos.y))
            {
                if (timeSinceMouseMove > TimeToShowTooltip)
                {
                    TileType mousedTile = MapManager.GetTileType(mousePos);
                    if (mousePos != lastTile)
                    {
                        TooltipCanvas.gameObject.SetActive(true);
                        TooltipCanvas.transform.position = pi.GetWorldMousePosition();
                        TooltipPD.HideAllPanels();
                        lastTile = mousePos;
                        TooltipPD.ShowPanel(StringDisplayPanelPrefab, TileDescription.GetTileDescription(mousedTile));
                    }
                } else
                {
                    TooltipCanvas.gameObject.SetActive(false);
                    TooltipPD.HideAllPanels();
                }
            } else
            {
                TooltipCanvas.gameObject.SetActive(false);
                TooltipPD.HideAllPanels();
                timeSinceMouseMove = 0f;
            }
        }

        lastMousePos = mousePos;

        print("Time since mouse move: " + timeSinceMouseMove);
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