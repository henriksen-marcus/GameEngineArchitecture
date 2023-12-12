using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public List<Marker> adjacentMarkers = new();
    [SerializeField] private bool openForConnections;

    public bool OpenForConnections => openForConnections;

    public List<Vector3> GetAdjacentPositions()
    {
        return new List<Vector3>(adjacentMarkers.Select(x => x.transform.position).ToList());
    }

    private void OnDrawGizmos()
    {
        if (Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.red;
            if (adjacentMarkers.Count > 0)
            {
                foreach (var item in adjacentMarkers)
                {
                    Gizmos.DrawLine(transform.position, item.transform.position);
                }
            }
            Gizmos.color = Color.white;
        }
    }
}


