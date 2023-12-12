using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadHelper : MonoBehaviour
{
    [SerializeField] protected List<Marker> carMarkers;
    [SerializeField] protected bool isCorner;
    [SerializeField] protected bool hasCrosswalks;
    [SerializeField] private Marker incoming, outgoing;
    
    private float approximateThresholdCorner = 0.3f;

    public List<Marker> GetAllCarMarkers => carMarkers;

    public virtual Marker GetPositionForCarToSpawn(Vector3 nextPathPosition)
    {
        return outgoing;
    }

    public virtual Marker GetPositionForCarToEnd(Vector3 previousPathPosition)
    {
        return incoming;
    }

    protected Marker GetClosestMarkerTo(Vector3 position, List<Marker> markers, bool isCorner = false)
    {
        if (isCorner)
        {
            foreach (var marker in markers)
            {
                var direction = (marker.transform.position - position).normalized;
                
                if (Mathf.Abs(direction.x) < approximateThresholdCorner || Mathf.Abs(direction.z) < approximateThresholdCorner)
                    return marker;
            }
            
            return null;
        }
        
        Marker closestMarker = null;
        float distance = float.MaxValue;
        foreach (var marker in markers)
        {
            var markerDistance = Vector3.Distance(position, marker.transform.position);
            if (distance > markerDistance)
            {
                distance = markerDistance;
                closestMarker = marker;
            }
        }
        
        return closestMarker;
    }

    public Vector3 GetClosestCarMarkerPosition(Vector3 currentPosition)
    {
        return GetClosestMarkerTo(currentPosition, carMarkers, false).transform.position;
    }
}

