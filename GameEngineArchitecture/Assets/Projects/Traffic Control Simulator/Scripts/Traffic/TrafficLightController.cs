using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for a group of traffic lights in a crossing.
/// Subdivides four traffic lights into four directions.
/// Opposite directions switch simultaneously.
/// </summary>
[RequireComponent(typeof(ArrowComponent))]
public class TrafficLightController : MonoBehaviour
{
    /* Directions according to the arrow component. */
    [SerializeField] private GameObject _frontTrafficPole;
    [SerializeField] private GameObject _backTrafficPole;
    [SerializeField] private GameObject _rightTrafficPole;
    [SerializeField] private GameObject _leftTrafficPole;
}
