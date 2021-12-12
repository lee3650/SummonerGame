using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : MonoBehaviour, IState
{
    [SerializeField] MovementController MovementController;
    [SerializeField] Collider2D col;
    [SerializeField] SpriteRenderer SpriteRenderer;
    float startFadeTime = 3f;
    float fadeLength = 2f;

    float deathTimer = 0f;

    public void EnterState()
    {
        col.enabled = false;
        MovementController.DisableAllMovement();

        if (SpriteRenderer == null)
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        SetDeathGraphic();

        RemoveProjectiles();

        StartCoroutine(Destroy());
        
        VirtualEnterState();
    }
    
    public virtual void VirtualEnterState()
    {

    }

    private void SetDeathGraphic()
    {
        Animator m;
        if (TryGetComponent<Animator>(out m))
        {
            m.enabled = false;
        }
        SpriteRenderer.sprite = TombstoneSpriteManager.GetRandomTombstone();
        SpriteRenderer.sortingOrder = -3;
    }

    private void RemoveProjectiles()
    {
        foreach (Transform child in transform)
        {
            Projectile p;
            if (child.TryGetComponent<Projectile>(out p))
            {
                p.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateState()
    {
        deathTimer += Time.deltaTime;

        if (deathTimer >= startFadeTime)
        {
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 
                Mathf.Lerp(SpriteRenderer.color.a, 0, (deathTimer - startFadeTime) / fadeLength));
        } else
        {
            RemoveProjectiles();
        }
    }

    public void ExitState()
    {

    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
        //hm... that shouldn't cause problems, right? 
    }
}