using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public void SpawnExplosion()
    {
        if(ParticleManager.Instance != null)
        {
            ParticleManager.Instance.SpawnExplosionAtPosition(transform.position);
        }
    }
}
