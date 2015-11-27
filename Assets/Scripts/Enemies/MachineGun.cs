using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour
{
    public float damageAmount = 0.4f;
    public float fireRate = 5f;     //Bullets per second
    public float fireVelocity = 100f;
    public float bulletLifetime = 1f;

    public bool triggerPulled = false;

    private float fireInterval;
    private float fireTimer = 0;

	//Events
    void Awake()
    {
        //Calculate the fire interval
        fireInterval = 1 / fireRate;
    }

    void FixedUpdate()
    {
        //Count down the fire timer
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

        //If trigger is pulled and next bullet is ready, fire
        if (triggerPulled && fireTimer <= 0)
        {
            fireTimer = fireInterval;
            Fire();
        }
    }

    //Interface


    //Misc methods
    private void Fire()
    {
        //TODO: Allow random spread

        //Create the bullet
        GameObject bulletObj = (GameObject)Instantiate(Resources.Load("bullet_prefab"));
        DamageSource bulletSrc = bulletObj.GetComponent<DamageSource>();
        BulletBehaviour bullet = bulletObj.GetComponent<BulletBehaviour>();
        Rigidbody bulletRigidbody = bulletObj.GetComponent<Rigidbody>();

        //Set the bullet's location
        bulletObj.transform.position = transform.position;

        //Set the bullet's damage amount
        bulletSrc.damageAmount = damageAmount;
        bulletSrc.isHot = true;

        //Aim the bullet
        bulletObj.transform.forward = transform.forward;

        //Set the bullet's velocity
        bulletRigidbody.velocity = bulletObj.transform.forward * fireVelocity;
    }
}
