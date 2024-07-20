using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockLookAt : MonoBehaviour
{

    private Vector3 lookAtVector;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.playerController.transform;
    }
    // Update is called once per frame
    void Update()
    {
        lookAtVector = new(playerTransform.position.x , 0f, playerTransform.position.z);
        transform.LookAt(lookAtVector, Vector3.up);    
    }
}
