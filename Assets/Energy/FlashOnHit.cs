using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    [SerializeField] Material flashMaterial;
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] HealthManager HealthManager;

    private Material originalMaterial;

    private void Awake()
    {
        HealthManager.OnDamageTaken += OnDamageTaken;
        originalMaterial = SpriteRenderer.material;
    }

    private void OnDamageTaken()
    {
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        SpriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.15f);
        SpriteRenderer.material = originalMaterial;
    }
}
