using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explodePrefab;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Walls") 
            || collision.gameObject.CompareTag("Enemy") 
            || collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(gameObject);
        }
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
    }
}
