using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DamageSource))]
public class MachineGun : MonoBehaviour
{
    public float fireRate = 5f;         //Bullets per second
    public float fireVelocity = 100f;
    public float bulletLifetime = 1f;
    public float accuracyAngle = 10f;   //The smaller this angle, the less the bullets randomly deviate

    public bool triggerPulled = false;

    private float fireInterval;
    private float fireTimer = 0;

    private DamageSource damageSrc;


	//Events
    void Awake()
    {
        //Calculate the fire interval
        fireInterval = 1 / fireRate;

        //Get the damage source
        damageSrc = GetComponent<DamageSource>();
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
        //Create the bullet
        GameObject bulletObj = (GameObject)Instantiate(Resources.Load("bullet_prefab"));
        DamageSource bulletSrc = bulletObj.GetComponent<DamageSource>();
        BulletBehaviour bullet = bulletObj.GetComponent<BulletBehaviour>();
        Rigidbody bulletRigidbody = bulletObj.GetComponent<Rigidbody>();

        //Set the bullet's location
        bulletObj.transform.position = transform.position;

        //Configure the bullet's damage source.
        damageSrc.CopyDataTo(bulletSrc);
        bulletSrc.isHot = true;
        bulletSrc.useDefaultHitDetection = true;
        
        //Make sure the bullet doesn't hurt this object
        AbstractHealthPoints myHealth = GetComponent<AbstractHealthPoints>();
        if (myHealth != null)
        {
            bulletSrc.ignoreList.Add(myHealth);
        }
        
        //Aim the bullet
        bulletObj.transform.forward = GetBulletDirection(accuracyAngle);

        //Set the bullet's velocity
        bulletRigidbody.velocity = bulletObj.transform.forward * fireVelocity;
    }

    private Vector3 GetBulletDirection(float angle)
    {
        //Returns the direction the bullet should be going.  Random, but accurate within the given angle.
        
        //Find the direction of the bullet in local space.
        Vector3 localDir = new Vector3(0, 0, 1);
        
        localDir = Quaternion.Euler(Random.Range(0, angle / 2), 0 , 0) * localDir;
        localDir = Quaternion.Euler(0, 0, Random.Range(0, 360)) * localDir;
        
        //Return that direction in local space
        return transform.localToWorldMatrix * localDir;
    }
}
