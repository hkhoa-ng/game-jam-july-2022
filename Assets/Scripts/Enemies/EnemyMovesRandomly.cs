using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovesRandomly : MonoBehaviour
{
    private Vector3 direction;
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector3 lastVelocity;
    public float health = 50f;
    private SpriteRenderer spriteRenderer;
    // public ParticleSystem explosion;
    private void Awake() {
        // GameEvents.onPlayerDieDestroyObjects += SelfDestruct;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnDisable() {
        
        // GameEvents.onPlayerDieDestroyObjects -= SelfDestruct;
    }
    private void SelfDestruct() {
        // Destroy(gameObject);
        // GameEvents.Screenshake();
        // Instantiate(explosion, transform.position, Quaternion.identity);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0).normalized;
        rb.velocity = direction * moveSpeed;
    }
    private void FixedUpdate() {
        lastVelocity = rb.velocity;

        // Rotate the sprite accordingly to move direction (left-right)
        if (rb.velocity.x > 0) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Walls")) {
            
        }
        if (other.gameObject.tag == "Water") {
            health -= 1;
            Destroy(other.gameObject);
        } else {
            Vector2 wallNormal = other.contacts[0].normal;
            Vector2 inDir = new Vector2(lastVelocity.x, lastVelocity.y).normalized;
            direction = Vector2.Reflect(inDir, wallNormal).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }
    private void Update() {
        if (health <= 0) {
            Destroy(gameObject);
            // GameEvents.Screenshake();
            // Instantiate(explosion, transform.position, Quaternion.identity);
            // GameEvents.EnemyDie();
        }
    }
}
