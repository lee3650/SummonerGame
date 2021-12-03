using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventVisualizer : MonoBehaviour, ISubEntity
{
    [SerializeField] ParticleSystem FireSystem;
    [SerializeField] SpriteRenderer sr;
    private void Awake()
    {
        if (FireSystem == null)
        {
            FireSystem = GetComponent<ParticleSystem>();
        }
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
    }

    public Event ModifyEvent(Event e)
    {
        return e;
    }

    public void HandleEvent(Event e)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (e.MyType == EventType.Fire)
        {
            StartCoroutine(EnableFireParticles());
        }

        if (e.MyType == EventType.Poison)
        {
            StartCoroutine(EnablePoisonEffect());
        }
    }

    IEnumerator EnableFireParticles()
    {
        FireSystem.Play();
        yield return new WaitForSeconds(0.7f);
        FireSystem.Stop();
    }

    IEnumerator EnablePoisonEffect()
    {
        sr.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }
}
