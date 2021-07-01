using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] List<AIEntity> EnemiesInRoom;
    [SerializeField] List<GameObject> Doors; 

    public void RoomEnter()
    {
        //so, wake everyone up. 
        //once everyone is dead, open the doors. 

        CloseDoors();

        foreach (AIEntity s in EnemiesInRoom)
        {
            s.WakeUp();
        }
    }

    void OpenDoors()
    {
        foreach (GameObject door in Doors)
        {
            door.SetActive(false);
        }
    }
    void CloseDoors()
    {
        foreach (GameObject door in Doors)
        {
            door.SetActive(true);
        }
    }

    private void Update()
    {
        if (AllEnemiesDead())
        {
            OpenDoors();
        }
    }

    bool AllEnemiesDead()
    {
        foreach (AIEntity e in EnemiesInRoom)
        {
            if (e.IsAlive())
            {
                return false; 
            }
        }
        return true; 
    }
}
