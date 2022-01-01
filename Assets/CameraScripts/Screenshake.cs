using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    [SerializeField] float intensity;
    [SerializeField] float length;
    Vector2 originalPos;

    public void StartShake()
    {
        StopAllCoroutines();
        originalPos = transform.position;
        StartCoroutine(ShakeScreen());
    }

    IEnumerator ShakeScreen()
    {
        float timer = 0f;
        while (timer <= length)
        {
            timer += Time.deltaTime;
            transform.position = (Vector3)((intensity * Random.insideUnitCircle) + originalPos) + new Vector3(0, 0, -10);
            yield return null;
        }
        transform.position = originalPos;
    }
}
