using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTransition : MonoBehaviour
{
    public bool isActive;
    public GameObject sprite;

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
        sprite.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            sprite.SetActive(false);
        }
        else
        {
            sprite.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActive)
        {
            Rigidbody2D rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if ((collision.gameObject.transform.position.x < transform.position.x && playerChange.y == 0) ||
                (collision.gameObject.transform.position.y < transform.position.y && playerChange.x == 0))
            {
                cameraFollow.setNewBoundary(camMinChange, camMaxChange);
                collision.transform.position += playerChange;
            }
            else if (((collision.gameObject.transform.position.x > transform.position.x && playerChange.y == 0) ||
                (collision.gameObject.transform.position.y > transform.position.y && playerChange.x == 0)))
            {
                cameraFollow.setNewBoundary(-camMinChange, -camMaxChange);
                collision.transform.position -= playerChange;
            }
            else
            {

            }
        }
    }
}
