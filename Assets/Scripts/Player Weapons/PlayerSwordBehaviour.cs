using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSwordBehaviour : WeaponBehaviour
{
    public enum State {ready, swinging, recovering, coolingDown, swappedOut};
    private State currentState = State.ready;

    private delegate void StateMethod();
    private Dictionary<State, StateMethod> stateMethods = new Dictionary<State, StateMethod>();

    private float swingTime = 0.25f;
    private float recoverTime = 0.5f;
    private float cooldownTime = 0.2f;

    private float timer = 0f;
    private bool inFixedUpdate = false;


	//Events

	void Update ()
    {
	    
	}
}
