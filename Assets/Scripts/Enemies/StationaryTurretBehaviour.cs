using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(HealthPoints))]
[RequireComponent(typeof(VisionCone))]
public class StationaryTurretBehaviour : MonoBehaviour
{
    //States
    public enum State {searching, attacking}
    private State currentState = State.searching;
    private delegate void StateMethod();
    private Dictionary<State, StateMethod> stateMethods = new Dictionary<State, StateMethod>();


    //Misc fields
    private VisionCone visionCone;
    private PlayerBehaviour target;

    //Events
    void Awake()
    {
        //Set up the state machine
        stateMethods.Add(State.searching, WhileSearching);
        stateMethods.Add(State.attacking, WhileAttacking);

        //Get the cone
        visionCone = GetComponent<VisionCone>();
    }

    void FixedUpdate()
    {
        stateMethods[currentState]();
    }

    //State methods
    private void WhileSearching()
    {
        //Scane for a target
        List<PlayerBehaviour> hits = visionCone.Scan<PlayerBehaviour>();

        //Start attacking the first hit in the list
        if (hits.Count > 0)
        {
            target = hits[0].GetComponent<PlayerBehaviour>();
            currentState = State.attacking;

            //TODO: Play some kind of sound effect
        }
    }

    private void WhileAttacking()
    {
        //Look at target
        transform.LookAt(target.transform);

        //TODO: Fire at the target

        //If the target can't be seen anymore, go back into searching mode
        if (!visionCone.Scan<PlayerBehaviour>().Contains(target))
        {
            target = null;
            currentState = State.searching;
        }
    }
}
