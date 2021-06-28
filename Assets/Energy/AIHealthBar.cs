using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthBar : MonoBehaviour
{
    const float NormalHealth = 25f;

    [SerializeField] HealthManager HealthManager;
    [SerializeField] float Height = 1.5f;
    [SerializeField] float WidthAdjustment = -1f;

    Transform healthGraphic;
    float defaultScale;

    private void Awake()
    {
        healthGraphic = Instantiate<Transform>(Resources.Load<Transform>("healthGraphic"));
        defaultScale = healthGraphic.localScale.x * (HealthManager.GetMaxHealth() / NormalHealth);
    }

    private void Update()
    {
        healthGraphic.position = (Vector2)transform.position + new Vector2(WidthAdjustment, Height);
        healthGraphic.localScale = new Vector3(defaultScale * HealthManager.GetHealthPercentage(), healthGraphic.localScale.y);
    }

}
