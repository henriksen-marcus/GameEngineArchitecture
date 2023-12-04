using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    // We need a separate field here to avoid recursion in the private setter.
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null) SetupInstance();
            }
            return _instance;
        }
        private set => _instance = value;
    }

    protected virtual void Awake()
    {
        // Remove dupes
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
    }

    private static void SetupInstance()
    {
        _instance = (T)FindObjectOfType(typeof(T));
        if (_instance == null)
        {
            GameObject gameObj = new();
            gameObj.name = typeof(T).Name;
            _instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }
}
