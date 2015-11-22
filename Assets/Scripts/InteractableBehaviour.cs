using UnityEngine;
using System.Collections;

public class InteractableBehaviour : MonoBehaviour
{
    public string highlightText = "interact with object";

    public void Interact()
    {
        BroadcastMessage("OnInteract");
    }
}
