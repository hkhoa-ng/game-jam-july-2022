using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explodePrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Walls") 
            || collision.gameObject.CompareTag("Enemy") 
            || collision.gameObject.CompareTag("EnemyBullet")
            || collision.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
    }
}
