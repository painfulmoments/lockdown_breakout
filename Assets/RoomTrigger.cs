using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NewPlayerController npc = other.GetComponent<NewPlayerController>();
            if (npc != null)
                npc.isInRoom = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NewPlayerController npc = other.GetComponent<NewPlayerController>();
            if (npc != null)
                npc.isInRoom = false;
        }
    }
}