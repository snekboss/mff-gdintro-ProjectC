using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamagable
{
    public float projectileDamage;
    public float projectileSpeed;
    public float selfDestructTimer;
    Rigidbody rbody;

    Collider col;
    void Start()
    {
        rbody = this.gameObject.AddComponent<Rigidbody>();
        rbody.useGravity = false;
        rbody.AddForce(rbody.transform.forward * projectileSpeed, ForceMode.VelocityChange);

        col = this.GetComponent<Collider>();
        col.isTrigger = true;

        Destroy(this.gameObject, selfDestructTimer); // TODO: I don't know if doing this prematurely is risky.
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamagable dmgable = other.gameObject.GetComponent<IDamagable>();
        if (dmgable != null)
        {
            dmgable.GetDamaged(projectileDamage);
        }

        Destroy(this.gameObject);
    }

    public void GetDamaged(float amount)
    {
        Destroy(this.gameObject);
    }
}
