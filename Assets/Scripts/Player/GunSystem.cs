using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenBulletInAShot, projectileForce;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public int bulletsLeft, bulletsShot;
    public SpriteRenderer gunSprite;
    private Camera cam;
    private PlayerMovement playerController;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Transform firePoint;

    //Graphics
    public ParticleSystem muzzleFlash;
    public GameObject bulletPrefab;
    // public CamShake camShake;
    // public float camShakeMagnitude, camShakeDuration;


    private void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerMovement>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        cam = Camera.main;
    }
    private void Update()
    {
        // Gun aiming
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - (new Vector2(transform.position.x, transform.position.y));
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Rotate the gun's sprite
        if (angle > 90 || angle < -90) {
            gunSprite.flipY = true;
        } else {
            gunSprite.flipY = false;
        }
        MyInput();
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if ((Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) || (bulletsLeft == 0 && !reloading)) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
            bulletsShot = bulletsPerTap;
            Shoot();
            muzzleFlash.Play();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;
        Vector3 shotDir = firePoint.right;
        float aimAngle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        float shotAngle = aimAngle + Random.Range(-spread, spread);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // Adding damage powerup
        bullet.GetComponent<Bullet>().damage = damage + playerController.damageModifier;
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
        // bulletRigidBody.AddForce((firePoint.right + spreadValue) * projectileForce, ForceMode2D.Impulse);

        // Edit this
        bulletRigidBody.rotation = shotAngle;
        Vector2 bulletShootDir = new Vector2(Mathf.Cos(shotAngle * Mathf.Deg2Rad), Mathf.Sin(shotAngle * Mathf.Deg2Rad));
        bulletRigidBody.AddForce(bulletShootDir * projectileForce * 1.5f, ForceMode2D.Impulse);


        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenBulletInAShot);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
