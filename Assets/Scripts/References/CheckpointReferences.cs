using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointReferences : MonoBehaviour
{
    public List<UnityEvent> checkpointReferences = new List<UnityEvent>();

    public static CheckpointReferences instance = null;

    private void Start()
    {
        instance = this;
        CheckpointManager.instance.LoadNewestCheckpoint();
    }

    public UnityEvent GetReference(int referenceID)
    {
        if (referenceID >= checkpointReferences.Count || referenceID == -1) return null;
        return checkpointReferences[referenceID];
    }

    public void TryUnlockCheckpoint(int checkpointID)
    {
        CheckpointManager.instance.UnlockCheckpoint(checkpointID);
    }
}
