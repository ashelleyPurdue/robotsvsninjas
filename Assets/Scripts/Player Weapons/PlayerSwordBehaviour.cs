using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSwordBehaviour : WeaponBehaviour
{
    //Configuration
    //Timing parameters
    private float swingTime = 0.25f;
    private float recoverTime = 0.3f;

    //Swing position parameters
    private float swingAngle = 67.5f;
    private float swingDistance = 0.75f;
    private float swingHeight = 0f;


    //State-machine related
    public enum State {ready, swinging, recovering, swappedOut};
    private State currentState = State.ready;

    private delegate void StateMethod();
    private Dictionary<State, StateMethod> stateMethods = new Dictionary<State, StateMethod>();

    //Animation positions
    private Vector3 readyPos;
    private Quaternion readyRot;

    private Vector3 swingStartPos;
    private Quaternion swingStartRot;

    private Vector3 swingEndPos;
    private Quaternion swingEndRot;

    //Misc private fields
    private float timer = 0f;

    private bool inFixedUpdate = false;

    private bool attackButtonPrev = false;


	//Events

    void Awake()
    {
        //Pre-compute the animation stages
        readyPos = transform.localPosition;
        readyRot = transform.localRotation;

        //Calculate the swing start and end positions
        float midAngle = 90f * Mathf.Deg2Rad;
        float halfAngle = Mathf.Deg2Rad * (swingAngle / 2);

        float startAngle = midAngle - halfAngle;
        float endAngle = midAngle + halfAngle;

        swingStartPos = new Vector3(Mathf.Cos(startAngle), 0, Mathf.Sin(startAngle)) * swingDistance;
        swingEndPos = new Vector3(Mathf.Cos(endAngle), 0, Mathf.Sin(endAngle)) * swingDistance;

        //Calculate start and end rotations
        swingStartRot = Quaternion.Euler(90, swingAngle / 2, 0);
        swingEndRot = Quaternion.Euler(90, -swingAngle / 2, 0);

        //Set up the state machine
        stateMethods.Add(State.ready, WhileReady);
        stateMethods.Add(State.swinging, WhileSwinging);
        stateMethods.Add(State.recovering, WhileRecovering);
        stateMethods.Add(State.swappedOut, WhileSwappedOut);
    }

	void Update ()
    {
        //Call the non-fixed portion of the current state
        inFixedUpdate = false;
        stateMethods[currentState]();

        //Save attackButtonPrev
        attackButtonPrev = attackButton;
	}

    void FixedUpdate()
    {
        //Call the fixed-update portion of the current state
        inFixedUpdate = true;
        stateMethods[currentState]();
    }


    //State methods

    private void WhileReady()
    {
        if (!inFixedUpdate)
        {
            //Keep at the ready pos
            transform.localPosition = readyPos;
            transform.localRotation = readyRot;

            //Swing when the attack button is pressed.
            if (attackButton && !attackButtonPrev)
            {
                currentState = State.swinging;
                timer = 0f;
            }
        }
    }

    private void WhileSwinging()
    {
        if (inFixedUpdate)
        {
            //Swing
            timer += Time.deltaTime;

            transform.localPosition = Vector3.Slerp(swingStartPos, swingEndPos, timer / swingTime);
            transform.localRotation = Quaternion.Slerp(swingStartRot, swingEndRot, timer / swingTime);

            //Start recovering when done
            if (timer >= swingTime)
            {
                currentState = State.recovering;
                timer = 0f;
            }
        }
    }

    private void WhileRecovering()
    {
        if (inFixedUpdate)
        {
            //Recover
            timer += Time.deltaTime;

            transform.localPosition = Vector3.Slerp(swingEndPos, readyPos, timer / recoverTime);
            transform.localRotation = Quaternion.Slerp(swingEndRot, readyRot, timer / recoverTime);

            //Back to ready
            if (timer >= recoverTime)
            {
                currentState = State.ready;
                transform.localPosition = readyPos;
                transform.localRotation = readyRot;
                timer = 0f;
            }
        }
    }

    private void WhileSwappedOut()
    {
    }
}
