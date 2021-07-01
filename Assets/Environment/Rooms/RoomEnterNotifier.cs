using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterNotifier : MonoBehaviour
{
    [SerializeField] Room Room;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Room.RoomEnter();
        }
    }
}
