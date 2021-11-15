using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentManaDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] ManaManager PlayerMana;

    private void Update()
    {
        Text.text = string.Format("{0}", FloatRounder.RoundFloat(PlayerMana.GetCurrent(), 2));
    }
}
