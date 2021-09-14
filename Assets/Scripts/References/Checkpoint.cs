using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public bool isUnlocked = false;
    public int checkpointID = -1;

    public void LoadCheckpoint()
    {
        CheckpointReferences.instance.GetReference(checkpointID).Invoke();
        MovePlayerToCheckpoint();
    }

    public void Unlock()
    {
        isUnlocked = true;
        Debug.Log("Unlocked checkpoint " + transform.name);
    }

    public void MovePlayerToCheckpoint()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        player.position = transform.position;
        player.rotation = transform.rotation;
    }
}
