using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
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


    //Events
	void Awake()
    {
        //Swap to the first weapon.
        RightHandWeapon.OnSwapIn();
	}
	
	void Update ()
    {
        InteractControls();
        WeaponSwapControls();

        //Send the attackButton data to the right hand weapon
        if (RightHandWeapon != null)
        {
            RightHandWeapon.attackButton = Input.GetButton("Fire1");
        }
	}


    //Misc methods

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
