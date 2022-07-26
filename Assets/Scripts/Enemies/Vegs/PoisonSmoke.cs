using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmoke : MonoBehaviour
{
    public float lifeTime = 3f;

    private void Start() {
        Destroy(gameObject, lifeTime);
    }
}
