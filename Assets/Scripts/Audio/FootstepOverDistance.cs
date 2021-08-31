using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepOverDistance : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask ignoreMasks = 0;
    [SerializeField] private float groundedCheckDistance = 1.1f;
    [SerializeField] private float movedCheckDistance = 1f;
    [SerializeField] private AudioSource audioSource = null;

    private Vector3 lastStepPosition = Vector3.zero;
    private float sqrCheckDistance = 1f;
    private StepSound stepSound = null;
    // Start is called before the first frame update
    void Start()
    {
        lastStepPosition = transform.position;
        sqrCheckDistance = movedCheckDistance * movedCheckDistance;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        stepSound = StepSound.Instance;
    }

    /// <summary>
    /// Calls PLayFootstepSound after transform travelled a given distance
    /// </summary>
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.125f, Vector3.down, out hit, groundedCheckDistance, ~ignoreMasks))
        {
            if ((transform.position - lastStepPosition).sqrMagnitude > sqrCheckDistance)
            {
                AudioClip footstepSound = stepSound.GetRandom(hit.collider.gameObject.layer);
                if (footstepSound != null) audioSource.PlayOneShot(footstepSound);
                lastStepPosition = transform.position;
            }
        }
    }
}
