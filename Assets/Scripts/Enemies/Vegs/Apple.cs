using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public GameObject enemyExplosionPrefab;
    public float distanceTillExplode = 5f;
    private Transform player;
    private bool isExplode;

    public AudioSource explosionSFX;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDir3 = player.transform.position - this.transform.position; 
        float distanceToPlayer = (new Vector2(playerDir3.x, playerDir3.y)).magnitude;
        if (distanceToPlayer <= distanceTillExplode && !isExplode) {
            isExplode = true;
            SelfDestruct();
        }

    }

    void SelfDestruct() {

        StartCoroutine(Destroy());
        SpawnExplosion();
        SpawnExplosion();
        // SpawnExplosion();
    }

    IEnumerator Destroy()
    {
        explosionSFX.Play();
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

    void SpawnExplosion() {
        Instantiate(enemyExplosionPrefab, new Vector3(
            transform.position.x + Random.Range(-1f, 1f),
            transform.position.y + Random.Range(-1f, 1f),
            transform.position.z
        ),Quaternion.identity);
    }
}
