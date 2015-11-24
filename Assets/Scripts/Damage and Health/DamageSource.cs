using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageSource : MonoBehaviour
{
    public List<DamageTag> tags;
    public int damageAmount = 0;

    public bool isHot = true;
    public bool useDefaultHitDetection = true;  //If true, will deal damage when colliding with a vulnerable AbstractHealthPoints

    void OnTriggerEnter(Collider other)
    {
        //Attack any HealthPoints

        AbstractHealthPoints hp = other.GetComponent<AbstractHealthPoints>();

        if (hp != null && useDefaultHitDetection && isHot)
        {
            hp.AttackFrom(this);
        }
    }
}
