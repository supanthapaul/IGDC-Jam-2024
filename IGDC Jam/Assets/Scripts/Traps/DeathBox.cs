using Health_System;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHealth>(out var damagableObject))
        {
            damagableObject.TakeDamage(-50000);
        }
    }
}
