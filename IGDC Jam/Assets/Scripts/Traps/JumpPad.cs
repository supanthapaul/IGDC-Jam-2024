using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float upwardForce;
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        if(other.TryGetComponent(out FPController controller))
        {
            controller.JumpPadLogic(upwardForce);
        }

    }
}
