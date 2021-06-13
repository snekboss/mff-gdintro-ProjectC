using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MePickup : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        Ammo,
    }

    [SerializeField]
    public PickupType pickupType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        Player pl = other.GetComponent<Player>();
        if (pl == null)
        {
            return;
        }

        bool destroy = false;

        if (pickupType == PickupType.Health)
        {
            pl.RefillHealth();
            destroy = true;
        }
        else if (pickupType == PickupType.Ammo)
        {
            destroy = pl.RefillAmmo();
        }

        if (destroy)
        {
            Destroy(this.gameObject);
        }
    }
}
