using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    [SerializeField] private Transform entryLeftMarker;
    [SerializeField] private Transform entryRightMarker;
    [SerializeField] private Transform exitLeftMarker;
    [SerializeField] private Transform exitRightMarker;

    public Vector3 GetMarker(Vector3 forward)
    {
        if (Vector3.Dot(Vector3.forward, transform.forward) > 0)
        {
            // Going in our direction, give exit points
            return exitRightMarker.position;
        }
        else
        {
            // Going in opposite direction, give entry points
            return exitLeftMarker.position;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + transform.forward * 5f);
    }
}
