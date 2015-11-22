using UnityEngine;
using System.Collections;

public class TestInteractableBehaviour : MonoBehaviour, IInteractable
{
    public string GetHighlightText()
    {
        return "Interact with test object.";
    }

    public void Interact()
    {
        //Debug log something
        Debug.Log("Interacted.");
    }
}
