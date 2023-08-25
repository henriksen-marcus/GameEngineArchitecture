using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SineMovement : MonoBehaviour
{

    [SerializeField] private float amplitude = 1;
    [SerializeField] private float waveSpeed = 2;
    [SerializeField] private float movementSpeed = 0.5f;
    private Vector3 _initialPosition;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var xPos = transform.position.x + movementSpeed * Time.deltaTime;
        var yPos = _initialPosition.y + (Mathf.Sin(Time.realtimeSinceStartup * waveSpeed) * amplitude);
        transform.position = new Vector3(xPos, yPos, _initialPosition.z);
    }
}
