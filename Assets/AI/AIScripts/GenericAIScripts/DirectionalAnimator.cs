using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAnimator : MonoBehaviour
{
    [SerializeField] private AnimationClip[] AttackAnimation = new AnimationClip[4];
    [SerializeField] private AnimationClip[] WalkAnimation = new AnimationClip[4];
    [SerializeField] private AnimationClip[] IdleAnimation = new AnimationClip[4];
    [SerializeField] Animator Animator;
    [SerializeField] bool MakeAwakeRotationIdle = false;

    [Space(20)]

    [Tooltip("Do not modify: these are the directions to arrange the animations in order of")]
    [SerializeField] 
    private Vector2Int[] Directions = new Vector2Int[4]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    private static Vector2Int[] DirectionsStatic = new Vector2Int[4]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    private void Awake()
    {
        if (MakeAwakeRotationIdle)
        {
            float zRot = transform.eulerAngles.z;
            transform.eulerAngles = Vector3.zero;
            IdleDirection(zRot);
        }
    }

    public void PlayWalk(Vector2 direction)
    {
        //so, it'd be better to input our velocity, right? 

        direction = direction.normalized;
        Vector2Int dir = RoundToCardinalDirection(direction);

        Animator.Play(WalkAnimation[DirectionsIndexOf(dir)].name);
    }

    public void PlayAttack(float zRot)
    {
        Vector2Int dir = RoundToCardinalDirection(GetRotationVector(zRot));
        Animator.Play(AttackAnimation[DirectionsIndexOf(dir)].name);
    }

    private int DirectionsIndexOf(Vector2Int item)
    {
        for (int i = 0; i < Directions.Length; i++)
        {
            if (Directions[i] == item)
            {
                return i;
            }
        }
        throw new System.Exception("Could not get index of direction " + item);
    }

    public void PlayAttack(Vector2 pointToFace)
    {
        Vector2Int dir = RoundToCardinalDirection(GetRotationVector(pointToFace));

        Animator.Play(AttackAnimation[DirectionsIndexOf(dir)].name);
    }

    public void IdleFacePoint(Vector2 pointToFace)
    {
        Vector2Int dir = RoundToCardinalDirection(GetRotationVector(pointToFace));
        Animator.Play(IdleAnimation[DirectionsIndexOf(dir)].name);
    }

    public void IdleDirection(float zRotation)
    {
        Vector2Int dir = RoundToCardinalDirection(GetRotationVector(zRotation));
        if (dir == Vector2Int.zero)
        {
            print("Z rotation of " + zRotation + " resulted in zero dir!"); 
        }
        Animator.Play(IdleAnimation[DirectionsIndexOf(dir)].name);
    }

    private Vector2 GetRotationVector(float zRot)
    {
        return new Vector2(Mathf.Cos(zRot * Mathf.Deg2Rad), Mathf.Sin(zRot * Mathf.Deg2Rad));
    }

    private Vector2 GetRotationVector(Vector2 pointToFace)
    {
        return (pointToFace - (Vector2)transform.position).normalized;
    }

    public static Vector2Int RoundToCardinalDirection(Vector2 input)
    {
        float min = Mathf.Infinity;
        Vector2Int result = new Vector2Int();

        for (int i = 0; i < DirectionsStatic.Length; i++)
        {
            float cur = (input - DirectionsStatic[i]).sqrMagnitude;
            if (cur < min)
            {
                result = DirectionsStatic[i];
                min = cur;
            }
        }

        return result; 
    }
}
