using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTransition : MonoBehaviour
{
    public bool isActive;
    public bool isOpenable;
    public GameObject sprite;

    // Indicator for boss room portal
    public GameObject bossRoomMarker;
    public bool isBossRoom;

    // New Boundary offsets
    [SerializeField] private Vector3 playerChange;

    // GameManager
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        isBossRoom = false;
        bossRoomMarker.SetActive(false);
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isActive = false;
        isOpenable = false;
        sprite.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            sprite.SetActive(false);
            if (isBossRoom)
            {
                bossRoomMarker.SetActive(true);
            }
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
                collision.transform.position += playerChange;
                // Trigger room event
                if (playerChange.x == 0)
                {
                    gameManager.currentIndex -= 4;
                    gameManager.OnRoomEnter(gameManager.currentIndex);
                }
                else
                {
                    gameManager.currentIndex++;
                    gameManager.OnRoomEnter(gameManager.currentIndex);
                }
            }
            else if (((collision.gameObject.transform.position.x > transform.position.x && playerChange.y == 0) ||
                (collision.gameObject.transform.position.y > transform.position.y && playerChange.x == 0)))
            {
                collision.transform.position -= playerChange;
                // Trigger Room event
                if (playerChange.x == 0)
                {
                    gameManager.currentIndex += 4;
                    gameManager.OnRoomEnter(gameManager.currentIndex);
                }
                else
                {
                    gameManager.currentIndex--;
                    gameManager.OnRoomEnter(gameManager.currentIndex);
                }
            }
            else
            {

            }
        }
    }
}
