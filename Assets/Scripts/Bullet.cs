using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explodePrefab;
    public float damage;

    public AudioSource hitSFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Walls") 
            || collision.gameObject.CompareTag("Enemy") 
            || collision.gameObject.CompareTag("Boss"))
        {
            StartCoroutine(Destroy());
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

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
