using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AIAttackState
{
    [SerializeField] Projectile Projectile;
    [SerializeField] Transform firingPosition;
    [SerializeField] float ProjectileSpawnDelay = 0.25f;

    public override void StartAttack()
    {
        ITargetable target = TargetManager.Target;
        Animator.PlayAttack(target.GetPosition());
        StartCoroutine(SpawnProjectile());
    }

    public void SetProjectile(Projectile p)
    {
        Projectile = p;
    }

    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(ProjectileSpawnDelay);

        Vector3 rot = GetRotationToFaceTarget(TargetManager.Target.GetPosition());

        Projectile p = Instantiate(Projectile, (Vector2)firingPosition.position + getProjPositionAdjustment(rot), Quaternion.Euler(rot));
        ActivateProjectile(p, GetComponent<IWielder>());
    }

    private Vector2 getProjPositionAdjustment(Vector3 rotation)
    {
        float rot = (rotation.z + 90f) * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));
        return dir.normalized * 1.25f;
    }

    private Vector3 GetRotationToFaceTarget(Vector2 targetPos)
    {
        Vector2 delta = (Vector2)transform.position - targetPos;

        float rot = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        return new Vector3(0f, 0f, rot + 90f);
    }

    protected virtual void ActivateProjectile(Projectile p, IWielder wielder)
    {
        //p.gameObject.layer = LayerMask.NameToLayer(LayerManager.GetProjLayerFromFaction(AIEntity.GetFaction()));
        p.Fire(wielder, GetComponent<IEntity>()); //this is not great... 
    }
}
