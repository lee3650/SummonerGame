using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SummonInfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text; 

    public void DisplaySummonInfo(ControllableSummon summon)
    {
        text.text = summon.GetStatString();
    }
}
