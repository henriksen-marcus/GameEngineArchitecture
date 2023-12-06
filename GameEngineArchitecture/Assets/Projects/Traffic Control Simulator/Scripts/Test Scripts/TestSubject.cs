using System;
using UnityEngine;

public class TestSubject : MonoBehaviour
{
    public event Action MyEvent;

    protected virtual void OnMyEvent() => MyEvent?.Invoke();
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) OnMyEvent();
    }
}
