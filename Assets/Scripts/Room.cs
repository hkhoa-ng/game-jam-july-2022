using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject portalTop;
    public GameObject portalBottom;
    public GameObject portalLeft;
    public GameObject portalRight;
    public bool isInitialised;
    
    // Start is called before the first frame update
    void Start()
    {
        isInitialised = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
