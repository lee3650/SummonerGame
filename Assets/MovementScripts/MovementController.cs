using System.IO;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float BaseMoveSpeed;
    [SerializeField] float Sensitivity;

    [SerializeField] bool CanGoThroughWalls = true;

    float MoveSpeed; 

    Vector2 pathfindGoal = new Vector2();
    SearchNode pathfindPath = null;

    SightChecker SightChecker;
    RotationController rc;

    ISpeedSupplier SpeedSupplier; 

    private void Awake()
    {
        rc = GetComponent<RotationController>();
        SightChecker = GetComponent<SightChecker>();
        MoveSpeed = BaseMoveSpeed;
        SpeedSupplier = GetComponent<ISpeedSupplier>();
    }


    private void Update()
    {
        if (SpeedSupplier != null)
        {
            MoveSpeed = BaseMoveSpeed * SpeedSupplier.GetMoveSpeedAdjustment();
        }
    }

    public void SetPathToHomePath()
    {
        if (pathfindGoal != PathManager.HomeTile)
        {
            pathfindGoal = PathManager.HomeTile;
            pathfindPath = PathManager.GetLatestPath(VectorRounder.RoundVectorToInt(transform.position)); //hm... this is interesting. Don't even use spawn points. 
        }
    }

    public Vector2 GetNextPathfindPosition()
    {
        if (pathfindPath == null)
        {
            return pathfindGoal;
        }
        return new Vector2(pathfindPath.x, pathfindPath.y);
    }

    public void MoveTowardPointWithRotation(Vector2 pointToHold)
    {
        if (Vector2.Distance(transform.position, pointToHold) > 0.5f)
        {
            MonitorGoalAndFollowPath();
            //rc.FaceForward();
        }
    }

    public void MoveTowardPoint(Vector2 worldPoint)
    {
        Vector2 dir = worldPoint - rb.position;
        MoveInDirection(dir.normalized);
    }

    public void MonitorGoalAndFollowPath()
    {
        if (pathfindPath == null)
        {
            print("pathfindpath was null!");
            return; 
        }

        //if ((pathfindPath.x != (int)pathfindGoal.x && pathfindPath.y != (int)pathfindGoal.y) && SightChecker.CanSeePathToTarget(pathfindGoal))
        //{
        //    pathfindPath = new SearchNode((int)pathfindGoal.x, (int)pathfindGoal.y);
        //}
        //else
        //{
        //}
        MoveTowardPoint(new Vector2(pathfindPath.x, pathfindPath.y));

        if (Vector2.Distance(new Vector2(pathfindPath.x, pathfindPath.y), transform.position) < 0.25f)
        {
            if (pathfindPath.ParentNode != null)
            {
                pathfindPath = pathfindPath.ParentNode;
            }
        }
    }

    public bool FoundPath()
    {
        return pathfindPath != null;
    }

    public SearchNode GetPathfindPath()
    {
        return pathfindPath; //so, this is kind of bad, because now we can't really have the pathfinding be on another thread. 
    }

    public void SetPathfindGoal(Vector2 goal)
    {
        pathfindGoal = goal;
        if (SightChecker.CanSeePathToTarget(pathfindGoal))
        {
            pathfindPath = new SearchNode((int)goal.x, (int)goal.y);
        } else
        {
            print("Movement controller calculating path to target!");
            pathfindPath = Pathfinder.GetPathFromPointToPoint(goal, transform.position, CanGoThroughWalls);
        }
    }

    public void MoveInDirection(Vector2 dir)
    {
        float mag = dir.magnitude;
        
        float speed = MoveSpeed;
        
        if (mag >= 1)
        {
            speed = MoveSpeed / mag;
        }

        Vector2 targetVel = dir * speed;

        rb.velocity = Vector2.Lerp(rb.velocity, targetVel, Sensitivity * Time.deltaTime);
    }

    public void DisableRigidbody()
    {
        rb.isKinematic = true;
    }

    public void DisableAllMovement()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void SetMoveSpeed(float value)
    {
        MoveSpeed = value; 
    }
    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }

    public void SetVelocity(Vector2 direction, float magnitude)
    {
        rb.velocity = direction * magnitude;
    }

}
