using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWallsInTheWay : MonoBehaviour
{
    [SerializeField] MovementController MovementController;
    [SerializeField] TargetSearcher TargetSearcher;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerWall playerWall;
        if (collision.gameObject.TryGetComponent<PlayerWall>(out playerWall))
        {
            if (MovementController.GetNextPathfindPosition() == (Vector2)playerWall.transform.position)
            {
                TargetSearcher.AssignTarget(playerWall);
            }
        }
    }

}
