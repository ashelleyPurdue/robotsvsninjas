using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageSource : MonoBehaviour
{
    public List<DamageTag> tags;
    public float damageAmount = 0;

    public bool isHot = true;
    public bool useDefaultHitDetection = true;  //If true, will deal damage when colliding with a vulnerable AbstractHealthPoints

    void OnTriggerEnter(Collider other)
    {
        OnHealthCollisionEnter(other.GetComponent<AbstractHealthPoints>());
    }
    
    void OnCollisionEnter(Collision col)
    {
        OnHealthCollisionEnter(col.collider.GetComponent<AbstractHealthPoints>());
    }
    
    //Misc methods
    private void OnHealthCollisionEnter(AbstractHealthPoints hp)
    {
        Debug.Log("DamageSource: OnHealthCollisionEnter");
        
        //Attack any HealthPoints
        if (hp != null && useDefaultHitDetection && isHot)
        {
            hp.AttackFrom(this);
        }
    }
}
