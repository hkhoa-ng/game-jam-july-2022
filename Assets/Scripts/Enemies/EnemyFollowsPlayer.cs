using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowsPlayer : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    public float moveSpeed = 1f;
    public SpriteRenderer spriteRenderer;
    // public ParticleSystem explosion;
    private void Awake() {
        // GameEvents.onPlayerDieDestroyObjects += SelfDestruct;
    }
    private void OnDisable() {
        
        // GameEvents.onPlayerDieDestroyObjects -= SelfDestruct;
    }
    private void SelfDestruct() {
        Destroy(gameObject);
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        rb = GetComponent<Rigidbody2D>();
        
        // screenShake = gameManager.GetComponent<ScreenShake>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() { 
        MoveTowardPlayer();
    }


    private void MoveTowardPlayer() {
        // Get the direction of the player to move toward them
        Vector3 targetDir = player.position - transform.position;
        Vector2 moveDir = new Vector2(targetDir.x, targetDir.y).normalized;
        rb.velocity = moveDir * moveSpeed;

        // Rotate the sprite accordingly to move direction (left-right)
        if (rb.velocity.x > 0) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }
}
