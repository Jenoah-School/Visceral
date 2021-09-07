using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRange = 5f;
    [SerializeField] private float spawnDelay = 4f;
    [SerializeField] private LayerMask spawnableLayers;
    [SerializeField] private int maxSpawnableObjects = 5;
    [SerializeField] private UnityEvent OnSpawnedAllEnemies;
    [SerializeField] private GameObject objectToSpawn = null;


    private int spawnedObjects = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (objectToSpawn == null) enabled = false;
        InvokeRepeating("TrySpawnEnemy", spawnDelay, spawnDelay);
    }

    private void TrySpawnEnemy()
    {
        Vector3 spawnPosition = Random.insideUnitCircle * spawnRange;
        spawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y) + transform.position;
        if (Physics.Raycast(spawnPosition + Vector3.up, Vector3.down, out RaycastHit hit, 2f, spawnableLayers))
        {
            spawnPosition = hit.point;

            GameObject spawnedObject = Instantiate(objectToSpawn);
            spawnedObject.transform.position = spawnPosition;

            spawnedObjects++;

            if(spawnedObjects >= maxSpawnableObjects)
            {
                OnSpawnedAllEnemies.Invoke();
                CancelInvoke();
                enabled = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
