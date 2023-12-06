using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryTest : Factory<MovingCube>
{
    public IProduct GetProduct(Vector3 position)
    {
        var product = Instantiate(productPrefab, position, Quaternion.identity);
        product.gameObject.transform.position = position;
        product.Initialize();
        product.gameObject.SetActive(true);
        return product;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetProduct(new Vector3(0,0.5f,0));
        }
    }
}
