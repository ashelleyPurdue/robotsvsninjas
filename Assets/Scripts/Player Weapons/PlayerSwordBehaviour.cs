using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSwordBehaviour : WeaponBehaviour
{
    //Configuration
    //Timing parameters
    private float swingTime = 0.2f;
    private float recoverAnimationTime = 0.5f;  //How long the recovery animation lasts
    private float recoverTime = 0.3f;           //The rest of the recovery animation can be skipped after this time period.

    //Swing position parameters
    private float swingAngle = 67.5f;
    private float swingDistance = 1;//0.75f;
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

    private Vector3 recoverStartPos;
    private Quaternion recoverStartRot;

    //Misc private fields
    private float timer = 0f;

    private bool inFixedUpdate = false;

    private bool attackButtonPrev = false;
    private bool attackBuffered = false;

    private DamageSource damageSrc;


	//Events

    void Awake()
    {
        //Get the damageSrc
        damageSrc = GetComponent<DamageSource>();

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
        //Buffer an attack if the button is pressed
        if (attackButton == true && attackButtonPrev == false)
        {
            attackBuffered = true;
        }

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

    void OnDealDamage()
    {
        //Start recovering
        StartRecovering();
    }

    //Misc methods

    private void StartSwinging()
    {
        //Start swinging

        currentState = State.swinging;
        timer = 0;
        attackBuffered = false;
    }

    private void StartRecovering()
    {
        //Start recovering

        recoverStartPos = transform.localPosition;
        recoverStartRot = transform.localRotation;

        currentState = State.recovering;
        timer = 0f;
    }


    //State methods

    private void WhileReady()
    {
        if (!inFixedUpdate)
        {
            //Make sure the damageSrc is off
            damageSrc.isHot = false;

            //Keep at the ready pos
            transform.localPosition = readyPos;
            transform.localRotation = readyRot;

            //If an attack has been buffered, start swinging
            if (attackBuffered)
            {
                StartSwinging();
            }
        }
    }

    private void WhileSwinging()
    {
        if (inFixedUpdate)
        {
            //Make sure the damageSrc is on
            damageSrc.isHot = true;

            //Swing
            timer += Time.deltaTime;

            transform.localPosition = Vector3.Slerp(swingStartPos, swingEndPos, timer / swingTime);
            transform.localRotation = Quaternion.Slerp(swingStartRot, swingEndRot, timer / swingTime);

            //Start recovering when done
            if (timer >= swingTime)
            {
                transform.localPosition = swingEndPos;
                transform.localRotation = swingEndRot;

                StartRecovering();
            }
        }
    }

    private void WhileRecovering()
    {
        //If the recoveryTime has passed and the attack button is pressed, end the animation early and swing again.

        if (inFixedUpdate)
        {
            //Make sure the damageSrc is off
            damageSrc.isHot = false;

            //Recover
            timer += Time.deltaTime;

            transform.localPosition = Vector3.Slerp(recoverStartPos, readyPos, timer / recoverAnimationTime);
            transform.localRotation = Quaternion.Slerp(recoverStartRot, readyRot, timer / recoverAnimationTime);

            //If the recoveryTime has passed and an attack has been buffered, skip the rest of the animation and start swinging.
            if (timer >= recoverTime && attackBuffered)
            {
                StartSwinging();
            }

            //Back to ready
            if (timer >= recoverAnimationTime)
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
