using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampPosition : MonoBehaviour
{
    [SerializeField] float MaxXDelta;
    [SerializeField] float MaxYDelta;

    public void ClampTransformPosition()
    {
        Vector2 deltas = (Vector2)transform.position - MapCenterManager.MapCenter;
        if (Mathf.Abs(deltas.x) > MaxXDelta)
        {
            transform.position = new Vector2(MapCenterManager.MapCenter.x + (Mathf.Sign(deltas.x) * MaxXDelta), transform.position.y);
        }

        if (Mathf.Abs(deltas.y) > MaxYDelta)
        {
            transform.position = new Vector2(transform.position.x, MapCenterManager.MapCenter.y + (Mathf.Sign(deltas.y) * MaxYDelta));
        }
    }

    private void Update()
    {
        ClampTransformPosition();
    }
}
