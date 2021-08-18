using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthBar : MonoBehaviour, IInitialize
{
    const float NormalHealth = 25f;

    [SerializeField] HealthManager HealthManager;
    [SerializeField] float Height = 1.5f;
    [SerializeField] float WidthAdjustment = -1f;

    Transform healthGraphic;
    float defaultScale;

    public void Init()
    {
        healthGraphic = Instantiate<Transform>(Resources.Load<Transform>("healthGraphic"));
        defaultScale = healthGraphic.localScale.x * (HealthManager.GetMaxHealth() / NormalHealth);
        HealthManager.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        if (healthGraphic != null)
        {
            Destroy(healthGraphic.gameObject);
        }
    }

    private void Update()
    {
        if (HealthManager.IsAlive() && healthGraphic != null)
        {
            healthGraphic.position = (Vector2)transform.position + new Vector2(WidthAdjustment, Height);
            healthGraphic.localScale = new Vector3(Mathf.Clamp(defaultScale * HealthManager.GetHealthPercentage(), 0, Mathf.Infinity), healthGraphic.localScale.y);
        }
    }
}
