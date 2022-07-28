using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float damping;
    private Camera cam;

    private Vector3 velocity = Vector3.zero;

    private bool foundPlayer = false;

    void Awake() {
        cam = Camera.main;
    }

    void FixedUpdate() 
    {   
        if (!foundPlayer) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            foundPlayer = true;
        } 
        if (target != null) {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 characterPos = target.position + offset;
            Vector3 movePos = new Vector3(
                (characterPos.x + mousePos.x) / 2,
                (characterPos.y + mousePos.y) / 2,
                characterPos.z
            );
            transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, damping);
        } else {
            foundPlayer = false;
        }
    }
}
