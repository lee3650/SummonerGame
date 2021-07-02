using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterNotifier : MonoBehaviour
{
    [SerializeField] Room Room;

    bool CanEnter = true; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CanEnter)
        {
            Room.RoomEnter();
            collision.GetComponent<Summoner>().MoveSummonsToSummoner();
            CanEnter = false; 
        }
    }
}
