using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRainWeapon : Weapon, IFinancialPreviewer
{
    [SerializeField] Projectile RainProjectile;
    [SerializeField] float Offset = 10f;
    [SerializeField] private float launchAngle = -75;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float spread = 1f;
    [SerializeField] int minProjectiles, maxProjectiles;
    private const float adjustment = -90f;

    public override void UseWeapon(Vector2 mousePos)
    {
        int arrowNum = Random.Range(minProjectiles, maxProjectiles);
        Vector2 startLocation = GetStartLocation(mousePos);
        StartCoroutine(StartArrowRain(startLocation, arrowNum));
    }

    IEnumerator StartArrowRain(Vector2 startLocation, int arrowNum)
    {
        for (int i = 0; i < arrowNum; i++)
        {
            Projectile p = Instantiate(RainProjectile, startLocation + (Random.insideUnitCircle * spread), Quaternion.Euler(new Vector3(0f, 0f, launchAngle + adjustment)));
            p.Fire(Wielder.GetComponent<IWielder>(), null);
            yield return new WaitForSeconds(delay);
        }
    }

    private Vector2 GetStartLocation(Vector2 mousePos)
    {
        Vector2 launchDir = new Vector2(Mathf.Cos(launchAngle * Mathf.Deg2Rad), Mathf.Sin(launchAngle * Mathf.Deg2Rad));

        return (launchDir * -Offset) + mousePos;
    }

    public float EffectOnBalance()
    {
        return ManaDrain;
    }

    public float EffectOnIncome(Vector2 vector2)
    {
        return 0f;
    }

    public override bool CanUseWeapon(Vector2 mousePos)
    {
        return MapManager.IsPointInBounds((int)mousePos.x, (int)mousePos.y);
    }
}
