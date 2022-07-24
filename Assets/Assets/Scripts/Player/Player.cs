using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // -----------PLAYER AIMING-----------
    public Camera cam;
    Vector2 mousePos;
    private Rigidbody2D rb;
    public Transform gun;
    public SpriteRenderer playerSprite;

    // -----------PLAYER MOVEMENT-----------
    public float moveSpeed = 5f;
    public Animator animator;
    private float moveX;
    private float moveY;
    private Vector2 moveDir;

    // -----------PLAYER SHOOT & ENERGY-----------
    public GameObject waterPrefab;
    public Transform nozzle;
    public float waterSpeed = 5f;
    public float shootOffset = 2f;
    public float waterMinScale = 0.5f;
    public float waterMaxScale = 2f;
    public float energy = 100;
    public float energyRegenRate = 0.5f;
    public float shootingCost = 1f;
    public Image energyBar;

    // -----------PLAYER START-----------
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        // InvokeRepeating("EnergyRegen", 0, energyRegenRate);
        // GameEvents.GameStartGetPlayer(new onEventData(gameObject));
    }

    // -----------PLAYER FIXED UPDATE-----------
    private void FixedUpdate() {
        RotateGun();
        // SHOOTING
        if (Input.GetKey(KeyCode.Mouse0) && energy > 0) {
            Shoot();
        }
    }
    // -----------PLAYER UPDATE-----------
    void Update()
    {
        // MOVING
        ProcessInput();
        Move();

        // AIMING
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        
        energyBar.fillAmount = energy / 100f;
    }
    // -----------PLAYER COLLISION CHECK-----------
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy") {
            // GameEvents.PlayerHitEvent(new onEventData(gameObject));
        }
    }
    // -----------PLAYER AIMING-----------
    private void RotateGun() {
        // Get the look direction the player is looking
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gun.eulerAngles = new Vector3(0, 0, angle);
        // Rotate the player's sprite
        if (angle > 90 || angle < -90) {
            playerSprite.flipX = true;
        } else {
            playerSprite.flipX = false;
        }
    }
    // -----------PLAYER SHOOTING & ENERGY RELATED-----------
    void Shoot() {
        energy -= shootingCost;
        // Spawn a water drop
        float waterScale = Random.Range(waterMinScale, waterMaxScale);
        GameObject water = Instantiate(waterPrefab, nozzle.position, nozzle.rotation);
        water.transform.localScale = new Vector3(waterScale, waterScale, 0);
        Rigidbody2D waterRB = water.GetComponent<Rigidbody2D>();
        // Randomized the direction of shooting
        Vector2 nozzleAngle = nozzle.right;
        float shootAngle = Mathf.Atan2(nozzleAngle.y, nozzleAngle.x) * Mathf.Rad2Deg;
        float randomOffset = Random.Range(-shootOffset, shootOffset);
        shootAngle += randomOffset;
        Vector2 shootDir = (Vector2)(Quaternion.Euler(0,0,shootAngle) * Vector2.right);
        // Add force to the water droplets
        // waterRB.AddForce(shootDir * waterSpeed, ForceMode2D.Impulse);
        waterRB.velocity = shootDir * waterSpeed;
    }
    private void EnergyRegen() {
        if (energy < 100) {
            energy += 1;
        }
    }
    // -----------PLAYER MOVEMENT-----------
    void ProcessInput() {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        // normalized: returns a vector with magnitude of 1
        moveDir = new Vector2(moveX, moveY).normalized;
    }
    void Move() {
        rb.velocity = moveDir * moveSpeed;
        animator.SetFloat("Speed", moveDir.magnitude);
    }

}
