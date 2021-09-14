using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> checkpoints = new List<Checkpoint>();

    public static CheckpointManager instance = null;
    private int newestCheckpoint = -1;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadNewestCheckpoint()
    {
        if (newestCheckpoint != -1)
        {
            Debug.Log("Loading checkpoint " + newestCheckpoint);
            checkpoints[newestCheckpoint].LoadCheckpoint();
        }
    }

    public void UnlockCheckpoint(int checkpointID)
    {
        if (checkpointID >= checkpoints.Count || checkpoints[checkpointID].isUnlocked) return;
        checkpoints[checkpointID].Unlock();
        newestCheckpoint = checkpointID;
    }
}
