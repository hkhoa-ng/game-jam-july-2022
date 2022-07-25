using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rigidBody;
    public Camera cam;
    private SpriteRenderer playerSprite;
    public SpriteRenderer gunSprite;

    private Vector2 direction;
    private Vector2 mousePos;
    public Transform gun;

    public float angle;

    void Awake() {
        playerSprite = GetComponent<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        direction = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + direction * moveSpeed * Time.fixedDeltaTime);

        // Gun aiming
        Vector2 lookDir = mousePos - rigidBody.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gun.eulerAngles = new Vector3(0, 0, angle);
        // Rotate the player's sprite
        if (angle > 90 || angle < -90) {
            playerSprite.flipX = true;
            gunSprite.flipY = true;
        } else {
            playerSprite.flipX = false;
            gunSprite.flipY = false;
        }
    }
}
