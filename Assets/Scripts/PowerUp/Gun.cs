using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // 0: Pistol; 1: Shotgun; 2: Rifle
    public GameObject[] gunPrefabs;
    [SerializeField] private int index;

    private bool inCollider;
    private GameObject player;

    public AudioSource switchSFX;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && inCollider)
        {
            int currentGun;
            currentGun = player.GetComponent<PlayerMovement>().gunIndex;
            switchSFX.Play();
            player.GetComponent<PlayerMovement>().changeGun(index);
            Instantiate(gunPrefabs[currentGun], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inCollider = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inCollider = false;
        }
    }
}
