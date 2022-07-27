using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    const string BOSS_IDLE = "BossIdle";
    const string BOSS_SHOOT = "BossShoot";
    const string BOSS_CHASE = "BossChase";
    const string BOSS_SUMMON = "BossSummon";
    private string[] attackStates = {"shoot", "chase", "summon"};

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private string currentState;
    private Transform playerPos;
    private Transform startPos;
    private int attackIndex = 0;

    public float idleDuration = 2f;
    public float shootDuration = 2f;
    public float chaseDuration = 5f;
    public float summonDuration = 2f;
    public float chaseSpeed = 4f;
    public int enemiesSpawnNum = 3;
    public int shootNum = 5;
    public float alertTime = 1f;

    public float minSpawnX;
    public float maxSpawnX;
    public float minSpawnY;
    public float maxSpawnY;
    
    public GameObject bulletPrefab;
    public float bulletSpeed = 3f;
    public GameObject alertPrefab;
    public ParticleSystem explosion;
    public GameObject[] enemyPrefabs;

    public float health = 3000f;

    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = this.gameObject.transform;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();

        Invoke(nameof(IdleState), 1f);
    }

    private void IdleState() {
        ChangeAnimationState(BOSS_IDLE);
        Invoke(nameof(ChooseState), idleDuration);
    }

    private void ChooseState() {
        if (attackIndex > attackStates.Length - 1) {
            attackIndex = 0;
        }
        string chosenState = attackStates[attackIndex];
        if (chosenState == "chase") {
            ChangeAnimationState(BOSS_CHASE);
            Invoke(nameof(IdleState), chaseDuration);
        }
        if (chosenState == "shoot") {
            int numToShoot = shootNum;
            ChangeAnimationState(BOSS_SHOOT);
            Invoke(nameof(IdleState), shootDuration);
            while (numToShoot > 0) {
                Shooting(numToShoot);
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
        attackIndex++;
    }
    
    private void Shooting(int delay) {
        Invoke(nameof(SpawnBullet), delay * 1.2f);
    }

    private void SpawnBullet() {
        Vector2 shootDir = Random.insideUnitCircle.normalized;
        float mainAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        float perpendicularAngle = mainAngle + 90f;

        float randomAngle = Random.Range(-20, 20);

        for (int i = 0; i < 42; i++) {
            float angle = -20 + randomAngle - i * 8.75f;
            if (i > 10 || i < 9) {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                bulletRB.rotation = angle;
                Vector2 bulletShootDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                bulletRB.AddForce(bulletShootDir * bulletSpeed, ForceMode2D.Impulse);
            }
        }
        // float randomAngle = 0;
        // float angle1 = -20 + randomAngle;
        // float angle2 = -55 + randomAngle;
        // float angle3 = -90 + randomAngle;
        // float angle4 = -125 + randomAngle;
        // float angle5 = -160 + randomAngle;

        // GameObject bullet1 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // GameObject bullet2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // GameObject bullet3 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // GameObject bullet4 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // GameObject bullet5 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Rigidbody2D bullet1RB = bullet1.GetComponent<Rigidbody2D>();
        // Rigidbody2D bullet2RB = bullet2.GetComponent<Rigidbody2D>();
        // Rigidbody2D bullet3RB = bullet3.GetComponent<Rigidbody2D>();
        // Rigidbody2D bullet4RB = bullet4.GetComponent<Rigidbody2D>();
        // Rigidbody2D bullet5RB = bullet5.GetComponent<Rigidbody2D>();

        // bullet1RB.rotation = angle1;
        // bullet2RB.rotation = angle2;
        // bullet3RB.rotation = angle3;
        // bullet4RB.rotation = angle4;
        // bullet5RB.rotation = angle5;

        // Vector2 shootDir1 = new Vector2(Mathf.Cos(angle1 * Mathf.Deg2Rad), Mathf.Sin(angle1 * Mathf.Deg2Rad));
        // Vector2 shootDir2 = new Vector2(Mathf.Cos(angle2 * Mathf.Deg2Rad), Mathf.Sin(angle2 * Mathf.Deg2Rad));
        // Vector2 shootDir3 = new Vector2(Mathf.Cos(angle3 * Mathf.Deg2Rad), Mathf.Sin(angle3 * Mathf.Deg2Rad));
        // Vector2 shootDir4 = new Vector2(Mathf.Cos(angle4 * Mathf.Deg2Rad), Mathf.Sin(angle4 * Mathf.Deg2Rad));
        // Vector2 shootDir5 = new Vector2(Mathf.Cos(angle5 * Mathf.Deg2Rad), Mathf.Sin(angle5 * Mathf.Deg2Rad));

        // bullet1RB.AddForce(shootDir1 * bulletSpeed, ForceMode2D.Impulse);
        // bullet2RB.AddForce(shootDir2 * bulletSpeed, ForceMode2D.Impulse);
        // bullet3RB.AddForce(shootDir3 * bulletSpeed, ForceMode2D.Impulse);
        // bullet4RB.AddForce(shootDir4 * bulletSpeed, ForceMode2D.Impulse);
        // bullet5RB.AddForce(shootDir5 * bulletSpeed, ForceMode2D.Impulse);
    }

    private bool CheckValidSpawn(Vector3 spawnPos)
    {
        return !Physics.CheckSphere(spawnPos, 1);
    }

    private void Summoning(int delay) {
        Invoke(nameof(SpawnAlert), delay * 1.5f);
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
            Vector3 targetDir = playerPos.position - animator.transform.position;
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
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(0, 3, 0), chaseSpeed * Time.deltaTime);
        }
        if (health <= 0) {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
