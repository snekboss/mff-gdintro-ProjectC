using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public int curAmmoInMag;
    public int magazineCapacity;
    public int remainingAmmo;

    [Range(0.0f, 3.0f)]
    public float fireRate;

    [Range(0.0f, 5.0f)]
    public float reloadTime;

    // TODO: SetActive will cause coroutines to get messed up. Gotta think of something.
    bool isFiring;
    bool isReloading;


    public void Fire()
    {
        // TODO: Implement "out of ammo, gotta reload" cases, etc.

        if (!isFiring && !isReloading)
        {
            isFiring = true;
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = projectileSpawnPoint.position;
            projectile.transform.rotation = projectileSpawnPoint.transform.rotation;

            StartCoroutine(FireTimer());
        }
    }

    public void Reload()
    {
        // TODO: Implement "no ammo at all, I can't even reload" cases, etc.

        if (isReloading || !isFiring)
        {
            return;
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
        // looks pretty similar to FireTimer, ey? ringing any alarm bells? not yet
        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // I put weapons to Pickup layer. Because they can only be picked up by the player.
        // So just remove the collider, and make the attachments and shit.

        Player p = other.GetComponent<Player>();
        p.EquipNewWeapon(this.GetComponent<Weapon>());

        Destroy(this.GetComponent<Collider>());
    }
}
