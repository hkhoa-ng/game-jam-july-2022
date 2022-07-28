using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rigidBody;
    public Camera cam;
    private SpriteRenderer playerSprite;

    // Gun Systems (0: Pistol; 1: Shotgun; 2: Rifle)
    public int gunIndex;
    public GameObject[] gunSystem;
    public SpriteRenderer gunSprite;

    private Vector2 direction;
    private Vector2 mousePos;
    public Transform gun;

    private float angle;

    private const string PLAYER_IDLE = "PlayerIdle";
    private const string PLAYER_RUN = "PlayerRun";
    private const string PLAYER_HURT = "PlayerHurt";
    private Animator animator;
    private bool isRunning = false;
    private string currentState;

    // Stat modifier
    [SerializeField] private float maxHealth = 10;
    public float health;
    public float damageModifier;

    // Knockback
    [SerializeField] private float knockbackForce = 10f;
    public Vector2 moveDir;
    public Vector3 targetDir;

    // Invincible frame
    private bool isHurting;
    [SerializeField] private float invincibleSeconds = 1;

    // Text UI
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI bulletText;

    void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void Awake() {
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        
        int gunIndex = 0;
        gunSystem[gunIndex].SetActive(true);
        gunSprite = gunSystem[gunIndex].GetComponentInChildren<SpriteRenderer>();
        for (int i = 1; i < gunSystem.Length; i++)
        {
            gunSystem[i].SetActive(false);
        }
        cam = Camera.main;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        isHurting = false;
        health = maxHealth;
        damageModifier = 0;
        rigidBody = GetComponent<Rigidbody2D>();
        direction = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        isRunning = direction.magnitude > 0;

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        healthText.text = "Health: " + health;
        bulletText.text = "Bullet: " + gunSystem[gunIndex].GetComponent<GunSystem>().bulletsLeft;
    }

    private void FixedUpdate()
    {
        // Moving the player
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

        // Update animation
        if (isHurting)
        {
            ChangeAnimationState(PLAYER_HURT);
        }
        else if (isRunning) {
            ChangeAnimationState(PLAYER_RUN);
        } else {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    public void changeGun(int newGunIndex)
    {
        if (newGunIndex < gunSystem.Length && newGunIndex >= 0) {
            gunSystem[gunIndex].SetActive(false);
            gunSystem[newGunIndex].SetActive(true);
            gunIndex = newGunIndex;
            gunSprite = gunSystem[gunIndex].GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void SetHealth(float healthAdd)
    {
        health += healthAdd;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") 
            || collision.gameObject.CompareTag("Boss") 
            || collision.gameObject.CompareTag("EnemyBullet") ) 
            && (!isHurting))
        {
            health -= 1;
            isHurting = true;
            StartCoroutine(Hurting());
            Vector3 targetDir = transform.position - collision.gameObject.transform.position;
            moveDir = new Vector2(targetDir.x, targetDir.y).normalized;

            rigidBody.AddForce(moveDir * knockbackForce, ForceMode2D.Force);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy")
    || collision.gameObject.CompareTag("Boss")
    || collision.gameObject.CompareTag("EnemyBullet")) && !isHurting)
        {
            health -= 1;
            isHurting = true;
            StartCoroutine(Hurting());
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector3 targetDir = transform.position - collision.gameObject.transform.position;
            moveDir = new Vector2(targetDir.x, targetDir.y).normalized;
            rigidBody.AddForce(moveDir * knockbackForce, ForceMode2D.Force);
        }
    }

    IEnumerator Hurting()
    {
        yield return new WaitForSeconds(invincibleSeconds);
        isHurting = false;

    }
}
