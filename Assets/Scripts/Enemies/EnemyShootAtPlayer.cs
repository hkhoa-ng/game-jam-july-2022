using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootAtPlayer : MonoBehaviour
{
    public GameObject enemyBulletPrefab;
    private Transform player;
    public float bulletSpeed = 2f;
    public float shootInterval;

    private void Shoot() {
        // Get the direction of the player to shoot toward them
        Vector3 targetDir = player.position - transform.position;
        Vector2 shootDir = new Vector2(targetDir.x, targetDir.y).normalized;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.rotation = angle;
        bulletRB.AddForce(shootDir * bulletSpeed, ForceMode2D.Impulse);
    }
    // Start is called before the first frame update
    void Start()
    {
        shootInterval = Random.Range(2f, 4f);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("Shoot", 1.5f, shootInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
