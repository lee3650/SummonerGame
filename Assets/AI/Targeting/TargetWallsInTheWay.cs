using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWallsInTheWay : MonoBehaviour
{
    [SerializeField] MovementController MovementController;
    [SerializeField] SecondaryTargetSearcher SecondaryTargetSearcher;
    [SerializeField] TargetManager TargetManager;

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerWall playerWall;
        if (collision.gameObject.TryGetComponent<PlayerWall>(out playerWall))
        {
            if (MovementController.GetNextPathfindPosition() == (Vector2)playerWall.transform.position)
            {
                if (TargetManager.Target != playerWall) //I want this to be a reference comparison
                {
                    print("setting wall target!");
                    SecondaryTargetSearcher.SetSecondaryTarget(playerWall);
                }
            }
        }
    }

}
