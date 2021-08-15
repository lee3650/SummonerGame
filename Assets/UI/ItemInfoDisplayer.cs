using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoDisplayer : MonoBehaviour
{
    [SerializeField] ItemSelection ItemSelection;
    [SerializeField] PanelDisplayer ItemPanelDisplayer;
    [SerializeField] StringDisplayPanel StringDisplayPanel;

    private void Awake()
    {
        ItemSelection.SelectedItemChanged += SelectedItemChanged;
    }

    private void SelectedItemChanged()
    {
        ItemPanelDisplayer.HideAllPanels();

        if (ItemSelection.HasItem())
        {
            IPurchasable purchasable; 
            if (ItemSelection.SelectedItem.TryGetComponent<IPurchasable>(out purchasable))
            {
                string itemInfo = "";
                itemInfo += string.Format("{0}\n\n", purchasable.GetDescription());
                itemInfo += string.Format("Cost: {0}\n", purchasable.GetCost());
                itemInfo += string.Format("Maintenance Fee: {0}\n", purchasable.GetRecurringCost());

                ItemPanelDisplayer.ShowPanel(StringDisplayPanel, itemInfo);
            }
        }
    }
}