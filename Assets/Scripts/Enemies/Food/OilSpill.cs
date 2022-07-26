using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : MonoBehaviour
{
    void Awake() {
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 angle = new Vector3(0, 0, Random.Range(0, 360));
        float scaleFactor = Random.Range(0.5f, 1.2f);
        Vector3 scale = new Vector3(scaleFactor, scaleFactor, 0);
        this.transform.localScale = scale;
        this.transform.eulerAngles = angle;
        Invoke("SelfDestruct", 1.5f);
    }

    void SelfDestruct() {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
