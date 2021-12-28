using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipNonInteractable : Tooltip
{
    [SerializeField] Button Button;
    protected override bool ShouldShowTooltip()
    {
        return !Button.interactable;
    }
}