using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthMonitor : MonoBehaviour
{
    public float health = 6;
    public ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "PlayerBullet") {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
            Destroy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
