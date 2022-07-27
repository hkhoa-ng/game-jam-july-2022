using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    public float knockbackForce = 100f;
    public Vector2 knockbackDir = Vector2.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Enemy") 
            || other.gameObject.CompareTag("EnemyBullet"))
        {
            Vector3 knockbackDir3 = transform.position - other.transform.position;
            knockbackDir = new Vector2(knockbackDir3.x, knockbackDir3.y);
            rb.AddForce(knockbackDir * knockbackForce);
        }
    }
}
