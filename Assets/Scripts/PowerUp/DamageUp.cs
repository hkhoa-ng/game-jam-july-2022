using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUp : MonoBehaviour
{
    [SerializeField] private float damageUp = 0.5f;
    public AudioSource pickupSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().damageModifier += damageUp;
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        pickupSFX.Play();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
