﻿using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour
{
    public float damageAmount = 0.4f;
    public float fireRate = 5f;         //Bullets per second
    public float fireVelocity = 100f;
    public float bulletLifetime = 1f;
    public float accuracyAngle = 10f;   //The smaller this angle, the less the bullets randomly deviate

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
        bulletObj.transform.forward = GetBulletDirection(accuracyAngle);

        //Set the bullet's velocity
        bulletRigidbody.velocity = bulletObj.transform.forward * fireVelocity;
    }

    private Vector3 GetBulletDirection(float angle)
    {
        //Returns the direction the bullet should be going.  Random, but accurate within the given angle.
        
        //Find the direction of the bullet in local space.
        Vector3 localDir = Vector3.one;
        
        localDir = Quaternion.Euler(Random.Range(0, angle / 2), 0 , 0) * localDir;
        localDir = Quaternion.Euler(0, 0, Random.Range(0, 360)) * localDir;
        
        //Return that direction in local space
        return transform.localToWorldMatrix * localDir;
    }
}
