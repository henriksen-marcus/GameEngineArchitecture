using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Raycast");
            var ray = new Ray(transform.position, transform.forward);
            var hit = Physics.Raycast(ray, out var hitInfo, 100f);
            if (hit) print(hitInfo.collider.gameObject.name);
            Debug.DrawRay(transform.position, transform.forward* 100f, Color.red, 1f);
        }
    }
}
