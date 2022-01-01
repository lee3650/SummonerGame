using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutOnDamage : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] Image image;
    private void Start()
    {
        HealthManager.OnDamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return null;
    }
}
