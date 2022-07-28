using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explodePrefab;
    public float damage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wall") 
            || collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
        Instantiate(explodePrefab, transform.position, Quaternion.identity);
    }

    public void setDamage(float newDamage)
    {
        if (newDamage > 0)
        {
            damage = newDamage;
        } 
    }
}
