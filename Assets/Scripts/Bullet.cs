using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explodePrefab;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wall") 
            || collision.gameObject.CompareTag("Enemy") 
            || collision.gameObject.CompareTag("EnemyBullet")
            || collision.gameObject.CompareTag("Boss"))
        {
        }
        Destroy(gameObject);
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
    }
}
