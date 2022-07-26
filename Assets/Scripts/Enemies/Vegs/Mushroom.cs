using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public GameObject poisonSmokePrefab;
    public float smokeSpawnInterval = 4f;

    void SpawnSmoke() {
        Instantiate(poisonSmokePrefab, transform.position, new Quaternion(0, 0, 0, 0));
        
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnSmoke", 0f, smokeSpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
