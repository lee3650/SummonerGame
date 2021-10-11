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

    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(ProjectileSpawnDelay);

        Projectile p = Instantiate(Projectile, firingPosition.position, Quaternion.Euler(GetRotationToFaceTarget(TargetManager.Target.GetPosition())));
        ActivateProjectile(p, GetComponent<IWielder>());
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
