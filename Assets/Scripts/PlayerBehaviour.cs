using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
	public static PlayerBehaviour instance = null;
	
    public Transform myCamera;

    //Interaction
    public float interactReach = 1f;
    [SerializeField]
    private InteractableBehaviour selectedObject;

    //Weapons
    public WeaponBehaviour RightHandWeapon
    {
        get { return rightHandWeaponWheel[currentRightHandWeapon]; }
    }
    public WeaponBehaviour[] rightHandWeaponWheel;
    private int currentRightHandWeapon = 0;

    private bool cursorCaptured;

    //Events
	void Awake()
    {
		//Set the instance
		instance = this;
		
        //Swap to the first weapon.
        RightHandWeapon.OnSwapIn();

		//Lock the cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        cursorCaptured = true;
	}
	
	void Update ()
    {
        InteractControls();
        WeaponSwapControls();

        CaptureCursor();

        //Send the attackButton data to the right hand weapon
        if (RightHandWeapon != null)
        {
            RightHandWeapon.attackButton = Input.GetButton("Fire1");
        }
	}


    //Misc methods

    private void CaptureCursor()
    {
        //Handles capturing of the cursor.

        //Decide if the cursor should be captured
        if (cursorCaptured)
        {
            //If the "Cancel" button is pressed, release the cursor.
            if (Input.GetButtonDown("Cancel"))
            {
                cursorCaptured = false;
            }
        }
        else
        {
            //If any of the major buttons are pressed, capture the cursor.
            string[] majorButtons = {"Fire1", "Fire2"};
            foreach (string button in majorButtons)
            {
                if (Input.GetButtonDown(button))
                {
                    cursorCaptured = true;
                }
            }

            //If the movement axies are pressed, capture the cursor
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                cursorCaptured = true;
            }
        }

        //Capture the cursor if it should be.
        Cursor.visible = !cursorCaptured;
        if (cursorCaptured)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void WeaponSwapControls()
    {
        //TODO: Swap weapons
    }
    
    private void InteractControls()
    {
        //Select the object being pointed at
        SelectObject();

        //Interact with the selected object
        if (Input.GetButtonDown("Interact") && selectedObject != null)
        {
            selectedObject.Interact();
        }
    }

    private void SelectObject()
    {
        //Selects an object the player is pointing at.

        selectedObject = null;

        RaycastHit hit;
        if (Physics.Raycast(myCamera.position, myCamera.forward, out hit, interactReach))
        {
            InteractableBehaviour obj = hit.transform.GetComponent<InteractableBehaviour>();
            if (obj != null)
            {
                selectedObject = obj;
            }
        }
    }
}
