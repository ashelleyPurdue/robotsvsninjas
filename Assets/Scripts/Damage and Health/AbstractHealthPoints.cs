﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractHealthPoints : MonoBehaviour
{
    public List<DamageTag> invulnerableTo = new List<DamageTag>();
    public List<DamageTag> vulnerableTo = new List<DamageTag>();
    public List<DamageSource> ignoreList = new List<DamageSource>();

    public float cooldownTime = 0f;

    protected bool isCoolingDown = false;
    protected float timer = 0f;

    public virtual bool IsVulnerableTo(DamageSource src)
    {
        //Returns if this object is vulnerable to the given damage source according to only the vulnerability lists.

        //If the vulnerable list has something in it, then the src must contain one of tags in vulnerableTo
        if (vulnerableTo.Count > 0)
        {
            //Return true if there was atleast one match
            foreach (DamageTag tag in src.tags)
            {
                if (vulnerableTo.Contains(tag))
                {
                    return true;
                }
            }

            //If there were no matches, return false.
            return false;
        }

        //If the vulernableTo list is empty, then src must contain one tag that *isn't* on the invulnerableTo list.
        foreach(DamageTag tag in src.tags)
        {
            //Return true if this tag isn't there
            if (!invulnerableTo.Contains(tag))
            {
                return true;
            }
        }

        //If all of the tags are on the invulnerableTo list, return false.
        return false;
    }

    public virtual bool CanBeHurtBy(DamageSource src)
    {
        //Returns if this object can be hurt by the given source, taking into account both the vulnerability lists, the ignore list, and the cooldown state.

        return (!isCoolingDown) && (!ignoreList.Contains(src)) && IsVulnerableTo(src);
    }

    public abstract void DealDamage(int amount);    //Deals the given amount of damage.  Different implementations can have different ways of handling this.

    public virtual void AttackFrom(DamageSource src)
    {
        //Gets this object attacked by src.  Deals damage if vulnerable.

        if (CanBeHurtBy(src))
        {
            DealDamage(src.damageAmount);
        }
    }
}
