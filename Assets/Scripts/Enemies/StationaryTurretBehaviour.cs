using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(HealthPoints))]
public class StationaryTurretBehaviour : MonoBehaviour
{
    //Tweaking

    //States
    public enum State {searching, attacking}
    private State currentState = State.searching;

    //Events

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
