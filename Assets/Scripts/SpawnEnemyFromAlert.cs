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
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
