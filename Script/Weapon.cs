using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;

    //shooting
    public bool isShooting, ReadyToShoot;
    bool allowReset = true;
    public float shootingDelay = 1f;

    //burst
    public int bulletsPerBurst = 3;
    public int currentBurst;

    //spread
    public float spreadIntensity;

    //bullet
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletspeed = 20.0f;
    public float bulletPrefabLifeTime = 2.0f;

    // effects
    public GameObject muzzleFlashPrefab;

    public enum FireMode { Single, Burst, Auto };
    public FireMode currentFireMode;

    private void Awake()
    {
        ReadyToShoot = true;
        currentBurst = bulletsPerBurst;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFireMode == FireMode.Auto)
        {
            // holding down the mouse
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentFireMode == FireMode.Single || currentFireMode == FireMode.Burst)
        {
            // clicking the mouse
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (ReadyToShoot && isShooting)
        {
            currentBurst = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        ReadyToShoot = false;

        // Instantiate muzzle flash at the fire point
        if (muzzleFlashPrefab)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(muzzleFlash, 0.5f); // Destroy muzzle flash after a short time
        }

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward.normalized * bulletspeed, ForceMode.Impulse);
        StartCoroutine(DestroyBullet(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // burst mode
        if (currentFireMode == FireMode.Burst && currentBurst > 1)
        {
            currentBurst--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void ResetShot()
    {
        ReadyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if(Physics.Raycast(ray, out hit)){
                targetPoint = hit.point;
            } else {
                targetPoint = ray.GetPoint(100);
            }

            Vector3 direction = targetPoint - firePoint.position;

            float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
            float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

            return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBullet(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
