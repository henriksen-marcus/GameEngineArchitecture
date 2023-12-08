using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintBounds : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnDrawGizmosSelected()
    {
        if (_renderer != null) print(_renderer.bounds.size);
    }
}
