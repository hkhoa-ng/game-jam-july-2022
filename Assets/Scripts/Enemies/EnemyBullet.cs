using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public AudioSource exposionSFX;
    
    public ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
