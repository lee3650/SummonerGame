using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSummonUI : MonoBehaviour
{
    [SerializeField] DisplayUpgrade UpgradePanelPrefab;
    [SerializeField] StringDisplayPanel TooltipDisplayPanel;
    [SerializeField] StringDisplayPanel SellInfoDisplayPanel;
    [SerializeField] StickToWorldPoint SelectedSummonParent;

    [SerializeField] PlayerInput pi;
    [SerializeField] StickToWorldPoint TooltipParent;
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

        if (mousePos == lastMousePos)
        {
            timeSinceMouseMove += Time.deltaTime;
        }
        else
        {
            timeSinceMouseMove = 0f;
        }

        if (MapManager.IsPointInBounds((int)mousePos.x, (int)mousePos.y))
        {
            if (timeSinceMouseMove > TimeToShowTooltip)
            {
                TileType mousedTile = MapManager.GetTileType(mousePos);
                if (mousePos != lastTile && mousedTile != TileType.DoNotDraw)
                {
                    TooltipParent.gameObject.SetActive(true);
                    TooltipParent.SetWorldPoint(pi.GetWorldMousePosition());
                    TooltipPD.HideAllPanels();
                    lastTile = mousePos;
                    TooltipPD.ShowPanel(TooltipDisplayPanel, TileDescription.GetTileDescription(mousedTile));
                }
            }
            else
            {
                TooltipParent.gameObject.SetActive(false);
                TooltipPD.HideAllPanels();
                lastTile = new Vector2Int(-1, -1);
            }
        }
        else
        {
            TooltipParent.gameObject.SetActive(false);
            TooltipPD.HideAllPanels();
            timeSinceMouseMove = 0f;
            lastTile = new Vector2Int(-1, -1);
        }

        lastMousePos = mousePos;
    }
    
    public void SelectSummon(IControllableSummon s)
    {
        TooltipPD.HideAllPanels();

        SelectedSummonParent.SetWorldPoint(s.GetTransform().position + new Vector3(0, 1.5f, 0));
        SelectedSummonParent.gameObject.SetActive(true);

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

        PanelDisplayer.ShowPanel(SellInfoDisplayPanel, s.GetStatString(s.GetTransform().position));
    }
    
    public void DeselectSummon()
    {
        PanelDisplayer.HideAllPanels();
        SummonSelected = false;
        SelectedSummonParent.gameObject.SetActive(false);
    }
}