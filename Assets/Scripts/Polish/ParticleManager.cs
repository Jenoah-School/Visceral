using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance = null;

    [Header("Particle systems")]
    [SerializeField] private GameObject explosionParticleSystem = null;

    [Header("Lifetimes")]
    [SerializeField] private float explosionParticleLifetime = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnExplosionAtPosition(Vector3 position)
    {
        GameObject particles = LeanPool.Spawn(explosionParticleSystem);
        particles.transform.position = position;
        LeanPool.Despawn(particles, explosionParticleLifetime + 0.1f);

    }
}
