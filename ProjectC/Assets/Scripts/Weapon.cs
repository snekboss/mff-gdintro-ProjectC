using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public int curAmmoInMag;
    public int magazineCapacity;
    public int remainingAmmo;

    [Range(0.0f, 3.0f)]
    public float fireRate;

    [Range(0.0f, 5.0f)]
    public float reloadTime;

    bool isFiring;
    bool isReloading;

    public bool ownedByPlayer = false;

    public void Fire()
    {
        if (!isReloading && curAmmoInMag <= 0)
        {
            if (remainingAmmo > 0)
            {
                Reload();
            }

            return;
        }

        if (!isFiring && !isReloading)
        {
            curAmmoInMag--;

            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = projectileSpawnPoint.position;
            projectile.transform.rotation = projectileSpawnPoint.transform.rotation;

            if (ownedByPlayer)
            {
                PlayerStats.PlayerStatsSingleton.totalShotsFired++;
                projectile.transform.rotation = Camera.main.transform.rotation;
            }


            isFiring = true;
            StartCoroutine(FireTimer());
        }
    }

    public void Reload()
    {
        if (isReloading)
        {
            return;
        }

        if (curAmmoInMag == magazineCapacity)
        {
            return;
        }

        if (remainingAmmo <= 0)
        {
            return;
        }

        if (ownedByPlayer)
        {
            PlayerStats.PlayerStatsSingleton.totalReloads++;
        }

        isReloading = true;
        StartCoroutine(ReloadTimer());
    }

    private IEnumerator FireTimer()
    {
        yield return new WaitForSeconds(fireRate);

        isFiring = false;
        yield return null;
    }

    private IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = magazineCapacity - curAmmoInMag;
        int whatWeGot = Mathf.Min(neededAmmo, magazineCapacity);
        curAmmoInMag += whatWeGot;
        remainingAmmo -= whatWeGot;

        isReloading = false;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // I put weapons to Pickup layer. Because they can only be picked up by the player.
        // So just remove the collider, and make the attachments and stuff.

        Player p = other.GetComponent<Player>();
        if (p == null)
        {
            return;
        }

        p.EquipNewWeapon(this.GetComponent<Weapon>());
        ownedByPlayer = true;

        Destroy(this.GetComponent<Rotator>());

        Destroy(this.GetComponent<Collider>());
    }

    private void OnEnable()
    {
        isReloading = false;
        isFiring = false;
    }

    public bool IsReloading() { return isReloading; }
    public bool IsFiring() { return isFiring; }
}
