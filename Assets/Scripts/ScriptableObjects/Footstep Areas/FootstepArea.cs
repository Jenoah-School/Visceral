using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Footstep area", menuName = "Footsteps Area/New footstep area")]
public class FootstepArea : ScriptableObject
{
    public AudioClip[] footstepSounds = null;
    public LayerMask triggerLayer = 0;
}
