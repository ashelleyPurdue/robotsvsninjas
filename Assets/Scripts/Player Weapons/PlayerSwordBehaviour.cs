using UnityEngine;
using System.Collections;

public class PlayerSwordBehaviour : WeaponBehaviour
{
    public enum State {ready, swinging, recovering, coolingDown, swappedOut};
    private State currentState = State.ready;

    private float swingTime = 0.25f;
    private float recoverTime = 0.5f;
    private float cooldownTime = 0.2f;

    private float timer = 0f;
    private bool inFixedUpdate = false;

	// Update is called once per frame
	void Update ()
    {
	    
	}
}
