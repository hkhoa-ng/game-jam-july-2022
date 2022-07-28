using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : MonoBehaviour
{
    public GameObject oilSpillPrefab;
    private Transform burgerTransform;

    void SpawnOil() {
        Instantiate(oilSpillPrefab, transform.position, new Quaternion(0, 0, 0, 0));
        
    }
    void Awake() {
        burgerTransform = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnOil", 0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
