using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    const string BOSS_IDLE = "BossIdle";
    const string BOSS_SHOOT = "BossShoot";
    const string BOSS_CHASE = "BossChase";
    const string BOSS_SUMMON = "BossSummon";
    const string BOSS_CHARGE = "BossCharge";
    private string[] attackStates = {"circle", "chase", "follow", "summon", "spiral", "charge"};
    // private string[] attackStates = {"circle"};

    private Animator animator;
    public GameObject spriteAnimator;
    private SpriteRenderer spriteRenderer;
    private string currentState;
    private Transform playerPos;
    private Transform startPos;
    private int attackIndex = 0;
    private int spiralAngle = 0;

    public float idleDuration = 2f;
    public float shootDuration = 2f;
    public float chaseDuration = 5f;
    public float summonDuration = 2f;
    public float chargeDuration = 10f;
    public float chaseSpeed = 4f;
    public float chargeSpeed = 10f;
    public int enemiesSpawnNum = 3;
    public int shootCirclePatternNum = 5;
    public int shootWaveNum = 30;
    public float alertTime = 1f;
    public int chargeNum = 4;

    public Vector3 spawnPos;
    public float minSpawnX;
    public float maxSpawnX;
    public float minSpawnY;
    public float maxSpawnY;
    
    public GameObject bulletPrefab;
    public float bulletSpeed = 3f;
    public GameObject alertPrefab;
    public ParticleSystem explosion;
    public GameObject[] enemyPrefabs;

    private Vector3 chargeTarget;

    public float maxhealth = 100f;
    private float health;

    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void Awake() {
        spawnPos = transform.position;
        animator = spriteAnimator.GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = this.gameObject.transform;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        health = maxhealth;

        Invoke(nameof(IdleState), 1f);
    }

    private void IdleState() {
        ChangeAnimationState(BOSS_IDLE);
        Invoke(nameof(ChooseState), idleDuration);
    }

    private void ChooseState() {
        if (attackIndex > attackStates.Length - 1) {
            attackIndex = 0;
            spiralAngle = 0;
        }
        string chosenState = attackStates[attackIndex];
        if (chosenState == "chase") {
            ChangeAnimationState(BOSS_CHASE);
            Invoke(nameof(IdleState), chaseDuration);
        }
        if (chosenState == "circle") {
            int numToShoot = shootCirclePatternNum;
            ChangeAnimationState(BOSS_SHOOT);
            Invoke(nameof(IdleState), shootDuration * 2.2f);
            while (numToShoot > 0) {
                ShootingCircles(numToShoot);
                numToShoot--;
            }
        }
        if (chosenState == "follow") {
            int numToShoot = shootWaveNum;
            ChangeAnimationState(BOSS_SHOOT);
            Invoke(nameof(IdleState), shootDuration);
            while (numToShoot > 0) {
                ShootingFollow(numToShoot + 15);
                numToShoot--;
            }
        }
        if (chosenState == "spiral") {
            int numToShoot = shootWaveNum;
            ChangeAnimationState(BOSS_SHOOT);
            Invoke(nameof(IdleState), shootDuration);
            while (numToShoot > 0) {
                ShootingSpiral(numToShoot);
                numToShoot--;
            }
        }
        if (chosenState == "summon") {
            int enemiesToSummon = enemiesSpawnNum;
            ChangeAnimationState(BOSS_SUMMON);
            Invoke(nameof(IdleState), summonDuration);
            while (enemiesToSummon > 0) {
                Summoning(enemiesToSummon);
                enemiesToSummon--;
            }
        }
        if (chosenState == "charge") {
            int charges = chargeNum;
            ChangeAnimationState(BOSS_CHARGE);
            Invoke(nameof(IdleState), chargeDuration);
            while (charges > 0) {
                Charging(charges);
                charges--;
            }
        }
        attackIndex++;
    }
    private void ShootingFollow(int delay) {
        Invoke(nameof(SpawnBulletAtPlayer), delay * 0.2f);
    }
    
    private void ShootingCircles(int delay) {
        Invoke(nameof(SpawnBulletCircle), delay * 2.5f);
    }

    private void ShootingSpiral(int delay) {
        Invoke(nameof(SpawnBulletSpiral), delay * 0.2f);
    }

    private void SpawnBulletSpiral() {
        Vector2 shootDir = Random.insideUnitCircle.normalized;
        float mainAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < 3; i++) {
            float mainAngles = spiralAngle + i * 120f;
            for (int j = 0; j < 1; j++) {
                float angle = mainAngles + j * 15f;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                bulletRB.rotation = angle;
                Vector2 bulletShootDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                bulletRB.AddForce(bulletShootDir * bulletSpeed * 1.5f, ForceMode2D.Impulse);
            }
        }
        spiralAngle += 10;
    }

    private void SpawnBulletAtPlayer() {
        Vector3 targetDir = playerPos.position - transform.position;
        float offset = Random.Range(-1, 1);
        Vector2 bulletShootDir = new Vector2(targetDir.x + offset, targetDir.y + offset).normalized;
        float angle = Mathf.Atan2(bulletShootDir.y, bulletShootDir.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.rotation = angle;
        bulletRB.AddForce(bulletShootDir * bulletSpeed * 5f, ForceMode2D.Impulse);
    }

    private void SpawnBulletCircle() {
        // Vector2 shootDir = Random.insideUnitCircle.normalized;
        // float mainAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        // float perpendicularAngle = mainAngle + 90f;

        float randomAngle = Random.Range(-45, 45);

        for (int i = 0; i < 42; i++) {
            float angle = -20 + randomAngle - i * 8.75f;
            if (((i > 11 && i < 29) || i < 9 || i > 31) ) {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                bulletRB.rotation = angle;
                Vector2 bulletShootDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                bulletRB.AddForce(bulletShootDir * bulletSpeed, ForceMode2D.Impulse);
            }
        }
    }

    private bool CheckValidSpawn(Vector3 spawnPos)
    {
        return !Physics.CheckSphere(spawnPos, 1);
    }

    private void Summoning(int delay) {
        Invoke(nameof(SpawnAlert), delay * 1.5f);
    }

    private void Charging(int delay) {
        Invoke(nameof(ChargeTowardsPlayer), delay * 1.2f);
    }

    private void ChargeTowardsPlayer() {
        // Vector3 chargeDir = playerPos.position - transform.position;
        chargeTarget = playerPos.position + (playerPos.position - transform.position).normalized * 2f;
    }

    private void SpawnAlert() {
        Vector3 spawnPos = new Vector3(Random.Range(minSpawnX, maxSpawnX), Random.Range(minSpawnY, maxSpawnY), 0);
        while (!CheckValidSpawn(spawnPos))
        {
            spawnPos = new Vector3(Random.Range(minSpawnX, maxSpawnX), Random.Range(minSpawnY, maxSpawnY), 0);
        }

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject alert = Instantiate(alertPrefab, spawnPos, Quaternion.identity);
        alert.GetComponent<SpawnEnemyFromAlert>().enemyPrefab = enemyPrefabs[randomIndex];
        alert.GetComponent<SpawnEnemyFromAlert>().spawnPos = spawnPos;
        alert.GetComponent<SpawnEnemyFromAlert>().spawnTimer = alertTime;
    }

    private void Update() {
        if (currentState == BOSS_CHASE) {
            Vector3 targetDir = playerPos.position - transform.position;
            Vector2 moveDir = new Vector2(targetDir.x, targetDir.y).normalized;

            // Rotate the sprite accordingly to move direction (left-right)
            if (moveDir.x < 0) {
                spriteRenderer.flipX = false;
            } else {
                spriteRenderer.flipX = true;
            }

            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, chaseSpeed * Time.deltaTime);
        }
        if (currentState == BOSS_IDLE) {
            transform.position = Vector2.MoveTowards(transform.position, spawnPos, chaseSpeed * 2 * Time.deltaTime);
        }
        if (health <= 0) {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
        if (currentState == BOSS_CHARGE) { 
            var step = chargeSpeed * Time.deltaTime; 
            transform.position = Vector3.MoveTowards(transform.position, chargeTarget, step);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            if (health > 0)
            {
                health -= collision.gameObject.GetComponent<Bullet>().damage;
            }
            if (health <= 0)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
}
