using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    public int weaponDamage;

    // Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    // burst
    public int bulletPerBurst = 3;
    public int burstBulletsLeft;

    // spread
    public float spreadIntensity;

    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    private Animator animator;

    // Loading 
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;


    // Drop weapon
    public Transform dropPosition;  // Position where the weapon will be dropped
    public GameObject droppedWeaponPrefab;  // Optional: Prefab for the dropped weapon

    // Raycast detection for picking up weapon
    public float pickupRange = 5f;  // Range within which a weapon can be detected
    public LayerMask weaponLayer;   // Layer for weapons

    private GameObject detectedWeapon; // The weapon detected by raycast
    private bool isNearWeapon; // Track if the player is near a weapon

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }
    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        // Prevent shooting and other inputs when the game is paused
        if (PauseMenu.gameIsPaused)
        {
            return;  // Exit the Update function if the game is paused
        }

        if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazineSoundPistol.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
            {
                Reload();
            }

            // Automatically reload when the magazine is empty
            if (bulletsLeft <= 0 && !isReloading)
            {
                Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletPerBurst;
                FireWeapon();
            }
        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft} / {magazineSize}"; // Show current bullets and magazine size
        }



        // Pick up the weapon when "E" is pressed and a weapon is detected
        if (Input.GetKeyDown(KeyCode.E) && detectedWeapon != null)
        {
            PickupWeapon(detectedWeapon);
        }

        // Drop the weapon when "G" is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }
    }


    private void FireWeapon()
    {
        // Only shoot if there are bullets left
        if (bulletsLeft <= 0) return;

        bulletsLeft--; // Reduce the bullets count for every shot

        // Play appropriate muzzle effect and animations
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        // Play sound effects based on weapon type
        if (CompareTag("Shotgun"))
        {
            SoundManager.Instance.shotGunSound.Play();
        }
        else if (CompareTag("Pistol"))
        {
            SoundManager.Instance.shootingSoundPistol.Play();
        }

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;

        // Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        // Destroy the bullet after some time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // BurstMode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }



    private void Reload()
    {
        SoundManager.Instance.reloadingSoundPistol.Play();
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            // Hitting Something
            targetPoint = hit.point;
        }
        else
        {
            // Shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // Returning the direction and spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    // Method to drop the weapon
    private void DropWeapon()
    {
        // Check if the weapon prefab is set for dropping
        if (droppedWeaponPrefab != null && dropPosition != null)
        {
            // Instantiate the dropped weapon at the specified drop position
            Instantiate(droppedWeaponPrefab, dropPosition.position, dropPosition.rotation);
        }

        // Deactivate or destroy the weapon in the player's hand
        gameObject.SetActive(false);
    }

    // Method to pick up a weapon
    private void PickupWeapon(GameObject weapon)
    {
        // Enable the weapon and set it as active
        weapon.SetActive(true);
    }

    // Method to detect weapons using raycast from the center of the screen
    
}
