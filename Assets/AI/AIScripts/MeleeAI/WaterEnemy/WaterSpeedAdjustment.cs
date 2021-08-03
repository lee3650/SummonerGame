using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpeedAdjustment : MonoBehaviour
{
    [SerializeField] MovementController MovementController;
    [SerializeField] float MoveSpeedMultiplier = 2f;

    float originalSpeed;
    float adjustedSpeed; 

    private void Start()
    {
        originalSpeed = MovementController.GetMoveSpeed();
        adjustedSpeed = originalSpeed * MoveSpeedMultiplier;
    }

    void Update()
    {
        int roundX = Mathf.RoundToInt(transform.position.x);
        int roundY = Mathf.RoundToInt(transform.position.y);

        if (MapManager.IsTileType(roundX, roundY, TileType.Water))
        {
            MovementController.SetMoveSpeed(adjustedSpeed);
        } else
        {
            MovementController.SetMoveSpeed(originalSpeed);
        }
    }
}
