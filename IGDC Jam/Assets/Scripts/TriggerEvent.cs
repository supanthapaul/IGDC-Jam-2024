using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField]
    private bool triggerOnce = true;

    [SerializeField] private UnityEvent onTriggerEnter;
    
    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (_triggered && triggerOnce)
            return;
        onTriggerEnter?.Invoke();
        _triggered = true;
    }
}
