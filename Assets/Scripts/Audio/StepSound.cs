using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    public static StepSound Instance = null;
    [SerializeField] public FootstepArea[] footstepAreas = null;

    private void Awake()
    {
        Instance = this;
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    public AudioClip GetRandom(int layer)
    {
        if (footstepAreas.Length > 0)
        {
            for (int i = 0; i < footstepAreas.Length; i++)
            {
                FootstepArea checkArea = footstepAreas[i];
                if (checkArea.triggerLayer == (checkArea.triggerLayer | (1 << layer)) && checkArea.footstepSounds.Length > 0)
                {
                    AudioClip footstepSound = checkArea.footstepSounds[Random.Range(0, checkArea.footstepSounds.Length)];
                    return footstepSound;
                }
            }
        }

        return null;
    }
}
