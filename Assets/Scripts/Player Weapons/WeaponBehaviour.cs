using UnityEngine;
using System.Collections;

public abstract class WeaponBehaviour : MonoBehaviour
{
    public bool attackButton = false;

    public virtual void OnSwapIn()
    {
        //TODO: Stuff to do when this weapon is "swapped in"
    }

    public virtual void OnSwapOut()
    {
        //TODO: Stuff to do when this weapon is "swapped out" to another one.
    }
}
