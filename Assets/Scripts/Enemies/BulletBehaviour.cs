using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DamageSource))]
public class BulletBehaviour : MonoBehaviour
{
    public float lifeTime = 1f;

    void FixedUpdate()
    {
        //Destroy when out of time
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject.Destroy(gameObject, 0.01f);
    }
    
    void OnDealDamage()
    {
        //Destroy
        GameObject.Destroy(gameObject);
    }
}
