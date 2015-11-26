using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionCone : MonoBehaviour
{
    public float radius = 100f;
    public float visionAngle = 114f;    //In degrees

    public List<TargetType> Scan<TargetType>()
    {
        //Returns all visible colliders that also have the target type.

        List<TargetType> output = new List<TargetType>();

        //Get all colliders within the vision radius
        Collider[] inRange = Physics.OverlapSphere(transform.position, radius);

        //Keep only the colliders that have a TargetType component, line of sight, and are within the viewing angle
        foreach (Collider c in inRange)
        {
            TargetType targetComponent = c.GetComponent<TargetType>();

            //Skip this one if it isn't of the target type.
            if (targetComponent == null)
            {
                continue;
            }

            //Skip this one if the angle isn't within the FOV
            Vector3 diff = c.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, diff);

            if (angle > visionAngle / 2)
            {
                continue;
            }

            //Skip this one if the line-of-sight is blocked(that is: if the closest raycast hit isn't c)
            RaycastHit[] hits = Physics.RaycastAll(transform.position, diff.normalized, diff.magnitude);
            float closestDistance = float.MaxValue;
            Collider closestCollider = null;

            foreach (RaycastHit h in hits)
            {
                if (h.distance < closestDistance)
                {
                    closestDistance = h.distance;
                    closestCollider = h.collider;
                }
            }

            if (closestCollider != c)
            {
                continue;
            }

            //If all the checks were passed, add it to the list
            output.Add(targetComponent);
        }

        return output;
    }
}
