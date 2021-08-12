using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSummonUI : MonoBehaviour
{
    [SerializeField] DisplayUpgrade UpgradePanelPrefab;
    [SerializeField] SummonInfoPanel SummonInfoPrefab;
    [SerializeField] RectTransform UpgradePanelParent;

    [SerializeField] PlayerSummonController PlayerSummonController;

    List<GameObject> DisplayedPanels = new List<GameObject>();

    public void SelectSummon(IControllableSummon s)
    {
        UpgradePath[] upgrades = s.GetTransform().GetComponents<UpgradePath>();
        if (upgrades != null)
        {
            foreach (UpgradePath p in upgrades)
            {
                if (p.Useable)
                {
                    DisplayUpgrade d = Instantiate(UpgradePanelPrefab, UpgradePanelParent);
                    d.ShowUpgrade(p, PlayerSummonController);
                    DisplayedPanels.Add(d.gameObject);
                }
            }
        }

        SummonInfoPanel info = Instantiate(SummonInfoPrefab, UpgradePanelParent);
        info.DisplaySummonInfo(s);
        DisplayedPanels.Add(info.gameObject);
    }

    public void DeselectSummon()
    {
        foreach (GameObject g in DisplayedPanels)
        {
            Destroy(g);
        }

        DisplayedPanels = new List<GameObject>();
    }
}
