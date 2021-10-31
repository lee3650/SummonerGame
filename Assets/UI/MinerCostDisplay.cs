using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinerCostDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;

    private void Update()
    {
        Text.text = string.Format("{0}", MinerSummon.GetCurrentMinerCost());
    }
}
