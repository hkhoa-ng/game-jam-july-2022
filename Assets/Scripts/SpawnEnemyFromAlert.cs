using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyFromAlert : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Vector3 spawnPos;
    public float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {   
        Invoke("SpawnAnEnemy", spawnTimer + 0.2f);
    }

    void SpawnAnEnemy() {
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        if (enemyPrefab.CompareTag("Boss"))
        {
            enemy.GetComponent<BossAI>().minSpawnX = spawnPos.x - 5;
            enemy.GetComponent<BossAI>().minSpawnY = spawnPos.y - 5;
            enemy.GetComponent<BossAI>().maxSpawnX = spawnPos.x + 5;
            enemy.GetComponent<BossAI>().maxSpawnY = spawnPos.y + 5;
         }
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
