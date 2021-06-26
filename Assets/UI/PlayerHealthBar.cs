using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] HealthManager Health;
    [SerializeField] Slider Slider;

    private void Update()
    {
        Slider.value = Health.GetHealthPercentage();
    }

}
