﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : PlayerWall, IDamager, IWielder
{
    [SerializeField] Projectile Projectile;
    [SerializeField] Transform SpawnPos; 
    [SerializeField] float AttackLength;
    [SerializeField] float projDir;
    [SerializeField] DirectionalAnimator Animator;
    [SerializeField] float animationDelayTime = 0.15f;
    float timer = 0f;

    List<Event> AttackModifiers = new List<Event>();

    float zRot = 0f;

    private void Awake()
    {
        Vector2 spawnPos = SpawnPos.position;
        zRot = transform.eulerAngles.z;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Animator.IdleDirection(zRot);
        SpawnPos.position = spawnPos;
    }

    private void Update()
    {
        if (!WaveSpawner.IsCurrentWaveDefeated) //I'm pretty sure I shouldn't put this in update. 
        {
            timer += Time.deltaTime;
            if (timer > AttackLength)
            {
                timer = 0f;
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        Animator.PlayAttack(zRot);
        yield return new WaitForSeconds(animationDelayTime);
        Projectile p = Instantiate(Projectile, SpawnPos.position, Quaternion.Euler(new Vector3(0f, 0f, zRot + projDir)));
        p.Fire(this, this);
        Animator.IdleDirection(zRot);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnHit(IEntity hit)
    {

    }

    public List<Event> ModifyEventList(List<Event> original)
    {
        //I'm pretty sure I should not modify original. 

        List<Event> result = new List<Event>();

        for (int i = 0; i < original.Count; i++)
        {
            result.Add(original[i]);
        }

        for (int i = 0; i < AttackModifiers.Count; i++)
        {
            result.Add(AttackModifiers[i]);
        }

        return result; 
    }

    public void AddAttackModifier(Event e)
    {
        AttackModifiers.Add(e);
    }
}
