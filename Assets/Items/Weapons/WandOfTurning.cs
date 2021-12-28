using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandOfTurning : Weapon
{
    ArrowTurner selectedTurner = null; 

    public override void UseWeapon(Vector2 mousePos)
    {
        print("Using wand of turning!");

        //click once to select an arrow turner
        //click again to make it face the mouse
        if (selectedTurner != null)
        {
            print("Had a selected turner!");

            selectedTurner.GetComponent<SelectableComponent>().Deselect();
            Vector2 delta = mousePos - (Vector2)selectedTurner.transform.position;
            Vector2 rotv = delta.normalized;
                //DirectionalAnimator.RoundToCardinalDirection(delta);
            float rot = Mathf.Atan2(rotv.y, rotv.x) * Mathf.Rad2Deg;
            selectedTurner.SetRotation(rot); //something like that, idk. 
            selectedTurner = null;
        }
        else
        {
            print("No selected turner!");

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                if (hit.collider.TryGetComponent<ArrowTurner>(out ArrowTurner turner))
                {
                    selectedTurner = turner;
                    turner.GetComponent<SelectableComponent>().Select();
                }
            }
        }
    }

    public override bool CanUseWeapon(Vector2 mousePos)
    {
        return true; 
    }
}
