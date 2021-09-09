using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private float refreshTime = 0.5f;
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Label settings")]
    [SerializeField] private TMPro.TextMeshProUGUI countLabel = null;
    [SerializeField] private string labelPrefix = "Enemies left: ";

    int enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateEnemyCountLabel", refreshTime, refreshTime);
    }

    // Update is called once per frame
    public int GetEnemyCount()
    {
        int actualEnemyCount = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach(GameObject enemy in enemies)
        {
            if(enemy.TryGetComponent(out EntityHealth enemyHealth) && enemyHealth.health > 0)
            {
                actualEnemyCount++;
            }
        }
        return actualEnemyCount;
    }

    public void UpdateEnemyCountLabel()
    {
        enemyCount = GetEnemyCount();
        if (countLabel != null) countLabel.text = labelPrefix + enemyCount;
    }
}
