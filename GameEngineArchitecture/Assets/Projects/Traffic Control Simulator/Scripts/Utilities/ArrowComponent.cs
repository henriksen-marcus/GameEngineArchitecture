using UnityEngine;

/// <summary>
/// Gizmo component that draws a direction in the editor.
/// </summary>
public class ArrowComponent : MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    [SerializeField] private float length = 1;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Color color = Color.white;
    
    void OnDrawGizmos()
    {
        Gizmos.color = color;
        var p = transform.position;
        Gizmos.DrawLine(p + offset, p + offset + transform.InverseTransformDirection(direction) * length);
        Gizmos.DrawWireSphere(p + offset, length * 0.05f);
    }

}
