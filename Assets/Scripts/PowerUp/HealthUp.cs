using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    [SerializeField] private float healthUp = 1f;
    public AudioSource pickupSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            pickupSFX.Play();
            collision.gameObject.GetComponent<PlayerMovement>().SetHealth(healthUp);
            Destroy(gameObject);
        }
    }
}
