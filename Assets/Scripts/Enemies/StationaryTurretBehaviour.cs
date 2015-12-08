using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(VisionCone))]
public class StationaryTurretBehaviour : MonoBehaviour
{
    public MachineGun machineGun;

    //Tweaking
    private float searchRotSpeed = 45f;
    private float rotSpeed = 180f;

    //States
    public enum State {searching, attacking}
    private State currentState = State.searching;
    private delegate void StateMethod();
    private Dictionary<State, StateMethod> stateMethods = new Dictionary<State, StateMethod>();

    //Misc fields
    private VisionCone visionCone;
    private PlayerBehaviour target;

    private Quaternion targetRot; 

    //Events
    void Awake()
    {
        //Set up the state machine
        stateMethods.Add(State.searching, WhileSearching);
        stateMethods.Add(State.attacking, WhileAttacking);

        //Get components
        visionCone = GetComponent<VisionCone>();

        //Start the targetRot at current rot
        targetRot = transform.rotation;
    }

    void FixedUpdate()
    {
        stateMethods[currentState]();

    }

    void OnDead()
    {
        //Destroy self
        GameObject.Destroy(gameObject);
    }

    //State methods
    private void WhileSearching()
    {
        //Stop firing
        machineGun.triggerPulled = false;

        //Rotate in a circle
        Vector3 euler = targetRot.eulerAngles;
        euler.x = 0;
        euler.y += searchRotSpeed * Time.deltaTime;
        targetRot.eulerAngles = euler;

        //Rotate towards the target rot
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        //Scan for a target
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
        Vector3 targetDiff = target.transform.position - transform.position;
        targetRot = Quaternion.LookRotation(targetDiff.normalized, Vector3.up);

        //Rotate towards the target rot
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        //Fire at the target
        machineGun.triggerPulled = true;

        //If the target can't be seen anymore, go back into searching mode
        if (!visionCone.Scan<PlayerBehaviour>().Contains(target))
        {
            target = null;
            currentState = State.searching;
        }
    }


}
