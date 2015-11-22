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
	void Update () {
        SelectObject();
	}


    //Misc methods

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
