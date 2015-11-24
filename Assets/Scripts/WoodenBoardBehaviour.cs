using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HealthPoints))]
public class WoodenBoardBehaviour : MonoBehaviour
{
    public void OnDead()
    {
        //Destory when dead
        GameObject.Destroy(gameObject);
    }
}
