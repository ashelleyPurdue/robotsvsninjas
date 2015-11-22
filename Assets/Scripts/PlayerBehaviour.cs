using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform myCamera;

    public float interactReach = 1f;

    [SerializeField]private InteractableBehaviour selectedObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        InteractControls();
	}


    //Misc methods

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
