using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandleSelection : MonoBehaviour
{
    [SerializeField] ItemSelection ItemSelection;
    [SerializeField] Transform HoldPosition;

    private void Start()
    {
        ItemSelection.SelectedItemChanged += SelectedItemChanged;
    }

    private void SelectedItemChanged()
    {
        print("selection changed!");
        if (ItemSelection.HasItem())
        {
            print("had item!");

            ItemSelection.SelectedItem.transform.parent = HoldPosition;
            ItemSelection.SelectedItem.transform.localPosition = Vector3.zero;
            ItemSelection.SelectedItem.transform.localScale = Vector3.one;
            ItemSelection.SelectedItem.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}