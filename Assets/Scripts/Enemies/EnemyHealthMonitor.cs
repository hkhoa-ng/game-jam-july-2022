using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthMonitor : MonoBehaviour
{
    public int health = 3;
    public ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "PlayerBullet") {
            health -= 1;
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
