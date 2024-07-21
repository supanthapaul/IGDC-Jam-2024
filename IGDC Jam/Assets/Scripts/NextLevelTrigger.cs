using Health_System;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&TryGetComponent(out IHealth _))
        {
            GameManager.Instance.LevelCompleted();
        }
    }
}
