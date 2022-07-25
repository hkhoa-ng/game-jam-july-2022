using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTransition : MonoBehaviour
{
    private bool isActive;
    private SpriteRenderer spriteRenderer;

    // New Boundary offsets
    private FollowPlayer cameraFollow;
    [SerializeField] private Vector2 camMaxChange;
    [SerializeField] private Vector2 camMinChange;
    [SerializeField] private Vector3 playerChange;

    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = Camera.main.GetComponent<FollowPlayer>();
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cameraFollow.setNewBoundary(camMinChange, camMaxChange);

            collision.transform.position += playerChange;
        }
    }
}
