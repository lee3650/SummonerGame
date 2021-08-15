using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StringDisplayPanel : UIPanel
{
    [SerializeField] TextMeshProUGUI text; 

    public override void Show(object summon)
    {
        text.text = (summon as string);
    }
}
