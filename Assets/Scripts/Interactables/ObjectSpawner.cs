using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Pool;
using UnityEngine.AI;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRange = 5f;
    [SerializeField] private float spawnDelay = 4f;
    [SerializeField] private LayerMask spawnableLayers;
    [SerializeField] private int maxSpawnableObjects = 5;
    [SerializeField] private UnityEvent OnSpawnedAllEnemies;
    [SerializeField] private List<GameObject> objectsToSpawn = new List<GameObject>();
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private float spawnErrorMargin = 4f;
    [SerializeField] private bool autoSpawn = true;

    public bool canSpawn = true;

    private int spawnedObjects = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (objectsToSpawn.Count <= 0) enabled = false;
        if (autoSpawn) StartSpawning();
    }

    public void StartSpawning()
    {
        spawnedObjects = 0;
        CancelInvoke();
        InvokeRepeating("TrySpawnEnemy", spawnDelay, spawnDelay);
    }

    private void TrySpawnEnemy()
    {
        if (!canSpawn) { CancelInvoke(); return; }
        Vector3 spawnPosition = Random.insideUnitCircle * spawnRange;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y) + transform.position;
        if (Physics.Raycast(spawnPosition + Vector3.up, Vector3.down, out RaycastHit hit, spawnErrorMargin, spawnableLayers))
        {
            spawnPosition = hit.point + spawnOffset;
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Count)];
            GameObject spawnedObject = LeanPool.Spawn(objectToSpawn);
            
            if(spawnedObject.TryGetComponent(out NavMeshAgent navMeshAgent))
            {
                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit closestValidPosition, spawnErrorMargin, NavMesh.AllAreas)){
                    spawnPosition = closestValidPosition.position;
                    //Debug.Log("Agent position is valid");
                }
                else
                {
                    Debug.Log("No valid navmesh could be found in the spawnregion for " + spawnedObject.name, spawnedObject);
                }
            }

            spawnedObject.transform.position = spawnPosition;

            spawnedObjects++;

            if(spawnedObjects >= maxSpawnableObjects)
            {
                OnSpawnedAllEnemies.Invoke();
                CancelInvoke();
                enabled = false;
            }
        }
        else
        {
            //Debug.Log("No mesh found to spawn on", gameObject);
        }
    }

    public void StopSpawning()
    {
        CancelInvoke();
    }

    public void SetSpawnState(bool spawnState)
    {
        if (!spawnState) StopSpawning();
        canSpawn = spawnState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
