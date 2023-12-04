using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class ObjectPoolingTester : MonoBehaviour
{
    private ObjectPooler ObjectPooler;
    public Transform SpawnPoint;

    void Start()
    {
        // Set amount and type of objects in the editor
        ObjectPooler = GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var obj = ObjectPooler.GetObject();
            obj.transform.position = SpawnPoint.position;
        }
    }
}
