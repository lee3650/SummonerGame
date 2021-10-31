using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedItemText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] ItemSelection ItemSelection;

    void Update()
    {
        if (ItemSelection.HasItem())
        {
            text.text = ItemSelection.SelectedItem.ItemName;
        } else
        {
            text.text = "";
        }
    }
}
