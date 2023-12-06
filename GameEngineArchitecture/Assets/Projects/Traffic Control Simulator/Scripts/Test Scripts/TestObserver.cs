using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestObserver : MonoBehaviour
{
    [SerializeField] private GameObject _subject;
    
    void Awake()
    {
        var subjectScript = _subject?.GetComponent<TestSubject>();
        if (subjectScript != null) subjectScript.MyEvent += OnNotify;
    }

    void OnNotify()
    {
        print("Notified");
    }
}
