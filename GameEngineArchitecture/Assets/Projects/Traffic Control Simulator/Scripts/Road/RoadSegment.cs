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
        if (Vector3.Dot(forward, transform.forward) > 0)
        {
            // Going in our direction
            return exitRightMarker.position;
        }
        else
        {
            // Going in opposite direction
            return exitLeftMarker.position;
        }
    }

    public Vector3 GetInfinityMarker(Vector3 forward)
    {
        if (Vector3.Dot(forward, transform.forward) > 0)
        {
            // Going in our direction
            return exitRightMarker.position + transform.forward * 100000f;
        }
        else
        {
            // Going in opposite direction
            return exitLeftMarker.position - transform.forward * 100000f;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + transform.forward * 5f);
    }
}
