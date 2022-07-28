using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject explodePrefab;
    public GameObject floorObj;
    public int health = 5;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet") 
            || collision.gameObject.CompareTag("EnemyBullet"))
        {
            health--;
        }
    }
    private void FixedUpdate() {
        if (health <= 0) {
            Destroy(gameObject);
            Instantiate(explodePrefab, transform.position, Quaternion.identity);
            Instantiate(floorObj, transform.position, Quaternion.identity);
        }
    }
}
