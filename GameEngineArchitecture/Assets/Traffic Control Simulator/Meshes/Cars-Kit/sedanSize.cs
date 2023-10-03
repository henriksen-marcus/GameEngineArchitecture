using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sedanSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var renderer = GetComponent<MeshRenderer>();
        var size = renderer.bounds.size;
        print(size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
