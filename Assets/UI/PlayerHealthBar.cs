using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] ManaManager ManaManager;

    [SerializeField] Slider Slider;
    [SerializeField] TextMeshProUGUI ActualNumbers;

    private IEnergyManager energyManager;

    private void Awake()
    {
        if (HealthManager == null)
        {
            energyManager = ManaManager as IEnergyManager;
        } else
        {
            energyManager = HealthManager as IEnergyManager;
        }
    }

    private void Update()
    {
        Slider.value = energyManager.GetRemainingPercentage();
        ActualNumbers.text = string.Format("{0} / {1}", energyManager.GetCurrent(), energyManager.GetMax());
    }
}
